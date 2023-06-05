using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CasualKit.Model.Persistent
{

    public interface IPersistentModel
    {
        string UserID { get; set; }
        string Username { get; set; }

        void Update(string userId, string username);
        void ExposeOnStart();
        string Dump();
        void Clear();
    }

}