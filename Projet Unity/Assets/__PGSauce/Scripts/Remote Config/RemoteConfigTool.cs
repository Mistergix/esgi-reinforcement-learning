using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using MonKey.Extensions;
using Newtonsoft.Json;
using PGSauce.Core;
using PGSauce.Core.PGDebugging;
using PGSauce.Core.Strings;
using PGSauce.Core.Utilities;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
#pragma warning disable 162

namespace PGSauce.PGRemote
{
    [CreateAssetMenu(menuName = MenuPaths.Remote + "Remote Config Tool")]
    public class RemoteConfigTool : SOSingleton<RemoteConfigTool>
    {
        [SerializeField] private RemoteConfigContainer container;
        
        [CalledByOdin]
        public static readonly List<string> SupportedTypes = new List<string> { "string", "bool", "float", "int", "long", "json" };
        [CalledByOdin]
        public static List<string> SupportedKeys => Instance.GetSettingsKeys();

        public const string BaseUrl = "https://remote-config-api.uca.cloud.unity3d.com/";
        public const string ApiLogin = "https://api.unity.com/v1/core/api/login";

        private HttpClient _client;
        private AccessToken _accessToken;
        private List<Value> _localSettings;

        public string Email => "bigcattostudio@gmail.com";
        public string Password => "f@Hx62W6c!nd";
        private bool HasAccessToken { get; set; }
        public RemoteConfigData.Configs DefaultConfigs { get; private set; } 
        public EnvironmentData DefaultEnvironment { get; private set; } 
        private string AccessTokenValue => _accessToken.access_token;
        private bool IsInit { get; set; }

        [ShowIf("IsInit"), ShowInInspector, PropertyOrder(1)]
        public string ProjectId
        {
            get
            {
#if UNITY_EDITOR
                return CloudProjectSettings.projectId;
#endif
                return "";
            }
        }

        public RemoteConfigContainer Container => container;


        [Button(ButtonSizes.Large)]
        public void Init()
        {
            IsInit = false;
            GetAccessToken();
            IsInit = true;

            GetDefaultEnvironment();
            GetDefaultConfig();
            DeleteCampaigns();
            UpdateConfig();
            CreateCampaigns();
        }

        private void CreateCampaigns()
        {
            var localCampaigns = container.Campaigns;
            HttpConfigure();
            var remoteCampaigns = GetAllCampaigns();
            foreach (var campaign in localCampaigns)
            {
                if (IsOnline(campaign, remoteCampaigns))
                {
                    UpdateCampaign(campaign);
                }
                else
                {
                    CreateCampaign(campaign);
                }
            }
        }

        private bool IsOnline(CampaignSo campaign, List<Campaign> remoteCampaigns)
        {
            return remoteCampaigns.Any(remoteCampaign => remoteCampaign.id.Equals(campaign.CampaignId));
        }
        
        private void UpdateCampaign(CampaignSo campaign)
        {
            var content = campaign.GetData();
            var response = _client.PutAsync($"rules/{campaign.CampaignId}?projectId={ProjectId}", content).Result;
            if (response.IsSuccessStatusCode)
            {
                PGDebug.Message($"Updated campaign {campaign.CampaignName}").Log();
            }
            else
            {
                throw ApiError(response, $"Error updating campaign {campaign.CampaignName}");
            }
        }

        private void CreateCampaign(CampaignSo campaign)
        {
            var content = campaign.GetData();
            var response = _client.PostAsync($"rules?projectId={ProjectId}", content).Result;
            if (response.IsSuccessStatusCode)
            {
                campaign.CampaignId =
                    JsonConvert.DeserializeObject<Campaign>(response.Content.ReadAsStringAsync().Result).id;
                PGDebug.Message($"Created campaign {campaign.CampaignName} with id {campaign.CampaignId}").Log();
            }
            else
            {
                throw ApiError(response, $"Error creating campaign {campaign.CampaignName}");
            }
        }

