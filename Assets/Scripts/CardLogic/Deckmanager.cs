using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    private int currentIndex = 0;

    private void Start()
    {
        // load all the assets
        Card[] cards = Resources.LoadAll<Card>("Cards");

        // now add them
        allCards.AddRange(cards);
    }
    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0) return;

        Card nextCard = allCards[currentIndex];
        handManager.addCardToHand(nextCard);
        currentIndex = (currentIndex +1) % allCards.Count;
    }
}
