using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashCardUI : MonoBehaviour
{


    [SerializeField] private Button editButton, deleteButton;
    [SerializeField] private Text questionText;

    Action<FlashCard> onEdit;


    public FlashCard currentCard { get; private set; }

    public Action<FlashCardUI> onDelete;


    public void Init(FlashCard card, Action<FlashCard> editCallback)
    {
        currentCard = card;
        currentCard.onUpdate += UpdateUI;
        currentCard.onDelete += OnDelete;
        UpdateUI();
        
        onEdit = editCallback;

        AddListeners();
    }

    private void UpdateUI()
    {
        questionText.text = currentCard.question;
    }

    private void AddListeners()
    {
        
        editButton.onClick.AddListener(OnEdit);
        deleteButton.onClick.AddListener(DeleteDesk);
    }

    private void OnEdit()
    {
        onEdit?.Invoke(currentCard);
    }

    private void DeleteDesk()
    {
        currentCard.onDelete?.Invoke();
    }

    private void OnDelete()
    {
        onDelete?.Invoke(this);
        Destroy(this.gameObject);
    }


    private void OnDestroy()
    {
        currentCard.onUpdate -= UpdateUI;
        currentCard.onDelete -= OnDelete;
        editButton.onClick.RemoveListener(OnEdit);
        deleteButton.onClick.RemoveListener(DeleteDesk);
    }

}
