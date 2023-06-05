using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using CasualKit;
using CasualKit.Api;
using CasualKit.Factory;
using Casualkit.Toolkit.UI;
using CasualKit.Api.Auth;
using System;

namespace Casualkit.Toolkit.Loader
{

    public class TkRegisterPanel : TkCanvasBase
    {

        [Inject] IAuth _Api;


        [Header("[PANEL]")]
        public Image _panel;

        public RectTransform _avatarContent;
        public string _avatarName;
        Image _prevAvatar;
        Image _avatar;

        public TMP_InputField _usernameInputField;
        public string UsernameInputField
        {
            get => _usernameInputField.text;
            set => _usernameInputField.text = value;
        }

        public Button _enterBttn;
        public TextMeshProUGUI _enterBttnText;
        public string EnterBttnTxt
        {
            get => _enterBttnText.text;
            set => _enterBttnText.text = value;
        }

        public TextMeshProUGUI _logBox;
        public string LogBox
        {
            set
            {
                _logBox.text = value;
                _logBox.gameObject.SetActive(!string.IsNullOrEmpty(value));
            }
        }

        protected override void Awake()
        {
            CKFactory.Inject(this);
            base.Awake();
            _enterBttn.onClick.AddListener(OnRegister);
            foreach (RectTransform avatar in _avatarContent)
            {
                avatar.GetComponent<Button>().onClick.AddListener(() =>
                {
                    OnSelectAvatar(avatar.GetComponent<Image>());
                    avatar.GetComponent<Image>().color = Color.green;
                });
            }
        }

        public Action _onRegisterDone;
        public void OnRegister()
        {
            if (string.IsNullOrEmpty(UsernameInputField) ||
                !Regex.IsMatch(UsernameInputField, CKSettings.Auth.RegisterUsernameRegex))
            {
                LogBox = CKSettings.Auth.InvalidUsernameLog;
                return;
            }
            if (string.IsNullOrEmpty(_avatarName))
            {
                LogBox = CKSettings.Auth.AvatarNotSelected;
                return;
            }
            LogBox = null;
            StartLoading();
            _Api.Register(UsernameInputField, _avatarName,
                (playerData) =>
                {
                    LogBox = CKSettings.Api.Success;
                    StopLoading();
                    _onRegisterDone?.Invoke();
                },
                (status) =>
                {
                    if (status.status == HttpStatus.alreadyExists.ToString())
                    {
                        LogBox = CKSettings.Auth.UsernameAlreadyExistsLog;
                    }
                    else if (status.status == HttpStatus.netError.ToString() || status.status == HttpStatus.serverError.ToString())
                    {
                        LogBox = CKSettings.Api.NerError;
                    }
                    else
                    {
                        LogBox = CKSettings.Api.UnhandledError;
                    }
                    StopLoading();
                });
        }

        public void OnSelectAvatar(Image avatar)
        {
            if (_prevAvatar != null)
                _prevAvatar.color = Color.white;
            _avatar = avatar;
            _avatarName = _avatar.gameObject.name;
            _avatar.color = Color.green;
            _prevAvatar = _avatar;
        }

        public void OnPrivacyPolicy()
        {
            Application.OpenURL("");
        }

        public void OnTermsOfUse()
        {
            Application.OpenURL("");
        }


        Coroutine _loadingCoroutine = null;
        void StartLoading()
        {
            _loadingCoroutine = StartCoroutine(LoadingEnterCo());
            _enterBttn.enabled = false;
        }

        void StopLoading()
        {
            if (_loadingCoroutine != null)
            {
                StopCoroutine(_loadingCoroutine);
                _loadingCoroutine = null;
            }
            EnterBttnTxt = "ENTER";
            _enterBttn.enabled = true;
        }

        IEnumerator LoadingEnterCo()
        {
            while (true)
            {
                EnterBttnTxt = ".";
                yield return new WaitForSeconds(0.15f);
                EnterBttnTxt = "..";
                yield return new WaitForSeconds(0.15f);
                EnterBttnTxt = "...";
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

}