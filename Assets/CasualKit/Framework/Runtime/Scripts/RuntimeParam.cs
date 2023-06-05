using System;
using CasualKit.Factory;
using CasualKit.Model;
using UnityEngine;


namespace CasualKit.Runtime
{
    [Serializable]
    public class RuntimeData<T>
    {
        [SerializeField]
        private T _value;
        public T Value
        {
            get => _value; set
            {
                _value = value;
                OnSet?.Invoke(_value);
            }
        }
        public Action<T> OnSet { get; set; }
        public RuntimeData(T initVal, Action<T> onSet = null) => (_value, OnSet) = (initVal, onSet);
    }

    public class RuntimeParam : MonoBehaviour
    {
        [Inject] IDataModel _DataModel;
        private void Awake()
        {
            CKFactory.Inject(this);
        }

        private void Start()
        {
            Initialize();
            Bind();
        }

        public RuntimeData<int> _levelCoin;
        public RuntimeData<int> _totalCoin;

        public RuntimeData<int> _levelGem;
        public RuntimeData<int> _totalGem;

        public RuntimeData<int> _levelScore;
        public RuntimeData<int> _totalScore;

        public RuntimeData<int> _levelXp;
        public RuntimeData<int> _totalXP;

        public RuntimeData<int> _level;


        public void Initialize()
        {
            _totalCoin.Value = _DataModel.PlayerData.score.coin;
            _levelCoin.Value = 0;

            _totalGem.Value = _DataModel.PlayerData.score.gem;
            _levelGem.Value = 0;

            _totalScore.Value = _DataModel.PlayerData.score.score;
            _levelScore.Value = 0;

            _totalXP.Value = _DataModel.PlayerData.score.xp;
            _levelXp.Value = 0;

            _level.Value = _DataModel.PlayerData.score.level;
        }

        public void Bind()
        {
            _totalCoin.OnSet += (v) => _DataModel.PlayerData.score.coin = v;
            _totalGem.OnSet += (v) => _DataModel.PlayerData.score.gem = v;
            _totalScore.OnSet += (v) => _DataModel.PlayerData.score.score = v;
            _totalXP.OnSet += (v) => _DataModel.PlayerData.score.xp = v;
            _level.OnSet += (v) => _DataModel.PlayerData.score.level = v;
        }

    }

}