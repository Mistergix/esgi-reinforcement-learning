using System.Threading.Tasks;
using PGSauce.Core.Utilities;
using PGSauce.Unity;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace PGSauce.Services
{
    public class PGServices : MonoSingleton<PGServices>
    {
        public async Task InitServices()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
        }
    }
}