using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DuringTraining : Menu
{

    [SerializeField] private Text currentCardNumberText, maxCardNumberText;

    [SerializeField] private CompleteContext completeContext;

    private void Start()
    {
        Hide();
    }

    public void StartTraining(Desk desk)
    {
        completeContext.Hide();
        questionContext.Hide();
        base.Show();
        currentDesk = desk;

        maxCardNumberText.text = desk.flashCards.Count.ToString();

        StartFirstCard();

    }


    private Desk currentDesk;

    [SerializeField] private QuestionContextMenu questionContext;

    private int currentCardNumber = -1;
    private void StartFirstCard()
    {
        currentDesk.SortCards();
        currentCardNumber = -1;
        NextCard();
    }


    public void NextCard()
    {
        currentCardNumber++;
        currentCardNumberText.text = currentCardNumber.ToString();

        if(currentCardNumber >= currentDesk.flashCards.Count)
        {
            questionContext.Hide();
            completeContext.Show(currentDesk);
            return;
        }

        bool havePosition = false;
        FlashCard selectedCard = null;
        for (int errorCounter = 0; errorCounter < currentDesk.flashCards.Count; errorCounter++)
        {
            foreach (var card in currentDesk.flashCards)
            {
                if (card.position == currentCardNumber)
                {
                    havePosition = true;
                    selectedCard = card;
                }
            }
            if (havePosition)
            {
                questionContext.Show(selectedCard);
                return;
            }
        }
        

    }


    public override void Hide()
    {
        completeContext.Hide();
        questionContext.Hide();
        base.Hide();
    }






}
