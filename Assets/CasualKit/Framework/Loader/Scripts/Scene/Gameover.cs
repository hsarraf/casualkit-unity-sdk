using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace CasualKit.Loader
{

    public class Gameover : MonoBehaviour
    {
        public event Action<bool?> OnGameover;

        public void DoGameover(bool? gameoverState, float Triggerdelay = 0f, Action<bool?> callback = null)
        {
            UpdateGameData();
            StartCoroutine(GameoverCo(gameoverState, Triggerdelay, callback));
        }

        IEnumerator GameoverCo(bool? gameoverState, float Triggerdelay, Action<bool?> callback)
        {
            yield return new WaitForSeconds(Triggerdelay);
            OnGameover?.Invoke(gameoverState);
            callback?.Invoke(gameoverState);
        }

        void UpdateGameData()
        {
            if (false/* Check if data manager assigned */)
            {
                /* update game data byy MGDataManager */
            }
        }
    }

}