using CasualKit.Loader.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CasualKit.Loader.Asset
{

    public interface IAssetLoader
    {
        Dictionary<string, AssetItem> RemoteAssetMap { get; set; }
        bool HasRemoteAssets { get; }

        event Action OnAssetsLoadedSuccess;
        event Action<float> OnAssetsLoadingInProgress;
        event Action<string[]> OnAssetsLoadFail;

        void TryLoadRemoteAssets(Action OnRemoteAssetsLoadedSuccess = null, Action<string[]> OnRemoteAssetsLoadFail = null);
    }

}