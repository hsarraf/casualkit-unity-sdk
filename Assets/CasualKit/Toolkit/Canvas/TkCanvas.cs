using UnityEngine;
using UnityEngine.UI;


namespace Casualkit.Toolkit.UI
{

    public class TkCanvasBase : MonoBehaviour
    {
        public bool Active
        {
            get => _canvas.enabled;
            set => _canvas.enabled = value;
        }

        Canvas _canvas;
        CanvasScaler _canvasScaler;

        public float _screenWidth;

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasScaler = GetComponent<CanvasScaler>();
            _canvasScaler.scaleFactor = _canvasScaler.scaleFactor * Screen.width / _screenWidth;
        }
    }

}