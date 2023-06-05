using System;
using CasualKit.Factory;
using CasualKit.Quick.Client;
using CasualKit.Quick.Dispatcher;
using CasualKit.Quick.View;
using UnityEngine;


namespace CasualKit.Quick.Scene
{

    public interface IScene
    {
        // VIEW MAP FUNCTORS //
        //////////////////////////
        void AddQuickViewToOwnerMap(QuickView nodeView);
        void RemoveQuickViewFromOwnerMap(QuickView nodeView);

        QuickView GetOwnerViewByIndex(int index);
        QuickView GetOppViewByIndex(int index);

        void CallRecievedOwnerView(int index, object data);
        void CallRecievedOppView(int index, object data);

        QuickView[] OwnerViewsList { get; }
        QuickView[] OppViewsList { get; }

        int[] OwnerVidsList { get; }
        int[] OppVidsList { get; }


        // INSTANTIATIONS CALLS //
        //////////////////////////
        QuickView Instantiate(string resPath, Vector3 position, Quaternion rotation);

        // DESTROY CALLS //
        //////////////////////////
        void Destroy(QuickView nodeView);
        void DestroyAll();
        void DisableAll();
        void OnDestroyOppObjects(OppLeftRoomObject oppLeftRoomObj);
        void DestroyAllOpps();
        void CleanUp(bool destroy = false);

        // CALLBACKS //
        //////////////////////////
        event Action<InstantiateObject> OnOppInstantiated;
        event Action<SceneObject> OnOppDestroyed;


    }

}