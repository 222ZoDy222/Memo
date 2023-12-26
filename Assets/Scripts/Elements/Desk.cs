using System;
using System.Collections.Generic;

[Serializable]
public class Desk : Card
{
   
    public string Name
    {
        get => name;
        set
        {            
            name = value;
        }
    }

    public void SortCards()
    {
        flashCards.Sort((x, y) => x.position.CompareTo(y.position));
    }

    public float PercentRightAnswers
    {
        get
        {
            int countRightAnswers = 0;
            for (int i = 0; i < flashCards.Count; i++)
            {
                if (flashCards[i].lastAnswer) countRightAnswers++;
            }
            return (int)((float)((float)countRightAnswers / (float)flashCards.Count) * 100); 
        }
    }

    public Desk(string name, uint id)
    {
        this.name = name;
        this.id = id;
        Init();
    }

    public void Init()
    {
        onUpdate += OnUpdate;
        onDelete += OnDelete;
    }
    
    public List<FlashCard> flashCards { get; set; }

    private void OnUpdate()
    {
        // Обновление информации в БД
        DataBase.instance.UpdateDeskInfo(this);
    }

    private void OnDelete()
    {
        // Удалить доску из бд
        DataBase.instance.DeleteDesk(this);
    }

}
