using Newtonsoft.Json;


namespace CasualKit.Model.Player
{

    public interface IPlayerModel
    {
        void Update(PlayerModel pd);
        //void Push();
        //void Fetch();
        string Dump() => JsonConvert.SerializeObject(this);
    }

}