using UnityEngine;
using CasualKit.Api;
using CasualKit.Api.Auth;
using CasualKit.Quick.Client;
using CasualKit.Quick.Dispatcher;
using CasualKit.Quick.Room;
using CasualKit.Quick.Scene;
using CasualKit.Quick.Validator;
using CasualKit.Model;
using CasualKit.Model.Player;
using CasualKit.Model.Profile;
using CasualKit.Model.Leaderboard;
using CasualKit.Model.Payment;
using CasualKit.Model.DailyChallenge;
using CasualKit.Loader.Scenes;
using CasualKit.Loader.Asset;
using CasualKit.Loader.ForceUpadate;
using Casualkit.Toolkit.Api;
using CasualKit.Toolkit.Loader;
using Casualkit.Toolkit.Model;


namespace CasualKit.Factory
{
    [DefaultExecutionOrder(-50)]
    public class CKContext : MonoBehaviour
    {
        static CKContext Instance { get; set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                /// CONTEXT ///
                ////////////////////////////////////////////
//                CKFactory.Bind<CKContext>(BindingType.instance);


                /// FRAMEWORK //////////////////////////////
                ////////////////////////////////////////////

                /// QUICK ///
                /////////////
                CKFactory.Bind<IClient, QuickClient>(BindingType.instance);
                CKFactory.Bind<IValidator, DummyValidator>(BindingType.singletone);
                CKFactory.Bind<IRoom, QuickRoom>(BindingType.singletone);
                CKFactory.Bind<IScene, QuickScene>(BindingType.singletone);
                CKFactory.Bind<IDispatcher, QuickDispatcher>(BindingType.singletone);

                /// API ///
                ///////////
                CKFactory.Bind<IAuth, ApiAuth>(BindingType.singletone);

                CKFactory.Bind<IWebRequest<PlayerModel>, WebRequest<PlayerModel>>(BindingType.instance);
                CKFactory.Bind<IWebRequest<ProfileModel>, WebRequest<ProfileModel>>(BindingType.instance);
                CKFactory.Bind<IWebRequest<LeaderboardModel>, WebRequest<LeaderboardModel>>(BindingType.instance);
                CKFactory.Bind<IWebRequest<PaymentModel>, WebRequest<PaymentModel>>(BindingType.instance);
                CKFactory.Bind<IWebRequest<ScoreModel>, WebRequest<ScoreModel>>(BindingType.instance);
                CKFactory.Bind<IWebRequest<SocialModel>, WebRequest<SocialModel>>(BindingType.instance);
                CKFactory.Bind<IWebRequest<DailyChallengeModel>, WebRequest<DailyChallengeModel>>(BindingType.instance);

                /// MODEL ///
                /////////////
                CKFactory.Bind<IDataModel, DataModel>(BindingType.instance);

                /// LOADER ///
                //////////////
                CKFactory.Bind<ISceneLoader, SceneLoader>(BindingType.singletone);
                CKFactory.Bind<IAssetLoader, AssetLoader>(BindingType.singletone);
                CKFactory.Bind<IForceUpdate, ForceUpdate>(BindingType.singletone);

                CKFactory.Bind<IWebRequest<string>, WebRequest<string>>(BindingType.instance);
                CKFactory.Bind<IWebRequest<Texture2D>, WebRequest<Texture2D>>(BindingType.instance);
                CKFactory.Bind<IWebRequest<AssetBundle>, WebRequest<AssetBundle>>(BindingType.instance);

                /// TOOLKIT ////////////////////////////////
                ////////////////////////////////////////////

                /// MODULES ///
                ///////////
                CKFactory.Bind<TkApiModule>(BindingType.instance);
                CKFactory.Bind<TkModelModule>(BindingType.instance);
                CKFactory.Bind<TkLoaderModule>(BindingType.instance);

                // TEST
                //
                CKFactory.Bind<InputHandler>(BindingType.instance);


                ///
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

}