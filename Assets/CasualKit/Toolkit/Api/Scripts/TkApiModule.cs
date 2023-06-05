using CasualKit.Api;
using CasualKit.Api.Auth;
using CasualKit.Model.Player;
using UnityEngine;


namespace Casualkit.Toolkit.Api
{

    public class TkApiModule : ApiBehaviour
    {
        protected override void OnRegistered(PlayerModel playerData)
        {
            Debug.Log("API: OnRegistered, " + playerData.Dump());
        }

        protected override void OnRegisterFailed(WebFailResponse error)
        {
            Debug.Log("API: OnRegisterFailed, " + error.status);
        }

        protected override void OnLoggedIn(PlayerModel playerData)
        {
            Debug.Log("API: OnLoggedIn, " + playerData.Dump());
        }

        protected override void OnLoginFailed(WebFailResponse error)
        {
            Debug.Log("API: OnLoginFailed, " + error.status);
        }
    }

}
