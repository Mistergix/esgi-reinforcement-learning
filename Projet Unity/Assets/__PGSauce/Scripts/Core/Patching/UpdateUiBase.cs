using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Google.Play.AppUpdate;
using PGSauce.Core.PGDebugging;
using PGSauce.Unity;

namespace PGSauce.Core.Patching
{
    public abstract class UpdateUiBase : PGMonoBehaviour
    {
        #region Public And Serialized Fields
        #endregion
        #region Private Fields
        #endregion
        #region Properties
        #endregion
        #region Unity Functions
        #endregion
        #region Public Methods
        public abstract void LookForUpdate();
        public abstract void Error(AppUpdateErrorCode error);
        public abstract void NoUpdate();
        public abstract void StartUpdating();
        public abstract void Updating(AppUpdateRequest startUpdateRequest);
        public abstract void FinishUpdate();
        public abstract void LoadNextScene();
        #endregion

        #region Private Methods

        protected static string GetErrorMessage(AppUpdateErrorCode error)
        {
            return error switch
            {
                AppUpdateErrorCode.ErrorUnknown => "Une erreur inconnue s'est produite. Veuillez réessayer plus tard.",
                AppUpdateErrorCode.NoError => "Il n'y a pas d'erreur",
                AppUpdateErrorCode.ErrorInternalError =>
                    "Une erreur est survenue sur les serveurs du Play Store. Veuillez réessayer plus tard.",
                AppUpdateErrorCode.ErrorInvalidRequest => GetErrorMessage(AppUpdateErrorCode.ErrorUnknown),
                AppUpdateErrorCode.ErrorUpdateFailed =>
                    "La mise à jour a échoué. Ceci peut-être dû à une connexion internet coupée",
                AppUpdateErrorCode.ErrorUpdateUnavailable =>
                    "La mise à jour a échoué. Peut-être que votre appareil n'est pas compatible ?",
                AppUpdateErrorCode.ErrorUserCanceled => "Vous avez annulé la mise à jour",
                AppUpdateErrorCode.ErrorApiNotAvailable => GetErrorMessage(AppUpdateErrorCode.ErrorUpdateUnavailable),
                AppUpdateErrorCode.ErrorAppNotOwned =>
                    "Vous n'avez pas téléchargé le jeu depuis le PLay STore, la mise à jour n'est pas disponible",
                AppUpdateErrorCode.ErrorDownloadNotPresent => "La mise à jour n'est pas totalement téléchargée",
                AppUpdateErrorCode.ErrorUpdateInProgress => GetErrorMessage(AppUpdateErrorCode.ErrorDownloadNotPresent),
                AppUpdateErrorCode.ErrorUpdateNotAllowed =>
                    "Votre appareil interdit la mise à jour : y'a-t-il assez de batterie et d'espace de stockage ?",
                AppUpdateErrorCode.NoErrorPartiallyAllowed => GetErrorMessage(AppUpdateErrorCode.ErrorUnknown),
                AppUpdateErrorCode.ErrorPlayStoreNotFound =>
                    "Le Play Store n'est pas disponible sur votre appareil, l'avez-vous supprimé ?",
                _ => GetErrorMessage(AppUpdateErrorCode.ErrorUnknown)
            };
        }
        #endregion
    }
}
