using System;
using UnityEngine;


namespace CasualKit.Api
{

    public interface IWebRequest<T>
    {
        void POSTFORM(object wdata, string url, Action<WebResponse<T>> onSuccess = null, Action<WebFailResponse> onFail = null);
        void GET(object wdata, string url, Action<WebResponse<T>> onSuccess = null, Action<WebFailResponse> onFail = null);
        void POSTJSON(object wdata, string url, Action<WebResponse<T>> onSuccess = null, Action<WebFailResponse> onFail = null);

        void DownloadTexutre(string url, Action<WebResponse<Texture2D>> onSuccess, Action<WebFailResponse> onFail);
        Coroutine DownloadAssetBundle(string assetUrl, string assetName, Hash128 assetHash, Action<WebResponse<AssetBundle>> onSuccess, Action<WebFailResponse> onFail);
    }

}