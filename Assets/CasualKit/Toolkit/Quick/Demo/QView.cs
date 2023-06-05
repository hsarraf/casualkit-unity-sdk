using UnityEngine;
using CasualKit.Quick;
using CasualKit.Quick.View;


public class QView : QuickView
{

    public override void OnRecieved(object data)
    {
        Debug.Log(data.ToString());
    }

}
