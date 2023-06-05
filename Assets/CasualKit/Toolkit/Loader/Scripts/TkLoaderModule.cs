using UnityEngine;
using UnityEngine.SceneManagement;
using CasualKit.Api;
using CasualKit.Api.Auth;
using CasualKit.Factory;
using CasualKit.Loader;
using Casualkit.Toolkit.Loader;


namespace CasualKit.Toolkit.Loader
{

    public class TkLoaderModule : LoaderBehaviour
    {
        [Inject] IAuth _ApiAuth;

        public string _mainScene;

        public TkLoadingBar _loadingBar;
        public TkLoadingIcon _loadingIcon;

        protected override void Start()
        {
            base.Start();
            _ForceUpdate.TryCheckLatestVersion();
        }

        public override void OnNeedForceUpdate(int res)
        {
            Debug.Log("OnNeedForceUpdate: " + res);
            if (res == 1)
            {
                /* Show Force Update Popup */
            }
            else
            {
                _loadingBar.Show("HELLO", 0.2f, _SceneLoader.OnLoadingInProggress);
                _AssetLoader.TryLoadRemoteAssets();
            }
        }

        public override void OnForceUpdateFail(WebFailResponse err)
        {
            Debug.Log("OnForceUpdateFail: " + err.error);
            /* Show Connection Error try again Popup */
        }

        public override void OnAssetsLoadedSuccess()
        {
            Debug.Log("OnAssetsLoadedSuccess");
            if (!_ApiAuth.IsLoggedIn)
            {
                //_registerPanel.Active = true;
                TkRegisterPanel registerPanel = Instantiate(Resources.Load<TkRegisterPanel>("TkRegisterPanel"));
                registerPanel._onRegisterDone = () => _SceneLoader.LoadAsync(_mainScene);
            }
            else
            {
                Debug.Log("logging in..");
                _ApiAuth.Login((playerData) =>
                {
                    /* Load Main Scene */
                    _SceneLoader.LoadAsync(_mainScene);
                }, 
                (error) =>
                {
                    /* Show Connection Error try again Popup */
                });
            }
        }

        private void LateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _ApiAuth.Register("hsarraf3", "avatar3");
            }
        }

        public override void OnAssetsLoadingInProgress(float percent)
        {
            Debug.Log("OnAssetsLoadingInProgress: " + percent);
        }

        public override void OnAssetsLoadFail(string[] assetsNotLoaded)
        {
            Debug.Log("OnAssetsLoadFail: " + assetsNotLoaded.Length);
        }


        public override void OnSceneLoadingStarted(string sceneName, LoadSceneMode loadMode)
        {
            Debug.Log("OnLoadingStarted: " + sceneName + ", " + loadMode.ToString());
        }

        public override void OnSceneLoadingDone(string sceneName, LoadSceneMode loadMode)
        {
            Debug.Log("OnLoadingDone: " + sceneName + ", " + loadMode.ToString());
            //_loadingBar.Hide();
        }

        public override void OnSceneLoadingInProggress(float proggress)
        {
            Debug.Log("OnLoadingInProggress: " + proggress);
        }
    }

}