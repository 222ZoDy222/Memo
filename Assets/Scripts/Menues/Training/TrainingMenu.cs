using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingMenu : Menu
{
    [SerializeField] private Transform spawnContainer;
    [SerializeField] private TrainingDeskUI deskUIPrefab;

    [SerializeField] private DuringTraining duringTraining;

    private List<TrainingDeskUI> deskUIs = new List<TrainingDeskUI>();



    public override void Show()
    {
        duringTraining.Hide();
        base.Show();
        // Подгрузить все доски из БД
        LoadAllDesks();


    }

    public override void Hide()
    {
        duringTraining.Hide();
        base.Hide();
    }

    private void LoadAllDesks()
    {

        // Подгрузить все доски
        var desks = DataBase.instance.GetAllDesks();

        spawnContainer.DestroyAllChildren();

        List<Desk> SortedList = desks.OrderBy(o => o.position).ToList();

        for (int i = 0; i < SortedList.Count; i++)
        {
            AddDesk(SortedList[i]);
        }

    }


    private void AddDesk(Desk desk)
    {
        if (desk == null) return;
        var deskUI = Instantiate(deskUIPrefab, spawnContainer);

        deskUI.Init(desk, OnSelectDesk);        
        deskUIs.Add(deskUI);
    }

    private void OnSelectDesk(Desk desk)
    {
        DataBase.instance.GetAllCards(desk);
        duringTraining.StartTraining(desk);

    }



}
