//using System.Collections.Generic;

//using UnityEngine;


//namespace CasualKit.Logger
//{
//    public class Logger : MonoBehaviour
//    {
//        public static Logger Instance { get; private set; }
//        void Awake()
//        {
//            if (Instance == null)
//            {
//                Instance = this;
//                _logList = new List<LogObject>();
//                RemoveFirstLogLoop();
//                DontDestroyOnLoad(gameObject);
//            }
//            else if (Instance != this)
//            {
//                Destroy(gameObject);
//            }
//        }

//        public float _logStayTime = 4f;

//        public bool _logInfo;
//        public bool _logWarning;
//        public bool _logError;

//        public static List<LogObject> _logList;

//        public Color INFO_COLOR = new Color(0.941f, 0.941f, 0.941f);
//        public Color WARNING_COLOR = new Color(1f, 0.756f, 0.027f);
//        public Color ERROR_COLOR = new Color(1f, 0.325f, 0.29f);

//        public enum LogType
//        {
//            info, warn, err
//        }

//        public class LogObject
//        {
//            public string _module;
//            public LogType _logType;
//            public string _logMsg;
//            public string ToJson() => JsonUtility.ToJson(this);
//        }

//        void OnGUI()
//        {
//            //GUI.skin = _logGuiSkin;
//            var centeredStyle = GUI.skin.GetStyle("Label");
//            centeredStyle.alignment = TextAnchor.UpperCenter;
//            GUILayout.BeginVertical();
//            foreach (LogObject log in _logList)
//            {
//                if (_logInfo && log._logType == LogType.info)
//                    GUILayout.Label(string.Format("{0}: {1}", log._module, log._logMsg), new GUIStyle(GUI.skin.label)
//                    {
//                        fontSize = (int)(Screen.width * 0.025f),
//                        wordWrap = true,
//                        normal = new GUIStyleState { textColor = INFO_COLOR }
//                    });
//                else if (_logWarning && log._logType == LogType.warn)
//                    GUILayout.Label(string.Format("{0}: {1}", log._module, log._logMsg), new GUIStyle(GUI.skin.label)
//                    {
//                        fontSize = (int)(Screen.width * 0.025f),
//                        wordWrap = true,
//                        normal = new GUIStyleState { textColor = WARNING_COLOR }
//                    });
//                else if (_logError && log._logType == LogType.err)
//                    GUILayout.Label(string.Format("{0}: {1}", log._module, log._logMsg), new GUIStyle(GUI.skin.label)
//                    {
//                        fontSize = (int)(Screen.width * 0.025f),
//                        wordWrap = true,
//                        normal = new GUIStyleState { textColor = ERROR_COLOR }
//                    });
//            }
//            GUILayout.EndVertical();
//        }


//        public static void LogInfo(string log, string module)
//        {
//            _logList.Add(new LogObject { _logMsg = log, _module = module, _logType = LogType.info });
//        }

//        public static void LogWarning(string log, string module)
//        {
//            _logList.Add(new LogObject { _logMsg = log, _module = module, _logType = LogType.warn });
//        }

//        public static void LogError(string log, string module)
//        {
//            _logList.Add(new LogObject { _logMsg = log, _module = module, _logType = LogType.err });
//        }

//        void RemoveFirstLogLoop()
//        {
//            if (_logList.Count > 0)
//                _logList.RemoveAt(0);
//            Invoke("RemoveFirstLogLoop", _logStayTime);
//        }

//    }

//}
