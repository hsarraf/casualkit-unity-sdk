using System;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;

namespace CasualKit.Api
{

    public interface IApiModel<T>
    {
        void Push(Action<WebResponse<T>> onSuccess, Action<WebFailResponse> onFail);
        void Fetch(Action<WebResponse<T>> onSuccess, Action<WebFailResponse> onFail);
    }

}