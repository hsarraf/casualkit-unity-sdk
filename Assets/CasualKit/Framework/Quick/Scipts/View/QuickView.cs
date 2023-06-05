using UnityEngine;
using CasualKit.Quick.Dispatcher;
using CasualKit.Quick.Scene;
using CasualKit.Factory;

namespace CasualKit.Quick.View
{

    public abstract class QuickView : MonoBehaviour
    {
        [Inject] public IScene _sceneView;
        [Inject] public IDispatcher _dispatcher;


        public int ViewId { get; set; }

        public string ResourcePath { get; set; }

        public bool Online { get; set; }

        public bool Owner { get; set; }

        public object Data { get; set; }

        public Vector3 InitialPos { get; set; }

        public Quaternion InitialRot { get; set; }
        
        private void Awake()
        {
            CKFactory.Inject(this);
        }

        private void OnDestroy()
        {
            _sceneView.RemoveQuickViewFromOwnerMap(this);
        }

        public void Broadcast(object data)
        {
            if (Online)
                _dispatcher.Broadcast(data, ViewId);
        }

        public abstract void OnRecieved(object data);

    }

}