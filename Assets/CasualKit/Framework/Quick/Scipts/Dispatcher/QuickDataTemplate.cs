using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
using CasualKit.Quick.Dispatcher;
using CasualKit.Quick.View;



namespace CasualKit.Quick
{

    public class GenericData<T>
    {
        public string Dump() => JsonConvert.SerializeObject(this);
        public static T Load(object data) => ((JObject)data).ToObject<T>();
    }

    [Serializable]
    public class BroadcastObject
    {
        public int v;
        public object d;

        public BroadcastObject(object dat, int vid)
        {
            this.v = vid;
            this.d = dat;
        }
        public string Dump() => QuickDispatcher.OPT_BROADCAST + JsonConvert.SerializeObject(this);
        public static BroadcastObject Load(string data) => JsonConvert.DeserializeObject<BroadcastObject>(data);
    }

    [Serializable]
    public abstract class QuickStatusObject
    {
        public string status;
        public abstract string Dump();
        public static T Load<T>(string data) => (T)JsonConvert.DeserializeObject(data, typeof(T));
    }

    [Serializable]
    public class PingObject : QuickStatusObject
    {
        public int time;
        public override string Dump() => QuickDispatcher.OPT_BROADCAST + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class ValidationObject : QuickStatusObject  /* Just for TCP */
    {
        public string ticket;
        public string username;
        public string roomName;
        public override string Dump() => QuickDispatcher.OPT_VALIDATION + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public abstract class RoomObject : QuickStatusObject
    {
        public string roomName;
        public int roomCap;
        public abstract override string Dump();
    }

    [Serializable]
    public class CreateRoomObject : RoomObject
    {
        public override string Dump() => QuickDispatcher.OPT_CREATE_ROOM + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class JoinRoomByNameObject : RoomObject
    {
        public override string Dump() => QuickDispatcher.OPT_JOIN_ROOM_BY_NAME + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class RejoinRoomObject : RoomObject
    {
        public override string Dump() => QuickDispatcher.OPT_CHECKED_IN + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class CreateOrJoinRoomObject : RoomObject
    {
        public override string Dump() => QuickDispatcher.OPT_CREATE_OR_JOIN_ROOM + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class LeaveRoomObject : RoomObject
    {
        public int[] vidList;
        public override string Dump() => QuickDispatcher.OPT_LEAVE_ROOM + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public abstract class OppRoomObject : QuickStatusObject
    {
        public string oppUid;
        public abstract override string Dump();
    }

    [Serializable]
    public class OppJoinedRoomObject : OppRoomObject
    {
        public override string Dump() => QuickDispatcher.OPT_OPP_JOINED_ROOM + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class OppLeftRoomObject : OppRoomObject
    {
        public int[] vidList;
        public override string Dump() => QuickDispatcher.OPT_OPP_LEFT_ROOM + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class OppDisconnectedObject : QuickStatusObject
    {
        public string oppUid;
        public override string Dump() => null;
    }

    [Serializable]
    public abstract class SceneObject : QuickStatusObject
    {
        public int viewId;
        public string resPath;
        public abstract override string Dump();
        public static float[] Vector3ToFloatArray(Vector3 vec) => new float[3] { vec.x, vec.y, vec.z };
        public static Vector3 FloatArrayToVector3(float[] arr) => new Vector3(arr[0], arr[1], arr[2]);
    }

    [Serializable]
    public class InstantiateObject : SceneObject
    {
        public float[] pos;
        public float[] rot;
        public override string Dump() => QuickDispatcher.OPT_INSTANTIATE + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class DestroyObjObject : SceneObject
    {
        public override string Dump() => QuickDispatcher.OPT_DESTROY + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class SyncObject : QuickStatusObject
    {
        public InstantiateObject[] instances; // to instantiate <vid, resPath>
        public SyncObject() { }
        public SyncObject(QuickView[] viewList)
        {
            instances = new InstantiateObject[viewList.Length];
            int i = 0;
            if (viewList != null)
                foreach (QuickView nodeView in viewList)
                    instances[i++] = new InstantiateObject
                    {
                        viewId = nodeView.ViewId,
                        resPath = nodeView.ResourcePath,
                        pos = SceneObject.Vector3ToFloatArray(nodeView.InitialPos),
                        rot = SceneObject.Vector3ToFloatArray(nodeView.InitialRot.eulerAngles)
                    };
        }
        public override string Dump() => QuickDispatcher.OPT_SYNC + JsonConvert.SerializeObject(this);
    }

    [Serializable]
    public class KillSocketObject : QuickStatusObject
    {
        public override string Dump() => QuickDispatcher.OPT_KILL_SOCKET + JsonConvert.SerializeObject(this);
    }

}