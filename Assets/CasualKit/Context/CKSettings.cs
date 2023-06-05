using UnityEngine;
using CasualKit.Quick.Settings;
using CasualKit.Loader.Settings;
using CasualKit.Api.Settings;


namespace CasualKit
{

    public static class CKSettings
    {
        // Api
        //
        public static class Api
        {
            public static ApiSettings Settings => (ApiSettings)Resources.Load("ApiSettings");
            public static string ApiKey = Settings._ApiKey;
            public static string ServerAddress = Settings._Local ? Settings._LocalAddress : Settings._ServerAddress;

            public static string NerError = Settings._NetError;
            public static string InternetError = Settings._InternetError;
            public static string HttpError = Settings._HttpError;
            public static string UnhandledError = Settings._UnhandledError;
            public static string Success = Settings._Success;
        }

        // Auth
        //
        public static class Auth
        {
            public static AuthSettings Settings => (AuthSettings)Resources.Load("AuthSettings");
            public static string RegisterUrl => Api.ServerAddress + Settings._RegisterUrl;
            public static string LoginUrl => Api.ServerAddress + Settings._LoginUrl;

            public static string RegisterUsernameRegex = Settings._RegisterUsernameRegex;
            public static string RegisterPasswordRegex = Settings._RegisterPasswordRegex;

            public static string RegisterDoneLog = Settings._RegisterDoneLog;
            public static string UsernameAlreadyExistsLog = Settings._UsernameAlreadyExistsLog;
            public static string EmailAlreadyExistsLog = Settings._EmailAlreadyExistsLog;
            public static string InvalidUsernameLog = Settings._InvalidUsernameLog;
            public static string InvalidEmailLog = Settings._InvalidEmailLog;
            public static string IncorrectPasswordLog = Settings._IncorrectPasswordLog;
            public static string InvalidConfirmPasswordLog = Settings._InvalidConfirmPasswordLog;
            public static string GenderNotSpecifiedLog = Settings._GenderNotSpecifiedLog;
            public static string AvatarNotSelected = Settings._AvatarNotSelectedLog;
            public static string LoginDoneLog = Settings._LoginDoneLog;
            public static string LoginFailedLog = Settings._LoginFailedLog;
            public static string RegisterUsernameTooltip = Settings._RegisterUsernameTooltip;
            public static string RegisterPasswordTooltip = Settings._RegisterPasswordTooltip;
        }

        // Loader
        //
        public static class Loader
        {
            public static LoaderSettings Settings => (LoaderSettings)Resources.Load("LoaderSettings");
            public static string CurrentVersion => Settings._CurrentVerion;
            public static string CheckLatestVerionUrl => Api.ServerAddress + Settings._CheckLatestVerionUrl;
            public static RemoteAssetList RemoteAssetList => Settings._RemoteAssetList;
        }


        // Quick
        //
        public static class Quick
        {
            public static QuickSettings Settings => (QuickSettings)Resources.Load("QuickSettings");
            public static QuickSettings.Protocol Protocol => Settings._Protocol;
            public static string ServerAddress => Settings._UseLocal ? Settings._LocalAddress : Settings._ServerAddress;
            public static string IssueTicketUrl => Settings._ServerAddress + Settings._IssueTicketUrl;
            public static int RecieveBufferLength => Settings._RecieveBufferLength;
            public static float ConnectLoopRepeatDuration => Settings._ConnectLoopRepeatDuration;
        }

    }

}