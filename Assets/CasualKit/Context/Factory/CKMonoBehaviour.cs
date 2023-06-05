using UnityEngine;


namespace CasualKit.Factory
{

    public class CKMonoBehaviour : MonoBehaviour
    {
        protected void Awake() => CKFactory.Inject(this);
        private void OnDestroy() => CKFactory.Withdraw(this);
    }
}