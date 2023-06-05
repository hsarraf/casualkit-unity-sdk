using System;
using Newtonsoft.Json;


namespace CasualKit.Model.Payment
{

    [Serializable]
    public class PaymentModel : ModelBase
    {
        public enum PaymentType
        {
            Money, Ad
        }
        public enum Currency
        {
            Dollar, Lira, Rial
        }
        public enum RewardType
        {
            Coin, Gem, Card
        }
        public string name;
        public string description;
        public int reward;
        public int price;
        public RewardType rewardType;
        public PaymentType paymentType;
        public Currency currency;
        public string Dump() => JsonConvert.SerializeObject(this);
        public static PaymentModel Load(string json) => JsonConvert.DeserializeObject<PaymentModel>(json);
    }

    [Serializable]
    public class ReceiptModel : PaymentModel
    {
    }

}