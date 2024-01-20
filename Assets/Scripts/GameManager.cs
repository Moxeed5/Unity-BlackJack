using System;
using System.Collections;
using System.Collections.Generic;
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
    //public Text mainText;
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
        TextMeshProUGUI buttonText = standBtn.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = "Stand";
        pot = 40;
        betsText.text = pot.ToString();

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

}
