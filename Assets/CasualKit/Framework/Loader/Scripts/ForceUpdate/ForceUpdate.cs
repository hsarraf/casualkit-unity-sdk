using CasualKit.Api;
using CasualKit.Factory;
using CasualKit.Model;
using System;


namespace CasualKit.Loader.ForceUpadate
{
    public class ForceUpdate : IForceUpdate
    {
        [Inject] IWebRequest<string> _WebRequest;
        public ForceUpdate() => CKFactory.Inject(this);

        public event Action<int> OnNeedForceUpdate;
        public event Action<WebFailResponse> OnForceUpdateFail;

        public void TryCheckLatestVersion(Action<int> onNeedForceupdate = null, Action<WebFailResponse> onForceUpdateFail = null)
        {
            _WebRequest.GET(null,
                CKSettings.Loader.CheckLatestVerionUrl,
                (response) =>
                {
                    int res = string.Compare(response.payload, CKSettings.Loader.CurrentVersion); // 1: needs update, -1, 0: no need to update //
                    onNeedForceupdate?.Invoke(res);
                    OnNeedForceUpdate?.Invoke(res);
                },
                (err) =>
                {
                    onForceUpdateFail?.Invoke(err);
                    OnForceUpdateFail?.Invoke(err);
                });
        }
    }

}