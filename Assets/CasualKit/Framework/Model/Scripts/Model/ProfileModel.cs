using System;
using Newtonsoft.Json;


namespace CasualKit.Model.Profile
{

    [Serializable]
    public class ProfileModel : ModelBase
    {
        public string emailAddress;
        public string displayName;
        public string gender;
        public string avatar;

        public string Dump() => JsonConvert.SerializeObject(this);
        public static ProfileModel Load(string json) => JsonConvert.DeserializeObject<ProfileModel>(json);
    }

}