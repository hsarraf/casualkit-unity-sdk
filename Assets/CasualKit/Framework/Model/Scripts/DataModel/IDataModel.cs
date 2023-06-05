using System;
using CasualKit.Model.Auth;
using CasualKit.Model.Persistent;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;


namespace CasualKit.Model
{

    public interface IDataModel
    {
        public PersistentModel PersistentData { get; set; }
        public PlayerModel PlayerData { get; set; }
    }

}