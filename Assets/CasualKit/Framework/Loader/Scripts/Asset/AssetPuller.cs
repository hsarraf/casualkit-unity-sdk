using CasualKit.Factory;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace CasualKit.Loader.Asset
{
    public enum AssetSource
    {
        Local, Remote
    }

    [AttributeUsage(AttributeTargets.Field)]
    public sealed class Pull : Attribute
    {
        public AssetSource _sourceType;
        public string _path;
        public string _name;
        public Hash128 _hash;
        public Pull(AssetSource sourceType, string path, string name, Hash128 hash)
        {
            (_sourceType, _path, _name, _hash) = (sourceType, path, name, hash);
        }
    }

    public class AssetPuller
    {
        [Inject]
        AssetLoader _assetLoader;

        Dictionary<string, UnityEngine.Object> _assetMap = new Dictionary<string, UnityEngine.Object>();
        public AssetPuller()
        {
            CKFactory.Inject(this);

            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic |
                                        BindingFlags.Static | BindingFlags.Instance;

            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type script in ass.GetTypes())
                {
                    foreach (FieldInfo field in script.GetFields(bindingFlags))
                    {
                        Pull pull = field.GetCustomAttribute<Pull>();
                        if (pull != null)
                        {
                            if (pull._sourceType == AssetSource.Remote)
                            {
                                _assetLoader.PullRemoteAsset(pull._path, pull._name, pull._hash,
                                    (bundle) =>
                                    {
                                        LoaderBehaviour.Instance.StartCoroutine(_assetLoader.LoadRemoteAsset(bundle, pull._name,
                                            (asset) =>
                                            {
                                                Debug.Log("Remote asset loaded: " + pull._name);
                                                _assetMap[script.GetType().ToString() + field.GetType().ToString()] = asset;
                                            },
                                            () =>
                                            {
                                                Debug.LogError("Remote asset loading failed: " + pull._name);
                                            }));
                                    },
                                    (error) =>
                                    {
                                        Debug.LogError("Remote asset pullingg failed: " + pull._name);
                                    });
                            }
                            else if (pull._sourceType == AssetSource.Local)
                            {
                                LoaderBehaviour.Instance.StartCoroutine(_assetLoader.PullLocalAsset(pull._path,
                                    (asset) =>
                                    {
                                        Debug.Log("Local asset loaded: " + pull._name);
                                    },
                                    () =>
                                    {
                                        Debug.LogError("Local asset loading failed: " + pull._name);
                                    }));
                            }
                        }
                    }
                }
            }
        }

        public void Pull(object sender)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.Static | BindingFlags.Instance;
            foreach (FieldInfo field in sender.GetType().GetFields(bindingFlags))
            {
                string key = sender.GetType().ToString() + field.GetType().ToString();
                if (field.GetCustomAttribute<Pull>() != null)
                {
                    if (_assetMap.ContainsKey(key))
                    {
                        UnityEngine.Object asset = _assetMap[key];
                    }
                    else
                    {

                    }
                }
            }
        }
    }

}