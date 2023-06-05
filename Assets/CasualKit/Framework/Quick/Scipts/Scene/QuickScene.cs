using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using CasualKit.Quick.Dispatcher;
using CasualKit.Quick.Scene;
using CasualKit.Quick.Client;
using CasualKit.Quick.View;
using CasualKit.Factory;

namespace CasualKit.Quick.Scene
{

    public class QuickScene : IScene
    {
        [Inject] public IDispatcher _Dispatcher;
        [Inject] public IClient _Client;

        readonly Dictionary<int, QuickView> OwnerViewMap = new Dictionary<int, QuickView>();
        readonly Dictionary<int, QuickView> OppViewMap = new Dictionary<int, QuickView>();

        public event Action<InstantiateObject> OnOppInstantiated;
        public event Action<SceneObject> OnOppDestroyed;

        public QuickView GetOwnerViewByIndex(int index) => OwnerViewMap[index];
        public QuickView GetOppViewByIndex(int index) => OppViewMap[index];

        public QuickView[] OwnerViewsList => OwnerViewMap.Values.ToArray();
        public QuickView[] OppViewsList => OwnerViewMap.Values.ToArray();

        public int[] OwnerVidsList => OwnerViewMap.Keys.ToArray();
        public int[] OppVidsList => OwnerViewMap.Keys.ToArray();

        public void CallRecievedOwnerView(int index, object data) => OwnerViewMap[index].OnRecieved(data);
        public void CallRecievedOppView(int index, object data) => OppViewMap[index].OnRecieved(data);

        public void AddQuickViewToOwnerMap(QuickView nodeView) => OwnerViewMap.Add(nodeView.ViewId, nodeView);
        public void RemoveQuickViewFromOwnerMap(QuickView nodeView) => OwnerViewMap.Remove(nodeView.ViewId);

        public QuickScene()
        {
            CKFactory.Inject(this);
            _Dispatcher.OnOppInstantiate += (instantiateObj) =>
            {
                if (OppViewMap.ContainsKey(instantiateObj.viewId))
                    return;
                string resPath = instantiateObj.resPath;
                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(resPath),
                                             SceneObject.FloatArrayToVector3(instantiateObj.pos),
                                             Quaternion.Euler(SceneObject.FloatArrayToVector3(instantiateObj.rot)));
                QuickView nodeView = obj.GetComponent<QuickView>();
                if (nodeView == null)
                    nodeView = obj.AddComponent<QuickView>();
                nodeView.ViewId = instantiateObj.viewId;
                nodeView.ResourcePath = instantiateObj.resPath;
                nodeView.Owner = false;
                nodeView.Online = true;
                OppViewMap[nodeView.ViewId] = nodeView;
                OnOppInstantiated?.Invoke(instantiateObj);
            };

            _Dispatcher.OnSyncOppInstantiations += (syncObj) =>
            {
                foreach (InstantiateObject instance in syncObj.instances)
                {
                    if (!OppViewMap.ContainsKey(instance.viewId))
                    {
                        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(instance.resPath),
                                                     SceneObject.FloatArrayToVector3(instance.pos),
                                                     Quaternion.Euler(SceneObject.FloatArrayToVector3(instance.rot)));
                        QuickView nodeView = obj.GetComponent<QuickView>();
                        if (nodeView == null)
                            nodeView = obj.AddComponent<QuickView>();
                        nodeView.ViewId = instance.viewId;
                        nodeView.ResourcePath = instance.resPath;
                        nodeView.Owner = false;
                        nodeView.Online = true;
                        OppViewMap[nodeView.ViewId] = nodeView;
                        OnOppInstantiated?.Invoke(instance);
                    }
                }
            };

            _Dispatcher.OnOppDestroy += (destroyObj) => {
                int viewId = destroyObj.viewId;
                if (OppViewMap.ContainsKey(viewId))
                {
                    OppViewMap[viewId].Online = false;
                    GameObject.Destroy(OppViewMap[viewId].gameObject);
                    OppViewMap.Remove(viewId);
                }
                OnOppDestroyed?.Invoke(destroyObj);
            };

            _Client.OnClosed += (state) => CleanUp();

        }

        public QuickView Instantiate(string resPath, Vector3 position, Quaternion rotation)
        {
            if (_Client.Status.State == QuickStatus.StateEnum.inRoom)
            {
                GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(resPath), position, rotation);
                QuickView nodeView = obj.GetComponent<QuickView>();
                if (nodeView == null)
                    nodeView = obj.AddComponent<QuickView>();
                nodeView.ViewId = UnityEngine.Random.Range(0, int.MaxValue);
                nodeView.ResourcePath = resPath;
                nodeView.InitialPos = position;
                nodeView.InitialRot = rotation;
                nodeView.Owner = true;
                nodeView.Online = true;
                OwnerViewMap[nodeView.ViewId] = nodeView;
                _Dispatcher.SendToServer(new InstantiateObject
                {
                    status = "instantiate",
                    viewId = nodeView.ViewId,
                    resPath = resPath,
                    pos = SceneObject.Vector3ToFloatArray(position),
                    rot = SceneObject.Vector3ToFloatArray(rotation.eulerAngles)
                });
                return nodeView;
            }
            Debug.LogError("NODE: Instantiate, client is not in any room!!");
            return null;
        }

        public void Destroy(QuickView nodeView)
        {
            if (_Client.Status.State == QuickStatus.StateEnum.inRoom)
            {
                int viewId = nodeView.ViewId;
                if (OwnerViewMap.ContainsKey(viewId))
                {
                    nodeView.Online = false;
                    _Dispatcher.SendToServer(new DestroyObjObject { viewId = viewId });
                    GameObject.Destroy(nodeView.gameObject);
                    OwnerViewMap.Remove(viewId);
                }
            }
            else
                Debug.LogError("NODE: Destroy, client is not in room");
        }

        public void DestroyAll()
        {
            foreach (int viewId in OwnerViewMap.Keys)
            {
                OwnerViewMap[viewId].Online = false;
                GameObject.Destroy(OwnerViewMap[viewId].gameObject);
            }
            OwnerViewMap.Clear();
        }

        public void DisableAll()
        {
            foreach (int viewId in OwnerViewMap.Keys)
                OwnerViewMap[viewId].Online = false;
        }

        public void OnDestroyOppObjects(OppLeftRoomObject oppLeftRoomObj)
        {
            foreach (int viewId in oppLeftRoomObj.vidList)
            {
                if (OppViewMap.ContainsKey(viewId))
                {
                    OppViewMap[viewId].Online = false;
                    GameObject.Destroy(OppViewMap[viewId].gameObject);
                    OppViewMap.Remove(viewId);
                }
            }
        }

        public void DestroyAllOpps()
        {
            foreach (int viewId in OppViewMap.Keys)
            {
                OppViewMap[viewId].Online = false;
                GameObject.Destroy(OppViewMap[viewId].gameObject);
            }
            OppViewMap.Clear();
        }

        public void CleanUp(bool destroy = false)
        {
            //if (QuickBehaviour.Instance.Client.Status.State == QuickStatus.StateEnum.inRoom)
            //{
            //    Debug.LogError("QUICK: CleanUp, you must first left the room before cleaning up!!");
            //    return;
            //}
            if (destroy)
                DestroyAll();
            else
                DisableAll();
            DestroyAllOpps();
        }

    }

}