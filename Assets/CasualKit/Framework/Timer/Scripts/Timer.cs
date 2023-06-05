using System;
using System.Collections;
using UnityEngine;


namespace CasualKit.Timeer
{
    public class Timer : MonoBehaviour
    {
        Coroutine _slowMotionCoroutine = null;
        Coroutine _normalMotionCoroutine = null;
        //float _gameSpeed = 1f;
        //public float GameSpeed { get => _gameSpeed; }

        public float _defaultSpeed;
        public float _fixedDeltaTime;
        public AnimationCurve _slowMotionCurve;
        public AnimationCurve _normalMotionCurve;

        const float DEFAULT_DELTATIME = 1f / 60f;

        public void SlowMo(float toSpeed, float rate, Action onDone = null)
        {
            if (_slowMotionCoroutine == null)
                _slowMotionCoroutine = StartCoroutine(SlowMoCo(toSpeed, rate, onDone));
        }

        IEnumerator SlowMoCo(float toSpeed, float rate, Action onDone)
        {
            for (float p = 0f; p <= 1f; p += DEFAULT_DELTATIME * rate)
            {
                //Time.timeScale = _slowMotionCurve.Evaluate(p);// + 0.1f;// * toSpeed;
                Time.timeScale = Mathf.Lerp(_defaultSpeed, toSpeed, p);
                Time.fixedDeltaTime = Time.timeScale * _fixedDeltaTime;
                yield return null;
            }
            _slowMotionCoroutine = null;
            onDone?.Invoke();
        }

        public void NormalMo(float rate, Action onDone = null)
        {
            if (_normalMotionCoroutine == null)
                _normalMotionCoroutine = StartCoroutine(NormalMoCo(rate, onDone));
        }

        IEnumerator NormalMoCo(float rate, Action onDone)
        {
            float currentSpeed = Time.timeScale;
            for (float p = 0f; p <= 1f; p += DEFAULT_DELTATIME * rate)
            {
                //Time.timeScale = _normalMotionCurve.Evaluate(p);// * toSpeed;
                Time.timeScale = Mathf.Lerp(currentSpeed, _defaultSpeed, p);
                Time.fixedDeltaTime = Time.timeScale * _fixedDeltaTime;
                yield return null;
            }
            _normalMotionCoroutine = null;
            onDone?.Invoke();
        }
    }

}