using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuCreateCard : MonoBehaviour
{

    [SerializeField] private CanvasGroup firstSide, secondSide;

    [SerializeField] private TMP_InputField questionField, answerField, infoField;


    [SerializeField] private CardsMenu cardMenu;

    [SerializeField] private Button hideButton;



    private FlashCard currentCard;


    private void Awake()
    {
        contextMenuRT = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Hide();
        
    }

    public void SwitchSides()
    {
        if(firstSide.alpha == 0)
        {
            secondSide.ShowMenu(false);
            firstSide.ShowMenu(true);
        }
        else
        {
            secondSide.ShowMenu(true);
            firstSide.ShowMenu(false);
        }
    }

    
    private void RemoveListeners()
    {
        hideButton.onClick.RemoveAllListeners();
    }


    

    

   


    public void ShowCreate()
    {
        blackBackground.SetActive(true);
        secondSide.ShowMenu(false);
        firstSide.ShowMenu(true);
        ShowAnimation();

        hideButton.onClick.AddListener(HideCreate);

        questionField.text = "";
        answerField.text = "";
        infoField.text = "";
    }

    public void ShowEdit(FlashCard card)
    {
        
        blackBackground.SetActive(true);
        secondSide.ShowMenu(false);
        firstSide.ShowMenu(true);

        currentCard = card;
        ShowAnimation();
        hideButton.onClick.AddListener(HideEdit);

        questionField.text = card.question;
        answerField.text = card.answer;
        infoField.text = card.info;

    }

    private void Hide()
    {
        RemoveListeners();
        blackBackground.SetActive(false);
    }

    private void HideCreate()
    {
        if (answerField.text == "" || questionField.text == "")
        {

        }
        else
        {
            FlashCard flashCard = new FlashCard(questionField.text, answerField.text);
            flashCard.info = infoField.text;
            cardMenu.CreateCard(flashCard);
        }
        
        Hide();
    }

    private void HideEdit()
    {
        if (answerField.text == "" || questionField.text == "")
        {

        }
        else
        {
            currentCard.question = questionField.text;
            currentCard.answer = answerField.text;
            currentCard.info = infoField.text;
            currentCard.onUpdate?.Invoke();
        }
        Hide();
    }




    private RectTransform contextMenuRT;
    [SerializeField]
    private RectTransform hidePos, showPos;
    [SerializeField] private GameObject blackBackground;


    private void ShowAnimation()
    {
        contextMenuRT.DOLocalMove(hidePos.localPosition, 0f);
        contextMenuRT.DOLocalMove(showPos.localPosition, 0.5f);
    }




}
