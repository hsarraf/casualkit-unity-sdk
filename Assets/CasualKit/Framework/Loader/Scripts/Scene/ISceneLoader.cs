using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace CasualKit.Loader.Scenes
{

    public interface ISceneLoader
    {
        event Action<string, LoadSceneMode> OnLoadingStarted;
        Action<float> OnLoadingInProggress { get; set; }
        event Action<string, LoadSceneMode> OnLoadingDone;

        void Load(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single);
        void LoadPrevScene();
        void LoadAsync(string sceneName, float extraTimeToLoad = 0f, LoadSceneMode loadMode = LoadSceneMode.Single,
            Action<string, LoadSceneMode> onLoadingStarted = null,
            Action<float> onLoadingInProggress = null,
            Action<string, LoadSceneMode> onLoadingDone = null);
    }

}