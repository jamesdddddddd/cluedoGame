using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card",menuName = "Card")]
public class Card : ScriptableObject 
{
    public string cardName;
    public CardType cardType;
    
    public enum CardType 
    {
        Weapon,
        character,
        Room
    }
}
