using CasualKit.Api;
using CasualKit.Factory;
using CasualKit.Loader.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CasualKit.Loader.Asset
{
    public class AssetLoader : IAssetLoader
    {
        [Inject] IWebRequest<AssetBundle> _WebRequest;
        public AssetLoader() => CKFactory.Inject(this);

        public event Action OnAssetsLoadedSuccess;
        public event Action<float> OnAssetsLoadingInProgress;
        public event Action<string[]> OnAssetsLoadFail;

        public Dictionary<string, AssetItem> RemoteAssetMap { get; set; }
        public bool HasRemoteAssets => RemoteAssetMap.Keys.Count > 0;

        List<Coroutine> _downloadCoroutines = new List<Coroutine>();

        public void CreateRemoteAssetMap()
        {
            RemoteAssetMap = new Dictionary<string, AssetItem>();
            foreach (AssetItem item in CKSettings.Loader.RemoteAssetList._assetList)
                RemoteAssetMap[item._name] = item;
        }

        public void PullRemoteAsset(string url, string name, Hash128 hash, Action<AssetBundle> onSuccess = null, Action<string> onFail = null)
        {
            _WebRequest.DownloadAssetBundle(url, name, hash,
                (bundle) =>
                {
                    onSuccess?.Invoke(bundle.payload);
                },
                (error) =>
                {
                    onFail?.Invoke(error.error);
                });
        }

        public IEnumerator LoadRemoteAsset(AssetBundle bundle, string name, Action<UnityEngine.Object> onSuccess = null, Action onFail = null)
        {
            AssetBundleRequest assReq = bundle.LoadAssetAsync(name);
            yield return new WaitUntil(() => assReq.isDone);
            if (assReq.asset != null)
                onSuccess?.Invoke(assReq.asset);
            else
                onFail?.Invoke();
        }

        public IEnumerator PullLocalAsset(string path, Action<UnityEngine.Object> onSuccess = null, Action onFail = null)
        {
            ResourceRequest resReq = Resources.LoadAsync(path);
            yield return new WaitUntil(() => resReq.isDone);
            if (resReq.asset != null)
                onSuccess?.Invoke(resReq.asset);
            else
                onFail?.Invoke();
        }

        public void TryLoadRemoteAssets(Action onSuccess = null, Action<string[]> onFail = null)
        {
            CreateRemoteAssetMap();
            if (HasRemoteAssets)
            {
                Coroutine co = null;
                foreach (KeyValuePair<string, AssetItem> asset in RemoteAssetMap)
                {
                    co = _WebRequest.DownloadAssetBundle(asset.Value._url, asset.Value._name, asset.Value.assetHash,
                        (bundle) =>
                        {
                            asset.Value._bundle = bundle;
                            _downloadCoroutines.Remove(co);
                            CheckIfAllAssetsLoaded();
                        },
                        (error) =>
                        {
                            Debug.LogError(error.error);
                            _downloadCoroutines.Remove(co);
                            CheckIfAllAssetsLoaded();
                        });
                    _downloadCoroutines.Add(co);
                }
            }
            else 
                OnAssetsLoadedSuccess?.Invoke();
        }

        void CheckIfAllAssetsLoaded()
        {
            List<string> assetsNotLoaded = new List<string>();
            foreach (KeyValuePair<string, AssetItem> asset in RemoteAssetMap)
                if (asset.Value._bundle == null)
                    assetsNotLoaded.Add(asset.Key);
            if (_downloadCoroutines.Count > 0)
                OnAssetsLoadingInProgress?.Invoke(1f - (float)assetsNotLoaded.Count / RemoteAssetMap.Count);
            else if (assetsNotLoaded.Count > 0)
                OnAssetsLoadFail?.Invoke(assetsNotLoaded.ToArray());
            else
                OnAssetsLoadedSuccess?.Invoke();
        }
    }

}