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
        betBtn.onClick.AddListener(() => BetClicked());

        hitBtn.gameObject.SetActive(false);
        standBtn.gameObject.SetActive(false);

    }

    private void DealClicked()
    {
        //Reset round 
        playerScript.ResetHand();
        dealerScript.ResetHand();
        //hides dealers card at the start of the deal
        mainText.gameObject.SetActive(false);
        //commenting out the below fixed deal button not working
        dealerScoreText.gameObject.SetActive(false);
        //hideCard.SetActive(true);
        GameObject.Find("Deck").GetComponent<Deck>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        //insert the sum of player's hand and dealer's hand into 
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        //Hides dealer card when active
        hideCard.GetComponent<Renderer>().enabled = true;
        //adjust buttons visibility 
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";
        pot = 40;
        betsText.text = pot.ToString();
        playerScript.AdjustMoney(-20);
        cashText.text = playerScript.GetMoney().ToString();

    }

    private void HitClicked()
    {
        if(playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue.ToString();
            if (playerScript.handValue > 20) RoundOver();
        }
    }

    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standBtnText.text = "Call";

    }

    private void HitDealer()
    {
        dealerScoreText.gameObject.SetActive(true);
        hideCard.GetComponent<Renderer>().enabled = false;

        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)
        {
            dealerScript.GetCard();
            
            //dealerscore
            dealerScoreText.text = dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();

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
            dealBtn.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = playerScript.GetMoney().ToString();
            standClicks = 0;

        }
    }

    /*void BetClicked()
    {
        //change to TextMeshProGUI if causing issies
        TextMeshProUGUI newBet = betBtn.GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        int intBet = int.Parse(newBet.text.ToString().Remove(0,1));
        playerScript.AdjustMoney(-intBet);
        cashText.text += playerScript.GetMoney().ToString();
        pot += (intBet * 2);
        betsText.text = pot.ToString();
    }*/
    void BetClicked()
    {
        int intBet = 20; // Directly use the bet value since it's always $20
        playerScript.AdjustMoney(-intBet);
        cashText.text = playerScript.GetMoney().ToString();
        pot += (intBet * 2);
        betsText.text = pot.ToString();
    }

}
