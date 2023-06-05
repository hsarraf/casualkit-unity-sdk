using CasualKit.Factory;
using UnityEngine;


public class DataModule : CKMonoBehaviour
{
    [Inject]
    public InputHandler _InputHandler;

    //private void Awake()
    //{
    //    CKFactory.Inject(this);
    //}

    //private void OnDestroy()
    //{
    //    CKFactory.Withdraw(this);
    //}

}
