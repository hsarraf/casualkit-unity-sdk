using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using CasualKit.Factory;


namespace CasualKit.Loader.Scenes
{

    public class SceneLoader : ISceneLoader
    {
        [Inject] public CKContext _Context;
        public SceneLoader()
        {
            CKFactory.Inject(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public event Action<string, LoadSceneMode> OnLoadingStarted;
        public Action<float> OnLoadingInProggress { get; set; }
        public event Action<string, LoadSceneMode> OnLoadingDone;

        string _currentScene = null;
        string _prevScene = null;


        public void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            _currentScene = scene.name;
            OnLoadingDone?.Invoke(scene.name, loadMode);
        }

        public void Load(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            _prevScene = _currentScene;
            SceneManager.LoadScene(sceneName, loadMode);
        }

        public void LoadPrevScene()
        {
            if (_prevScene != null)
            {
                SceneManager.LoadScene(_prevScene);
                _prevScene = null;
            }
            else
            {
                Debug.LogError("No previous scene!!");
            }
        }

        public void LoadAsync(string sceneName, float extraTimeToLoad = 0f, LoadSceneMode loadMode = LoadSceneMode.Single,
            //Texture background = null, Texture splash = null,
            //string hint = null, string loadingText = null,
            //bool showLoadingBar = true, bool showLoadingIcon = true, bool showLoadingPercent = true,
            Action<string, LoadSceneMode> onLoadingStarted = null,
            Action<float> onLoadingInProggress = null,
            Action<string, LoadSceneMode> onLoadingDone = null
            )
        {
            OnLoadingStarted?.Invoke(sceneName, LoadSceneMode.Single);
            onLoadingStarted?.Invoke(sceneName, LoadSceneMode.Single);
            _prevScene = _currentScene;
            _Context.StartCoroutine(LoadAsyncCo(sceneName, extraTimeToLoad, loadMode, onLoadingInProggress, onLoadingDone));
        }

        IEnumerator LoadAsyncCo(string sceneName, float extraTimeToLoad, LoadSceneMode loadMode = LoadSceneMode.Single,
            Action<float> onLoadingInProggress = null, Action<string, LoadSceneMode> onLoadingDone = null)
        {
            yield return null;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadMode);
            asyncOperation.allowSceneActivation = false;
            OnLoadingInProggress?.Invoke(0f);
            while (!asyncOperation.isDone)
            {
                OnLoadingInProggress?.Invoke(asyncOperation.progress);
                onLoadingInProggress?.Invoke(asyncOperation.progress);
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
            OnLoadingInProggress?.Invoke(1f);

            yield return new WaitForSeconds(extraTimeToLoad);

            onLoadingDone?.Invoke(sceneName, loadMode);
        }
    }

}