        private void DeleteCampaigns()
        {
            var localCampaigns = container.Campaigns;
            var remoteCampaigns = GetAllCampaigns();

            var toDelete = new List<Campaign>();

            foreach (var remoteCampaign in remoteCampaigns)
            {
                var contains = localCampaigns.Any(so => so.CampaignId.Equals(remoteCampaign.id));
                if (!contains)
                {
                    toDelete.Add(remoteCampaign);
                }
            }
            
            HttpConfigure();
            foreach (var campaign in toDelete)
            {
                var response = _client.DeleteAsync( $"rules/{campaign.id}?projectId={ProjectId}").Result;
                if (response.IsSuccessStatusCode)
                {
                    PGDebug.Message($"Deleted campaign {campaign.name}").Log();
                }
                else
                {
                    throw ApiError(response, $"Error deleting campaign {campaign.name}");
                }
            }
        }

        private List<Campaign> GetAllCampaigns()
        {
            HttpConfigure();
            var response = _client.GetAsync( $"configs/{DefaultConfigs.id}/rules?projectId={ProjectId}").Result;
            if (response.IsSuccessStatusCode)
            {
                PGDebug.Message($"res is {response.Content.ReadAsStringAsync().Result}").Log();
                var campaigns = JsonConvert.DeserializeObject<Campaigns>(response.Content.ReadAsStringAsync().Result).rules;
                PGDebug.Message($"Got Campaigns list ({campaigns.Count} campaigns)").Log();
                return campaigns;
            }
            throw ApiError(response, "Error getting default environment");
        }

        private void GetDefaultEnvironment()
        {
            HttpConfigure();
            var response = _client.GetAsync( $"environments/default?projectId={ProjectId}").Result;
            if (response.IsSuccessStatusCode)
            {
                DefaultEnvironment = JsonConvert.DeserializeObject<EnvironmentData>(response.Content.ReadAsStringAsync().Result);
                PGDebug.Message($"Default Environment id is  {DefaultEnvironment.id}").Log();
            }
            else
            {
                throw ApiError(response, "Error getting default environment");
            }
        }

        private void UpdateConfig()
        {
            GetLocalSettings();
            UpdateLocalSettingsWithRemoteSettings();
            UpdateNewConfig();
        }

        private void GetLocalSettings()
        {
            _localSettings = new List<Value>();
            var settingsHash = new HashSet<string>();
            foreach (var config in container.Configs)
            {
                foreach (var value in config.Values)
                {
                    if (settingsHash.Contains(value.key))
                    {
                        PGDebug.Message($"Key {value.key} is duplicated ! In {config.name}. Skip.").LogWarning();
                    }
                    else
                    {
                        _localSettings.Add(value);
                        settingsHash.Add(value.key);
                    }
                }
            }
        }

        private void UpdateNewConfig()
        {
            HttpConfigure();

            var createConfig = new CreateConfigData(DefaultEnvironment.id, _localSettings);

            var content = new StringContent(JsonConvert.SerializeObject(createConfig), System.Text.Encoding.UTF8,
                "application/json");

            var response = _client.PutAsync($"configs/{DefaultConfigs.id}?projectId={ProjectId}", content).Result;
            if (response.IsSuccessStatusCode)
            {
                PGDebug.Message($"Updated Config {DefaultConfigs.id}").Log();
            }
            else
            {
                throw ApiError(response, "Error trying to update config");
            }
        }

        private void ApiCreateConfig(CreateConfigData createConfig)
        {
            var content = new StringContent(JsonConvert.SerializeObject(createConfig), System.Text.Encoding.UTF8,
                "application/json");

            var response = _client.PostAsync($"configs?projectId={ProjectId}", content).Result;
            if (response.IsSuccessStatusCode)
            {
                DefaultConfigs = JsonConvert.DeserializeObject<RemoteConfigData.Configs>(response.Content.ReadAsStringAsync().Result);
                PGDebug.Message($"Created Config {DefaultConfigs.id}").Log();
            }
            else
            {
                throw ApiError(response, "Error trying to create new config");
            }
        }

        private void DeleteCurrentConfig()
        {
            if(DefaultConfigs.id.IsNullOrEmpty()){return;}
            HttpConfigure();
            var response = _client.DeleteAsync( $"configs/{DefaultConfigs.id}?projectId={ProjectId}").Result;
            if (response.IsSuccessStatusCode)
            {
                PGDebug.Message($"Deleted Config {DefaultConfigs.id}").Log();
                DefaultConfigs = new RemoteConfigData.Configs();
                
            }
            else
            {
                throw ApiError(response, "Error trying to delete default config");
            }
        }

