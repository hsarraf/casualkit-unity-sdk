using CasualKit.Api.Auth;
using CasualKit.Factory;
using CasualKit.Model.Persistent;
using CasualKit.Model.Player;
using UnityEngine;


namespace CasualKit.Model
{

    public class DataModel : MonoBehaviour, IDataModel
    {
        [Inject] public IAuth _ApiAuth;

        public static DataModel Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                CKFactory.Inject(this);
                //PersistentData.ExposeOnStart();
                BindAuthCallbacks();
                Instance = this;
            }
        }

        // PLAYER MODEL
        //
        [SerializeField]
        PlayerModel _playerData; // I have used concerete instead of interface to expose to Editor //
        public PlayerModel PlayerData
        {
            get => _playerData;
            set => _playerData = value;
        }

        // PERSISTENT
        //
        [SerializeField]
        PersistentModel _persistentData; // I have used concerete instead of abstract to expose to Editor //
        public PersistentModel PersistentData
        {
            get => _persistentData;
            set => _persistentData = (PersistentModel)value;
        }

        void BindAuthCallbacks()
        {
            // AUTH CALLBACKS
            /////////////////
            _ApiAuth.OnRegistered += (playerData) =>
            {
                PersistentData.Update(playerData.userId, playerData.username);
                _playerData.Update(playerData);
            };
            _ApiAuth.OnLoggedIn += (playerData) =>
            {
                _playerData.Update(playerData);
            };
        }
    }

}