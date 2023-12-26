using System;

[Serializable]
public class Card
{
    public uint id;
    public string name;

    public int position;

    public string SaveID
    {
        get => $"ID_{id.ToString("X")}";
    }

    public Action onUpdate;

    public Action onDelete;
}
