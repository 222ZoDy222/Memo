using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingDeskUI : MonoBehaviour
{
    [SerializeField] private Text nameText, completedCardsText, maxCardsValue;
    [SerializeField] private Button selectButton;

    public Desk currentDesk
    {
        get;
        private set;
    }

    Action<Desk> onSelect;

    public void Init(Desk desk, Action<Desk> selectCallback)
    {
        currentDesk = desk;
        currentDesk.onUpdate += UpdateUI;
        
        UpdateUI();
        onSelect = selectCallback;

        selectButton.onClick.AddListener(OnSelect);
    }

    private void OnSelect()
    {
        onSelect?.Invoke(currentDesk);
    }

    private void UpdateUI()
    {
        nameText.text = currentDesk.name;
        completedCardsText.text = DataBase.instance.GetCountCompletedCards(currentDesk).ToString();
        maxCardsValue.text = DataBase.instance.GetCountCards(currentDesk).ToString();
    }


    private void OnDestroy()
    {
        if(currentDesk != null)
        {
            currentDesk.onUpdate -= UpdateUI;
        }
        
        selectButton.onClick.RemoveListener(OnSelect);

    }

}
