using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuPanelSwitcher : MonoBehaviour
{

    [Header("All menues")]    
    [SerializeField] private Menu deskManager, cardManager, traningMenu;


    [SerializeField] private CanvasGroup settingsButton, deskButton, trainingButton;

    [SerializeField] private float alphaHide = 0.2f;

    public MenuType currentMenu { get; private set; }

    private void Start()
    {
        ShowDesksMenu();
    }

    public void HideAllContainers()
    {
        deskManager.Hide();
        cardManager.Hide();
        traningMenu.Hide();
        settingsButton.alpha = alphaHide;
        deskButton.alpha = alphaHide;
        trainingButton.alpha = alphaHide;
    }


    


    public void ShowDesksMenu()
    {
        HideAllContainers();
        deskManager.Show();
        deskButton.alpha = 1;
    }

    

    public void ShowTraining()
    {
        HideAllContainers();
        traningMenu.Show();
        trainingButton.alpha = 1;
    }


    public void ShowSettings()
    {
        HideAllContainers();
        
        settingsButton.alpha = 1;
    }
    



   


    public enum MenuType
    {

        mainMenu,
        editDesk,
        editCard,
        training,


    }


}



public static class CanvasExtension 
{

    public static void ShowMenu(this CanvasGroup canvas, bool value)
    {

        if (value) canvas.alpha = 1;
        else canvas.alpha = 0;

        canvas.blocksRaycasts = value;
        canvas.interactable = value;


    }

    public static void DestroyAllChildren(this Transform transform)
    {
        foreach (Transform child in transform) UnityEngine.Object.Destroy(child.gameObject);
    }


}


