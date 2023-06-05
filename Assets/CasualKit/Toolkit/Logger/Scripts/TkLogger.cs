using UnityEngine;
using System.Collections;
using System.IO;
using System;


namespace CasualKit.Toolkit.Logger
{

    public class LoggerHandler : ILogHandler
    {
        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            Debug.unityLogger.logHandler.LogFormat(logType, context, format, args);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            Debug.unityLogger.LogException(exception, context);
        }
    }

    public class Logger : MonoBehaviour
    {
        private UnityEngine.Logger _logger;

        void Start()
        {
            _logger = new UnityEngine.Logger(new LoggerHandler());
            LogCK("Hello");
        }

        public void LogCK(string msg)
        {
            _logger.Log("Ck", msg);
        }

        public void LogTK(string msg)
        {
            _logger.Log("Tk", msg);
        }
    }

}