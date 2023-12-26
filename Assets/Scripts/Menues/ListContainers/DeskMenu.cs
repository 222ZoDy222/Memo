using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saver;
using UnityEngine.UI.Extensions;
using DG.Tweening;
using System;
using System.Linq;

public class DeskMenu : Menu
{

    public readonly string path;

    [SerializeField] private ReorderableList reorderableList;
    [SerializeField] private Transform spawnContainer;
    [SerializeField] private DeskUI deskUIPrefab;

    [SerializeField] private ContextMenuCreateDesk contextMenu;

    [SerializeField] private CardsMenu cardsMenu;

    private List<DeskUI> deskUIs = new List<DeskUI>();


    private void Start()
    {
        reorderableList.OnElementDropped.AddListener(OnElementDropped);
    }

    public override void Show()
    {
        cardsMenu.Hide();

        base.Show();
        // Подгрузить все доски из БД
        LoadAllDesks();

        
    }

    private void OnElementDropped(ReorderableList.ReorderableListEventStruct arg0)
    {
        RecheckPosition();
    }

    /// <summary>
    /// After Element Dropped
    /// </summary>
    private void RecheckPosition()
    {
        if (checkPositionRoutine != null) StopCoroutine(checkPositionRoutine);

        checkPositionRoutine = StartCoroutine(routinePosition());
    }

    Coroutine checkPositionRoutine;
    
    IEnumerator routinePosition()
    {

        for (int i = 0; i < deskUIs.Count; i++)
        {
            yield return null;
            yield return null;
            yield return null;

            try
            {
                deskUIs[i].currentDesk.position = deskUIs[i].transform.GetSiblingIndex();
                deskUIs[i].currentDesk.onUpdate?.Invoke();
            }
            catch
            {

            }
            

        }

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

    
    

    private uint lastMaxID = 1;

    public void CreateDesk(string name)
    {        
        Desk desk = new Desk(name, lastMaxID);
        lastMaxID++;
        desk.position = spawnContainer.childCount;
        SpawnDesk(desk);
        // Добавить доску в БД
        DataBase.instance.AddDesk(desk);
    }


    private void AddDesk(Desk desk)
    {
        if (desk == null) return;
        
        if (desk.id >= lastMaxID) lastMaxID = desk.id + 1;
        desk.Init();
        SpawnDesk(desk);
    }
    
    private void SpawnDesk(Desk desk)
    {
        if (desk == null) return;
        var deskUI = Instantiate(deskUIPrefab, spawnContainer);
        
        deskUI.Init(desk, OnSelectDesk, OnEditDesk);
        deskUI.onDelete += OnDelete;
        deskUIs.Add(deskUI);
        
        
    }

    private void OnDelete(DeskUI deskUI)
    {
        deskUIs.Remove(deskUI);
    }

    private void OnSelectDesk(Desk desk)
    {
        Hide();
        cardsMenu.ShowDesk(desk);
    }

    private void OnEditDesk(Desk desk)
    {
        contextMenu.ShowEdit(desk);
    }

    


   

    public void ShowDeskCreate()
    {              
        contextMenu.ShowCreate();
    }

    public void HideDeskCreate()
    {
        contextMenu.Hide();
    }






}
