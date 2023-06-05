using System;
using UnityEngine;


namespace CasualKit.Loader.Settings
{
    [Serializable]
    public class AssetItem
    {
        public string _name;
        public Hash128 assetHash;
        public string _url;
        public object _bundle;
    }

    [Serializable]
    public class RemoteAssetList
    {
        public AssetItem[] _assetList;
    }

    [CreateAssetMenu(fileName = "LoaderSettings", menuName = "CasualKit/LoaderSettings")]
    public class LoaderSettings : ScriptableObject
    {
        [Header("[FORCE UPDATE]")]
        public string _CurrentVerion = "1.0.0";
        public string _CheckLatestVerionUrl = "force_update/get_latest_version/";

        [Header("[ASSET LOADER]")]
        public RemoteAssetList _RemoteAssetList;
    }

}