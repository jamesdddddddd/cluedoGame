using System.IO.Enumeration;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    

    public enum CardType 
    {
        Character,
        Weapon,
        Room
    }
}
