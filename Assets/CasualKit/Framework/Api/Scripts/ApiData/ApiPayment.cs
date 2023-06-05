using System;
using CasualKit.Factory;
using CasualKit.Model;
using CasualKit.Model.Payment;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;


namespace CasualKit.Api.Payment
{

    public class ApiStore : IApiModel<PaymentModel>
    {
        [Inject] IDataModel _model;
        [Inject] IWebRequest<PaymentModel> _webRequest;
        ApiStore() => CKFactory.Inject(this);

        public void Push(Action<WebResponse<PaymentModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl, onSuccess, onFail);

        public void Fetch(Action<WebResponse<PaymentModel>> onSuccess, Action<WebFailResponse> onFail)
            => _webRequest.POSTJSON(this, CKSettings.Auth.RegisterUrl,
                (sd) => { _model.PlayerData.payment = sd.payload; onSuccess?.Invoke(sd); }, onFail);
    }

}