        private void UpdateLocalSettingsWithRemoteSettings()
        {
            var remoteSettings = DefaultConfigs.value;
            for (var i = 0; i < _localSettings.Count; i++)
            {
                var localSetting = _localSettings[i];
                var key = localSetting.key;
                for (var j = 0; j < remoteSettings.Count; j++)
                {
                    if (!remoteSettings[j].key.Equals(key))
                    {
                        continue;
                    }

                    _localSettings[i] = remoteSettings[j];
                    break;
                }
            }
        }

        private void GetDefaultConfig()
        {
            HttpConfigure();
            var response = _client.GetAsync( $"configs?projectId={ProjectId}&environmentId={DefaultEnvironment.id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var config =
                    JsonConvert.DeserializeObject<RemoteConfigData>(response.Content.ReadAsStringAsync().Result);
                if (config.configs.Count > 0)
                {
                    DefaultConfigs = config.configs[0];
                    PGDebug.Message($"Default config id is {DefaultConfigs.id}").Log();
                    PGDebug.Log(DefaultConfigs.value);
                }
                else
                {
                    PGDebug.Message($"No default config").Log();
                    CreateEmptyConfig();
                }
                
            }
            else
            {
                throw ApiError(response, "Error trying to get default config");
            }
        }

        private void CreateEmptyConfig()
        {
            ApiCreateConfig(new CreateConfigData(DefaultEnvironment.id, new List<Value>()));
            PGDebug.Message("Created empty config").Log();
        }

        private void GetAccessToken()
        {
            HasAccessToken = false;
            HttpConfigure();
            var content = new StringContent(JsonConvert.SerializeObject(new LoginDataConnect(Email, Password, "PASSWORD")), System.Text.Encoding.UTF8,
                "application/json");
            var response = _client.PostAsync( ApiLogin , content).Result;
            if (response.IsSuccessStatusCode)
            {
                _accessToken =
                    JsonConvert.DeserializeObject<AccessToken>(response.Content.ReadAsStringAsync().Result);
                PGDebug.Message($"Access token is {AccessTokenValue}").Log();
                HasAccessToken = true;
            }
            else
            {
                throw ApiError(response, "Error trying to connect to web");
            }
        }
        
        private void HttpConfigure()
        {
            _client?.Dispose();
            _client = new HttpClient {BaseAddress = new Uri(BaseUrl)};

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (HasAccessToken)
            {
                _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessTokenValue}");
            }
        }
        
        private UnityException ApiError(HttpResponseMessage response, string message)
        {
            var error = $"{message} : {response.Content.ReadAsStringAsync().Result}";
            PGDebug.Message(error).LogError();
            return new UnityException(error);
        }
        
        private List<string> GetSettingsKeys()
        {
            var setting = container.Configs;
            var keys = new List<string>();

            foreach (var values in setting.Select(data => data.Values))
            {
                keys.AddRange(values.Select(value => value.key));
            }

            return keys;
        }

        private struct CreateConfigData
        {
            public string environmentId;
            public string type;
            public List<Value> value;

            public CreateConfigData(string environmentId, List<Value> value) : this()
            {
                this.environmentId = environmentId;
                this.value = value;
                type = "settings";
            }
        }

        private struct LoginDataConnect
        {
            public string username;
            public string password;
            public string grant_type;

            public LoginDataConnect(string username, string password, string grantType)
            {
                this.username = username;
                this.password = password;
                grant_type = grantType;
            }
        }

        public struct EnvironmentData
        {
            public string id;
        }

        private struct AccessToken
        {
            public string access_token;
            public string refresh_token;
            public string expires_in;
        }

        private struct Campaigns
        {
            public List<Campaign> rules;
        }
        
        private struct Campaign
        {
            public string id;
            public string name;
        }

        public struct RemoteConfigData
        {
            public List<Configs> configs;
            public struct Configs
            {
                public string id;
                public string projectId;
                public string environmentId;
                public string type;
                public List<Value> value;
            }
        }
        
        [Serializable]
        public struct Value
        {
            [ValueDropdown("@RemoteConfigTool.SupportedKeys")]
            public string key;
            [ValueDropdown("@RemoteConfigTool.SupportedTypes")]
            public string type;
            public string value;

            public override string ToString()
            {
                return $"{key}[{type}]: {value}";
            }
        }
    }
}