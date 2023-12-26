using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardsMenuInfo : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Text nameText, cardsCount;


    public void UpdateUI(Desk desk)
    {
        nameText.text = desk.Name;
        cardsCount.text = DataBase.instance.GetCountCards(desk).ToString();
    }

}
