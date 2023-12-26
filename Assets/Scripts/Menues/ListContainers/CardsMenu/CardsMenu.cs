using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CardsMenu : Menu
{
    [SerializeField] private CardsMenuInfo cardsMenuInfo;

    private Desk currentDesk;


    [SerializeField] private ReorderableList reorderableList;
    [SerializeField] private Transform spawnContainer;
    [SerializeField] private FlashCardUI cardUIPrefab;

    [SerializeField] private ContextMenuCreateCard contextMenu;

    private List<FlashCardUI> cardUis = new List<FlashCardUI>();

    private void Start()
    {
        reorderableList.OnElementDropped.AddListener(OnElementDropped);
    }

    public void ShowDesk(Desk desk)
    {
        base.Show();
        currentDesk = desk;
        InitDesk(currentDesk);

        LoadAllCards();
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

        for (int i = 0; i < cardUis.Count; i++)
        {
            yield return null;
            yield return null;
            yield return null;

            cardUis[i].currentCard.position = cardUis[i].transform.GetSiblingIndex();
            cardUis[i].currentCard.onUpdate?.Invoke();

        }

    }


    private void InitDesk(Desk desk)
    {
        cardsMenuInfo.UpdateUI(desk);
    }

    private void LoadAllCards()
    {
        // Подгрузить все доски
        DataBase.instance.GetAllCards(currentDesk);

        currentDesk.SortCards();

        LoadCardsUI();

    }

    private void LoadCardsUI()
    {
        spawnContainer.DestroyAllChildren();
        foreach (var card in currentDesk.flashCards)
        {
            AddCard(card);
        }
    }


    private uint lastMaxID = 1;

    public void CreateCard(FlashCard card)
    {
        FlashCard flashCard = new FlashCard(card.question, card.answer);        
        flashCard.info = card.info;        
        flashCard.id = lastMaxID;
        lastMaxID++;
        flashCard.position = spawnContainer.childCount;

        // Добавить карточку в БД
        DataBase.instance.AddCard(currentDesk, flashCard);


        SpawnCard(flashCard);

    }

    public void AddCard(FlashCard card)
    {
        if (card.id >= lastMaxID) lastMaxID = card.id + 1;
        SpawnCard(card);
    }

    private void SpawnCard(FlashCard card)
    {
        if (card == null) return;

        

        var cardUI = Instantiate(cardUIPrefab, spawnContainer);

        cardUI.Init(card, OnEditDesk);
        cardUI.onDelete += OnDelete;
        cardUis.Add(cardUI);

    }

    private void OnDelete(FlashCardUI cardUI)
    {
        cardUis.Remove(cardUI);
    }

    private void OnEditDesk(FlashCard card)
    {
        contextMenu.ShowEdit(card);
    }

}
