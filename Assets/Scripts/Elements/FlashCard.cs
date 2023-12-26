using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashCard : Card
{

    public FlashCard(string question, string answer)
    {
        this.question = question;
        this.answer = answer;
        onUpdate += OnUpdate;
        onDelete += OnDelete;
    }

    public FlashCard(FlashCard card)
    {
        this.answer = card.answer;
        this.id = card.id;
        this.info = card.info;
        this.lastAnswer = card.lastAnswer;
        this.name = card.name;
        this.position = card.position;
        this.question = card.question;
        onUpdate += OnUpdate;
        onDelete += OnDelete;

    }

    public string question;

    public string answer;

    public string info;

    public bool lastAnswer;

    public Desk desk { get; set; }

    private void OnUpdate()
    {
        // Обновление информации в БД
        DataBase.instance.UpdateCardInfo(this);
    }

    private void OnDelete()
    {
        // Удалить карточку из бд
        DataBase.instance.DeleteCard(this);
        desk.flashCards.Remove(this);
    }
}
