using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using CasualKit.Factory;
using CasualKit.Loader.Asset;
using CasualKit.Loader.Scenes;
using CasualKit.Loader.ForceUpadate;
using CasualKit.Api;


namespace CasualKit.Loader
{

    public abstract class LoaderBehaviour : MonoBehaviour
    {
        [Inject] public ISceneLoader _SceneLoader;
        [Inject] public IAssetLoader _AssetLoader;
        [Inject] public IForceUpdate _ForceUpdate;

        public static LoaderBehaviour Instance { get; private set; }
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                CKFactory.Inject(this);
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void Start()
        {
            _SceneLoader.OnLoadingStarted += OnSceneLoadingStarted;
            _SceneLoader.OnLoadingInProggress += OnSceneLoadingInProggress;
            _SceneLoader.OnLoadingDone += OnSceneLoadingDone;

            _AssetLoader.OnAssetsLoadedSuccess += OnAssetsLoadedSuccess;
            _AssetLoader.OnAssetsLoadingInProgress += OnAssetsLoadingInProgress;
            _AssetLoader.OnAssetsLoadFail += OnAssetsLoadFail;

            _ForceUpdate.OnNeedForceUpdate += OnNeedForceUpdate;
            _ForceUpdate.OnForceUpdateFail += OnForceUpdateFail;
        }

        public void LoadScene(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single) => _SceneLoader.Load(sceneName, loadMode);

        public void LoadSceneAsync(string sceneName, float extraTimeToLoad = 0f, LoadSceneMode loadMode = LoadSceneMode.Single, 
            Action<string, LoadSceneMode> onLoadingStarted = null,
            Action<float> onLoadingInProggress = null,
            Action<string, LoadSceneMode> onLoadingDone = null) => 
            _SceneLoader.LoadAsync(sceneName, extraTimeToLoad, loadMode, onLoadingStarted, onLoadingInProggress, onLoadingDone);


        public abstract void OnSceneLoadingStarted(string sceneName, LoadSceneMode loadMode);
        public abstract void OnSceneLoadingInProggress(float proggress);
        public abstract void OnSceneLoadingDone(string sceneName, LoadSceneMode loadMode);

        public abstract void OnAssetsLoadedSuccess();
        public abstract void OnAssetsLoadingInProgress(float percent);
        public abstract void OnAssetsLoadFail(string[] assetsNotLoaded);

        public abstract void OnNeedForceUpdate(int res);
        public abstract void OnForceUpdateFail(WebFailResponse err);
    }

}