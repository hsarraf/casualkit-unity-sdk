using CasualKit.Factory;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;
using UnityEngine;


namespace CasualKit.Model
{

    [RequireComponent(typeof(DataModel))]
    public abstract class ModelBehaviour : MonoBehaviour
    {
        public static ModelBehaviour Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                CKFactory.Inject(this);
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        [Inject] public IDataModel _DataModel;

        protected abstract void OnPlayerUpdated(PlayerModel playerData);
        protected abstract void OnProfileModelUpdated(ProfileModel playerData);

    }

}