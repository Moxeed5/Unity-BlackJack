using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;
    
    void Start()
    {
        GetCardValues();
        
    }

    // Update is called once per frame
    void GetCardValues()
    {
        int num = 0;

        for(int i = 0; i < cardSprites.Length; i++)
        {
            num = i;
            num %= 13;
            if(num > 10 || num == 0)
            {
                num = 10;
            }
            cardValues[i] = num++;
        }
        
        
    }

    public void Shuffle()
    {
        for (int i = cardSprites.Length - 1; i > 0; i--)
        {
            // Pick a random index from 0 to i
            int j = Random.Range(0, i + 1);

            // Swap elements at i and j
            Sprite tempSprite = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = tempSprite;

            int tempValue = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = tempValue;
        }
        currentIndex = 1;
    }


    public int DealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex++]);
        return cardScript.GetValueOfCard();

    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}
