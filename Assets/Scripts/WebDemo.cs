using CasualKit;
using CasualKit.Api;
using CasualKit.Api.Auth;
using CasualKit.Model.Player;
using UnityEngine;


public class WebDemo : ApiBehaviour
{

    protected override void Start()
    {
        base.Start();
        Debug.Log(CKSettings.Api.ApiKey);
    }

    protected override void OnRegistered(PlayerModel playerModel)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnRegisterFailed(WebFailResponse error)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnLoggedIn(PlayerModel playerModel)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnLoginFailed(WebFailResponse error)
    {
        throw new System.NotImplementedException();
    }

}
