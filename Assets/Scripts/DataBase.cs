
using Saver;
using System.Collections.Generic;
using System.IO;

public class DataBase
{

    private static DataBase m_instance;

    public static DataBase instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = new DataBase();
            }
            return m_instance;
        }
    }


    #region Desks

    public string DESK_PATH
    {
        get
        {
            return Path.Combine(SaveManager.pathFolder, "Desk");           
        }
    }


    /// <summary>
    /// Вернуть все доски
    /// </summary>
    /// <returns></returns>
    public List<Desk> GetAllDesks()
    {
        string[] desksFolders = GetAllDesksFolders();



        if(desksFolders != null)
        {
            //Desk[] desks = new Desk[desksFolders.Length];
            List<Desk> desks = new List<Desk>();
            for (int i = 0; i < desksFolders.Length; i++)
            {
                var desk = GetDeskInfo(desksFolders[i]);
                if (desk != null) desks.Add(desk);
            }
            return desks;
        }

        return null;
    }

    /// <summary>
    /// Получить все папки досок
    /// </summary>
    /// <returns></returns>
    private string[] GetAllDesksFolders() => Saver.Saver.GetDirectoriesFrom(DESK_PATH);
    

    private Desk GetDeskInfo(string path)
    {
        if (path == null) return null;

        string fullPath = Path.Combine(path, "info.json");
        var desk = SaveManager.Load<Desk>(fullPath);

        return desk;

    }


    public void AddDesk(Desk desk)
    {
        var pathToSave = Path.Combine(DESK_PATH, desk.SaveID);
        SaveManager.Save<Desk>("info", pathToSave, desk);
    }

    public void DeleteDesk(Desk desk)
    {
        var pathToSave = Path.Combine(DESK_PATH, desk.SaveID);
        SaveManager.Delete(pathToSave);
    }

    public void UpdateDeskInfo(Desk desk)
    {
        var pathToSave = Path.Combine(DESK_PATH, desk.SaveID);
        SaveManager.Save<Desk>("info", pathToSave, desk);
    }

    public int GetCountCards(Desk desk)
    {
        string pathCards = Path.Combine(DESK_PATH, desk.SaveID, "Cards");
        if (!Directory.Exists(pathCards))
        {
            return 0;
        }
        return Directory.GetFiles(pathCards).Length;        
    }

    public string GetFirstQuestion(Desk desk)
    {
        string pathCards = Path.Combine(DESK_PATH, desk.SaveID, "Cards");
        if (!Directory.Exists(pathCards))
        {
            return null;
        }
        string[] cardsDir = Directory.GetFiles(pathCards);

        for (int i = 0; i < cardsDir.Length; i++)
        {
            FlashCard flashCard = SaveManager.Load<FlashCard>(cardsDir[i]);
            if (flashCard != null)
            {
                return flashCard.question;
            }
        }
        return null;
    }


    public int GetCountCompletedCards(Desk desk)
    {
        string pathCards = Path.Combine(DESK_PATH, desk.SaveID, "Cards");
        if (!Directory.Exists(pathCards))
        {
            return 0;
        }
        string[] cardsDir = Directory.GetFiles(pathCards);

        int completedCards = 0;

        for (int i = 0; i < cardsDir.Length; i++)
        {
            FlashCard flashCard = SaveManager.Load<FlashCard>(cardsDir[i]);
            if (flashCard != null)
            {

                if (flashCard.lastAnswer) completedCards++;
            }
        }

        return completedCards;
    }

    #endregion

    #region Cards

    public void GetAllCards(Desk desk)
    {
        if (desk.flashCards == null) 
            desk.flashCards = new System.Collections.Generic.List<FlashCard>();        
        else desk.flashCards.Clear();

        string pathCards = Path.Combine(DESK_PATH, desk.SaveID, "Cards");
        if (!Directory.Exists(pathCards))
        {
            return;
        }
        string[] cardsDir = Directory.GetFiles(pathCards);

        for (int i = 0; i < cardsDir.Length; i++)
        {
            FlashCard jsonCard = SaveManager.Load<FlashCard>(cardsDir[i]);
            
            
            
            if (jsonCard != null)
            {
                FlashCard flashCard = new FlashCard(jsonCard);
                desk.flashCards.Add(flashCard);
                flashCard.desk = desk;
            }
        }

    }

    public void AddCard(Desk desk, FlashCard card)
    {

        string pathCards = Path.Combine(DESK_PATH, desk.SaveID, "Cards");

        SaveManager.Save<FlashCard>(card.id.ToString(), pathCards, card);
        desk.flashCards.Add(card);
        card.desk = desk;

    }

    public void UpdateCardInfo(FlashCard card)
    {
        string pathCards = Path.Combine(DESK_PATH, card.desk.SaveID, "Cards");

        SaveManager.Save<FlashCard>(card.id.ToString(), pathCards, card);
    }

    public void DeleteCard(FlashCard card)
    {
        string pathCard = Path.Combine(DESK_PATH, card.desk.SaveID, "Cards", card.id.ToString());

        SaveManager.Delete(pathCard);
    }

    #endregion



}
