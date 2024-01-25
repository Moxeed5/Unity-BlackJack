using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Game Buttons
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;
    
    

    private int standClicks = 0;

    //Access the player and dealer's script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    //public text to acccess and update the hud
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI dealerScoreText;
    public TextMeshProUGUI betsText;
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI standBtnText;


    //Card hiding dealers 2nd Card
    public GameObject hideCard;
    int pot = 0;
       


    void Start()
    {
        //adding on click listeners to buttons
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        
    }

    private void DealClicked()
    {
        //hides dealers card at the start of the deal
        //commenting out the below fixed deal button not working
        dealerScoreText.gameObject.SetActive(false);
        //hideCard.SetActive(true);
        GameObject.Find("Deck").GetComponent<Deck>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        //insert the sum of player's hand and dealer's hand into 
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + playerScript.handValue.ToString();
        //adjust buttons visibility 
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";
        pot = 40;
        betsText.text = pot.ToString();
        //playerSCript.AdjustMoney(-20);

    }

    private void HitClicked()
    {
        if(playerScript.GetCard() <= 10)
        {
            playerScript.GetCard();
        }
    }

    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) Debug.Log("end function");
        HitDealer();
        standBtnText.text = "Call";

    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
        }
    }

    //check for winner and loser

    void RoundOver()
    {
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;

        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;

        bool roundOver = true;

        if (!playerBust && dealerBust)
        {
            mainText.text = "All bust: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }

        //check to see who won
        else if (playerBust || !dealerBust && dealerScript.handValue > playerScript.handValue)
        {
            mainText.text = "Dealer wins!";
        }
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "Player wins!";
            playerScript.AdjustMoney(pot);
        }
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        //set ui for next hand. Update all buttons.
        if(roundOver)
        {
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(false);
            mainText.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(false);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = playerScript.GetMoney().ToString();
            standClicks = 0;

        }
    }

}
