using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerContextMenu : Menu
{

    [SerializeField]
    private Text answerText,
        questionText, realAnswerText;

    [SerializeField] private Button isGoodAnswer, isNotGoodAnswer;

    [SerializeField] private DuringTraining duringTraining;

    private FlashCard currentCard;

    private void Start()
    {
        Hide();
        isGoodAnswer.onClick.AddListener(GoodAnswer);
        isNotGoodAnswer.onClick.AddListener(BadAnswer);
    }


    public void Show(FlashCard card, string answer)
    {

        currentCard = card;
        if(currentCard.answer.ToLower() == answer.ToLower())
        {
            GoodAnswer();
            return;
        }

        base.Show();
        
        answerText.text = answer;
        realAnswerText.text = card.answer;        
        questionText.text = card.question;
        


    }


    private void GoodAnswer()
    {
        Hide();
        currentCard.lastAnswer = true;
        currentCard.onUpdate?.Invoke();
        duringTraining.NextCard();
    }

    private void BadAnswer()
    {
        Hide();
        currentCard.lastAnswer = false;
        currentCard.onUpdate?.Invoke();
        duringTraining.NextCard();
    }
   










}
