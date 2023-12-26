using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionContextMenu : Menu
{

    [SerializeField] private InputField inputField;

    [SerializeField] private Text infoText, questionText;

    [SerializeField] private Button flipButton;

    [SerializeField] private AnswerContextMenu answerContext;


    private FlashCard currentCard;


    private void Start()
    {
        Hide();
        flipButton.onClick.AddListener(FlipCard);
    }

    public void Show(FlashCard card)
    {
        answerContext.Hide();
        base.Show();
        currentCard = card;
        infoText.text = card.info;
        questionText.text = card.question;
        inputField.text = "";
    }


    private void FlipCard()
    {
        answerContext.Show(currentCard, inputField.text);
    }

    public override void Hide()
    {
        answerContext.Hide();
        base.Hide();
        
    }



}
