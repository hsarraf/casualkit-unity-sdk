using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class FootballTemplate : CardTemplate
{
    public enum CardName
    {
        Ronaldo
    }
    public CardName _name;
    public int _speed;
    public int _power;
}

