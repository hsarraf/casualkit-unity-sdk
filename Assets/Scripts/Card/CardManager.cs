using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    static CardManager _instance;
    public static CardManager Instance => _instance;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

}
