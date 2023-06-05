using UnityEngine;
using TMPro;
using CasualKit.Quick;
using CasualKit.Model;
using CasualKit.Api.Auth;
using CasualKit.Quick.Connection;
using CasualKit.Quick.View;
using CasualKit.Quick.Client;
using CasualKit.Quick.Scene;


public class QuickDemo : QuickBehaviour
{
    public TMP_InputField _username;
    public TMP_InputField _roomname;
    public TMP_InputField _sendData;
    public TMP_InputField _resPath;

    public Canvas _canvas;

    protected override void Start()
    {
        //ModelBehaviour.Instance.ModelHandler.PersistentData.Clear();
        _canvas.scaleFactor = Screen.width / 1080f * 0.9f;
        base.Start();
        _username.text = "hsarraf1";
        _roomname.text = "demoRoom";
        _sendData.text = new BroadcastObject(new { a = 1, b = 2 }, 111).Dump();
        _resPath.text = "Ball";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnAuth()
    {
        string userId = _Model.PersistentData.UserID;
        if (string.IsNullOrEmpty(userId))
            ApiBehaviour.Instance.Register(_username.text, "Male");
        else
            ApiBehaviour.Instance.Login();
    }

    public void OnConnect()
    {
        QuickConnect();
    }

    public void OnDisconnect()
    {
        QuickDisonnect();
    }

    public void OnCreateRoom()
    {
        QuickCreateRoom(_roomname.text, 2);
    }

    public void OnJoinRoomByName()
    {
    }

    public void OnJoinOrCreateRoom()
    {
        CreateOrJoinRoom(2);
    }

    public void OnLeaveRoom()
    {
        QuickLeaveRoom();
    }

    public void OnSendData()
    {
        //new QuickFactory<QuickClient>().Inject().Send(_sendData.text);
    }


    QuickView _instanceNode;
    public void OnInstantiate()
    {
        _instanceNode = QuickInstantiate(_resPath.text, Random.insideUnitSphere * 5f, Quaternion.Euler(Random.insideUnitSphere * 360));
    }

    public void OnDestroyIt()
    {
        QuickDestroy(_instanceNode);
    }

    public void OnCleanup()
    {
        //new QuickFactory<QuickScene>().Inject().CleanUp(true);
    }

    /// <summary>
    /// /////////////////////////////////////////////
    /// </summary>

    public override void OnCheckedIn(QuickStatusObject status)
    {
        Debug.Log("QUICK -> OnCheckedIn, " + status.status.ToString());
    }

    protected override void OnClientConnected()
    {
        Debug.Log("QUICK -> OnClientConnected!!");
    }

    protected override void OnClientError(string error)
    {
        Debug.Log("QUICK -> OnClientError!!, " + error);
    }

    protected override void OnClientClosed(CloseStatus status)
    {
        Debug.Log("QUICK -> OnClientClosed, " + status.ToString());
    }

    public override void OnRoomCreated(RoomObject room)
    {
        Debug.Log("QUICK -> OnRoomCreated, " + room.roomName);
    }

    public override void OnCreateRoomFailed(string err)
    {
        Debug.LogError("QUICK -> OnCreateRoomFailed, " + err);
    }

    public override void OnJoinedRoom(RoomObject room)
    {
        Debug.Log("QUICK -> OnRoomJoined, " + room.roomName);
    }

    public override void OnJoinRoomFailed(string err)
    {
        Debug.LogError("QUICK -> OnJoinRoomFailed, " + err);
    }

    public override void OnLeftRoom(RoomObject room)
    {
        Debug.Log("QUICK -> OnLeftRoom, " + room.roomName);
    }

    public override void OnOppJoinedRoom(OppRoomObject room)
    {
        Debug.Log("QUICK -> OnOppJoinedRoom, " + room.oppUid);
    }

    public override void OnOppLeftRoom(OppRoomObject room)
    {
        Debug.Log("QUICK -> OnOppLeftRoom, " + room.oppUid);
    }

    public override void OnOppDisconnected(OppDisconnectedObject opp)
    {
        Debug.Log("QUICK -> OnOppDisconnected, " + opp.oppUid);
    }

    public override void OnOppInstantiated(InstantiateObject instantiateObj)
    {
        Debug.Log("QUICK -> OnOppInstantiated, " + instantiateObj.viewId);
    }

    public override void OnOppDestroyed(SceneObject destroyObj)
    {
        Debug.Log("QUICK -> OnOppDestroyed, " + destroyObj.viewId);
    }

    public override void OnRoomIsFull()
    {
        Debug.Log("QUICK -> OnRoomFull");
    }

}
