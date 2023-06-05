using System;
using System.Reflection;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using CasualKit.Factory;
using CasualKit.Api.Auth;

namespace CasualKit.Api
{

    [Serializable]
    public class WebResponse<T>
    {
        public string status;
        public T payload;
    }

    [Serializable]
    public class WebFailResponse
    {
        public string status;
        public string error;
    }

    public enum Method
    {
        GET, POST
    }
    public enum Encode
    {
        FORM, JSON, NONE
    }

    public enum WebError
    {
        NetErr, HttpErr
    }

    public enum HttpStatus
    {
        success,
        badRequest,
        alreadyExists,
        doesNotExist,
        invalidReq,
        invalidAuth,
        serverError,
        netError,
        integrity,
        unhandled
    }

    class WebRequest<T> : IWebRequest<T>
    {
        public WebRequest() => CKFactory.Inject(this);

        public void GET(object wdata, string url, Action<WebResponse<T>> onSuccess = null, Action<WebFailResponse> onFail = null)
        {
            SendWebRequest(wdata, url, Method.GET, Encode.NONE, onSuccess, onFail);
        }

        public void POSTFORM(object wdata, string url, Action<WebResponse<T>> onSuccess = null, Action<WebFailResponse> onFail = null)
        {
            SendWebRequest(wdata, url, Method.POST, Encode.FORM, onSuccess, onFail);
        }

        public void POSTJSON(object wdata, string url, Action<WebResponse<T>> onSuccess = null, Action<WebFailResponse> onFail = null)
        {
            //Debug.Log(wdata);
            SendWebRequest(wdata, url, Method.POST, Encode.JSON, onSuccess, onFail);
        }

        void SendWebRequest(object wdata, string url, Method method, Encode encode, Action<WebResponse<T>> onSuccess = null, Action<WebFailResponse> onFail = null)
        {
            ApiBehaviour.Instance.StartCoroutine(WebRequestCo(wdata, url, method, encode, onSuccess, onFail));
        }

        IEnumerator WebRequestCo(object wdata, string url, Method method, Encode encode, Action<WebResponse<T>> onSuccess, Action<WebFailResponse> onFail)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                onFail?.Invoke(new WebFailResponse { status = "NoInternet" });
                yield break;
            }
            UnityWebRequest request = null;
            if (wdata == null)
            {
                request = UnityWebRequest.Get(url);
                request.downloadHandler = new DownloadHandlerBuffer();
            }
            else if (method == Method.POST)
            {
                if (encode == Encode.JSON)
                {
                    byte[] jsonBinary = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(wdata, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
                    DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
                    UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
                    uploadHandlerRaw.contentType = "application/json";
                    request = new UnityWebRequest(url, method.ToString(), downloadHandlerBuffer, uploadHandlerRaw);
                }
                else if (encode == Encode.FORM)
                {
                    List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
                    foreach (PropertyInfo property in wdata.GetType().GetRuntimeProperties())
                    {
                        formData.Add((IMultipartFormSection)property.GetValue(wdata));
                    }
                    request = UnityWebRequest.Post(url, formData);
                    request.downloadHandler = new DownloadHandlerBuffer();
                }
            }
            else if (method == Method.GET)
            {
                url += "?";
                foreach (PropertyInfo property in wdata.GetType().GetRuntimeProperties())
                {
                    ////Debug.Log(property.Name);
                    url += string.Format("{0}={1}&", property.Name, property.GetValue(wdata));
                }
                url = url.Remove(url.Length - 1);
                request = UnityWebRequest.Get(url);
                request.downloadHandler = new DownloadHandlerBuffer();
            }
            yield return request.SendWebRequest();
            //Debug.Log(request.result);
            //Debug.Log(request.downloadHandler.text);
            if (request.result != UnityWebRequest.Result.ConnectionError)
            {
                if (request.result != UnityWebRequest.Result.ProtocolError)
                {
                    onSuccess?.Invoke(JsonUtility.FromJson<WebResponse<T>>(request.downloadHandler.text));
                }
                else
                {
                    onFail?.Invoke(JsonUtility.FromJson<WebFailResponse>(request.downloadHandler.text));
                }
            }
            else
            {
                onFail?.Invoke(new WebFailResponse { status = HttpStatus.netError.ToString() });
            }
            request.Dispose();
        }


        /// TEXTURE DOWNLAODER ///
        /// 
        public void DownloadTexutre(string url, Action<WebResponse<Texture2D>> onSuccess, Action<WebFailResponse> onFail)
        {
            ApiBehaviour.Instance.StartCoroutine(DownloadTextureCo(url, onSuccess, onFail));
        }

        IEnumerator DownloadTextureCo(string url, Action<WebResponse<Texture2D>> onSuccess, Action<WebFailResponse> onFail)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.ConnectionError)
            {
                if (request.result != UnityWebRequest.Result.ProtocolError)
                {
                    onSuccess(new WebResponse<Texture2D>() { payload = ((DownloadHandlerTexture)request.downloadHandler).texture, status = HttpStatus.success.ToString() });
                }
                else
                {
                    onFail?.Invoke(JsonUtility.FromJson<WebFailResponse>(request.downloadHandler.text));
                }
            }
            else
            {
                onFail?.Invoke(new WebFailResponse { status = HttpStatus.netError.ToString() });
            }
            request.Dispose();
        }


        /// ASSET BUNDLE DOWNLOADER ///
        /// 
        public Coroutine DownloadAssetBundle(string assetUrl, string assetName, Hash128 assetHash, Action<WebResponse<AssetBundle>> onSuccess, Action<WebFailResponse> onFail)
        {
            return ApiBehaviour.Instance.StartCoroutine(DownloadAssetBundleCo(assetUrl, assetName, assetHash, onSuccess, onFail));
        }
        IEnumerator DownloadAssetBundleCo(string assetUrl, string assetName, Hash128 assetHash, Action<WebResponse<AssetBundle>> onSuccess, Action<WebFailResponse> onFail)
        {
            //UnityWebRequest.ClearCookieCache();
            yield return null;
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(assetUrl, new CachedAssetBundle(assetName, assetHash));
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.ConnectionError)
            {
                if (request.result != UnityWebRequest.Result.ProtocolError)
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                    //Mesh[] meshList = bundle.LoadAllAssets<Mesh>();
                    onSuccess?.Invoke(new WebResponse<AssetBundle> { payload = bundle, status = HttpStatus.success.ToString() });
                }
                else
                {
                    onFail?.Invoke(JsonUtility.FromJson<WebFailResponse>(request.downloadHandler.text));
                }
            }
            else
            {
                onFail?.Invoke(new WebFailResponse { status = HttpStatus.netError.ToString() });
            }
            request.Dispose();
        }
    }

}