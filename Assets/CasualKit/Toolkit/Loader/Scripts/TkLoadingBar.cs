using Casualkit.Toolkit.UI;
using CasualKit.Factory;
using CasualKit.Loader.Scenes;
using System;
using TMPro;
using UnityEngine;


namespace CasualKit.Toolkit.Loader
{

    public class TkLoadingBar : TkCanvasBase
    {
        [Inject] ISceneLoader _SceneLoader;

        public static TkLoadingBar Instance { get; private set; }
        protected override void Awake()
        {
            CKFactory.Inject(this);
            base.Awake();
        }

        public RectTransform _loadingBarGrp;
        public RectTransform _loadingBar;
        float LoadingBar
        {
            get => _loadingBar.localScale.x;
            set => _loadingBar.localScale = new Vector3(value, 1f, 1f);
        }

        public TextMeshProUGUI _loadingTxt;
        string LoadingTxt
        {
            get => _loadingTxt.text;
            set => _loadingTxt.text = value;
        }

        public TextMeshProUGUI _percentTxt;
        float Percent
        {
            set => _percentTxt.text = Mathf.CeilToInt(value * 100f).ToString() + "%";
        }

        void OnUpdateLoadingBar(float proggress)
        {
            LoadingBar = proggress;
            Percent = proggress;
        }

        public void Show(string loadingText, float posFactorY = 0.2f, Action<float> onProgressAction = null)
        {
            Percent = 0f;
            LoadingBar = 0f;
            LoadingTxt = loadingText;
            Debug.Log(Screen.height);
            Debug.Log(posFactorY);
            _loadingBarGrp.anchoredPosition = new Vector3(0f, Screen.height * posFactorY, 0f);
            if (onProgressAction != null)
                onProgressAction = OnUpdateLoadingBar;
            else
                _SceneLoader.OnLoadingInProggress += OnUpdateLoadingBar;
            Active = true;
        }
        public void Hide()
        {
            Active = false;
            Percent = 0f;
            LoadingBar = 0f;
            LoadingTxt = "";
        }
    }

}