using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CasualKit.Factory
{

    public class CKBehaviour
    {
        private CKBehaviour() => CKFactory.Inject(this);
    }

}