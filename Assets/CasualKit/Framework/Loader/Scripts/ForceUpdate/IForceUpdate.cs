using CasualKit.Api;
using System;
using UnityEngine;


namespace CasualKit.Loader.ForceUpadate
{

    public interface IForceUpdate
    {
        event Action<int> OnNeedForceUpdate;
        event Action<WebFailResponse> OnForceUpdateFail;

        void TryCheckLatestVersion(Action<int> onNeedForceupdate = null, Action<WebFailResponse> onForceUpdateFail = null);
    }

}