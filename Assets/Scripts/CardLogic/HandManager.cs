using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;
    public GameObject cardPrefab;
    public Transform handTransform;
    public float fanSpread = 7f;
    public List<GameObject> cardsInHand = new List<GameObject>();
    public float cardSpacing = -200f;
    public float verticalSpacing = 20f;
    public int cardsAmount = 3;


    public List<Card> allCards = new List<Card>();

    private int currentIndex = 0;
    
    void Start()
    {
        // load all the assets
        Card[] cards = Resources.LoadAll<Card>("Cards");

        // now add them
        allCards.AddRange(cards);
        // this will need to chack how many players are playing and calculat how many "add" functions will need to be prefomed.
        // for now i'll do defalt for 6 players (3 cards)
        for (int i = 0; i < cardsAmount; i++) 
        {
            DrawCard();
        }
    }

    void Update()
    {
        //UpdateHandVisuals();
    }

    public void DrawCard()
    {
        if (allCards.Count == 0) return;

        Card nextCard = allCards[currentIndex];
        addCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
    }

    public void addCardToHand(Card cardData)
    {
        // instantiats card 
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        // set the card data of the instentiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        UpdateHandVisuals();
    }

    private void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;
        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float normPosition = (2f * i / (cardCount - 1) - 1f);
            float horizantilOffset = (cardSpacing * (i - (cardCount - 1) / 2f));
            float verticalOffset = verticalSpacing * (1 - normPosition * normPosition);
            

            cardsInHand[i].transform.localPosition = new Vector3(horizantilOffset, verticalOffset, 0f);

        }
    }
}
