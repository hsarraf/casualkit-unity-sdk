using CasualKit.Model;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;
using UnityEngine;


namespace Casualkit.Toolkit.Model
{

    public class TkModelModule : ModelBehaviour
    {
        protected override void OnPlayerUpdated(PlayerModel playerData)
        {
            Debug.Log("Model: OnPlayerUpdated, " + playerData.Dump());
        }

        protected override void OnProfileModelUpdated(ProfileModel playerData)
        {
            Debug.Log("Model: OnProfileModelUpdated, " + playerData.Dump());
        }
    }

}