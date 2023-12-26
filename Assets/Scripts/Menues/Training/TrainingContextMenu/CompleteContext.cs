using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteContext : Menu
{
    [SerializeField] private MenuPanelSwitcher menuPanelSwitcher;

    [SerializeField] private UnityEngine.UI.Text percentText;
    [SerializeField] private UnityEngine.UI.Button okButton;


    private void Start()
    {
        Hide();
        okButton.onClick.AddListener(Ok);
    }

    public void Show(Desk desk)
    {
        base.Show();
        percentText.text = desk.PercentRightAnswers.ToString() + " %";
    }

    private void Ok()
    {
        menuPanelSwitcher.ShowTraining();
    }

}
