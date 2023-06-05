using UnityEngine;


namespace CasualKit.Api.Settings
{


    [CreateAssetMenu(fileName = "AuthSettings")]
    public class AuthSettings : ScriptableObject
    {
        // URLS
        //
        [Header("[REQUEST URLS]")]
        public string _RegisterUrl = "player/register/";
        public string _LoginUrl = "player/login/";

        // REGEX
        //
        [Header("[REGEX]")]
        public string _RegisterUsernameRegex = @"^(?=[a-zA-Z0-9._]{4,15}$)(?!.*[_.]{2})[^_.].*[^_.]$";
        public string _RegisterPasswordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";

        // LOGS
        //
        [Header("[LOG MESSAGES]")]
        public string _RegisterDoneLog = "You have been successfully registered";
        public string _UsernameAlreadyExistsLog = "The username you have eneterd has already been used";
        public string _EmailAlreadyExistsLog = "The email you have eneterd has already been used";
        public string _InvalidUsernameLog = "The username you have eneterd is not valid";
        public string _InvalidEmailLog = "The email address you have entered is not in correct format";
        public string _IncorrectPasswordLog = "The password you have entered is not correct";
        public string _InvalidConfirmPasswordLog = "The password and confirm password are not matched";
        public string _GenderNotSpecifiedLog = "The gender is not specified";
        public string _AvatarNotSelectedLog = "The avatart is not selected";
        public string _LoginDoneLog = "You have beed successfully logged in";
        public string _LoginFailedLog = "The username or password is not correct";


        // TOOLTIPS
        //
        [Header("[TOOL TIPS]")]
        public string _RegisterUsernameTooltip = "- Minimum 4, maximum 15 characters\n" +
                                                       "- Only letters, numbers, underline, dot\n" +
                                                       "- No underline or dot at the end or beginning";
        public string _RegisterPasswordTooltip = "- Minimum 8 characters\n" +
                                                       "- At least one letter, one number, one special character [@$!%*#?&]";
    }

}