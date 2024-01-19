using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerScript : MonoBehaviour

    //Script is for both player and dealer


{   
    //getting other scripts
    public CardScript cardScript;
    public Deck deckScript;

    public int handValue = 0;

    private int money = 1000;

    //Array for cards on the table
    public GameObject[] hand;

    public int cardIndex = 0;
    //public int aceCount = 0;

    //tracking aces
    List<CardScript> aceList = new List<CardScript>();


    public void StartHand()
    {
        GetCard();
        GetCard();
    }

    // add cards to dealer and player's hand

    public int GetCard()

        //get card and use dealCard to assign sprite and value to the card on the table
    {
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());
        //show card on game screen

        hand[cardIndex].GetComponent<Renderer>().enabled = true;

        handValue += cardValue;

        if (cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }

        //AceCheck();
        cardIndex++;
        return handValue;
    }
}
