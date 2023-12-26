using System;
using UnityEngine;
using UnityEngine.UI;

public class DeskUI : MonoBehaviour
{
    [SerializeField] private Button selectButton, editButton, deleteButton;
    [SerializeField] private Text nameText, countCardsText, preQuestionText;

    Action<Desk> onSelect;
    Action<Desk> onEdit;

    public Desk currentDesk
    {
        get;
        private set;
    }

    public Action<DeskUI> onDelete;

    public void Init(Desk desk, Action<Desk> selectCallback, Action<Desk> editCallback)
    {
        currentDesk = desk;
        currentDesk.onUpdate += UpdateUI;
        currentDesk.onDelete += OnDelete;
        UpdateUI();
        onSelect = selectCallback;
        onEdit = editCallback;
        
        AddListeners();
    }


    private void UpdateUI()
    {
        nameText.text = currentDesk.name;
        countCardsText.text = DataBase.instance.GetCountCards(currentDesk).ToString();
        var preQuestion = DataBase.instance.GetFirstQuestion(currentDesk);
        if(preQuestion != null)
        {
            preQuestionText.text = preQuestion;
        }
        else
        {
            preQuestionText.text = "";
        }
    }

    private void AddListeners()
    {
        selectButton.onClick.AddListener(OnSelect);
        editButton.onClick.AddListener(OnEdit);
        deleteButton.onClick.AddListener(DeleteDesk);
    }

    private void DeleteDesk()
    {
        currentDesk.onDelete?.Invoke();
    }

    private void OnSelect()
    {
        onSelect?.Invoke(currentDesk);
    }

    private void OnEdit()
    {
        onEdit?.Invoke(currentDesk);
    }

    private void OnDelete()
    {
        onDelete?.Invoke(this);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        currentDesk.onUpdate -= UpdateUI;
        currentDesk.onDelete -= OnDelete;
        selectButton.onClick.RemoveListener(OnSelect);
        editButton.onClick.RemoveListener(OnEdit);
        deleteButton.onClick.RemoveListener(DeleteDesk);
    }


}
