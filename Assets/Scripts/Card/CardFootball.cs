using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFootball : Card
{
    public FootballTemplate _cardTemplate;
    private void Start()
    {
        _cardTemplate = ScriptableObject.CreateInstance<FootballTemplate>();
        _cardTemplate._speed = 4;
    }
}
