using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ContextMenuCreateDesk : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;

    [SerializeField] private DeskMenu deskMenu;

    
    private Desk currentDesk;


    private void Awake()
    {
        contextMenuRT = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Hide();
    }


    private void AddListeners()
    {
        nameField.onEndEdit.AddListener(CreateDesk);
    }

    private void RemoveListeners()
    {
        nameField.onEndEdit.RemoveAllListeners();
    }


    private void AddListenersEdit()
    {
        nameField.onEndEdit.AddListener(EditDesk);
    }

    private void CreateDesk(string value)
    {
        if(value != "")
        {
            deskMenu.CreateDesk(value);
        }
        Hide();
    }

    private void EditDesk(string value)
    {
        if(value != "")
        {
            currentDesk.Name = value;
            currentDesk.onUpdate?.Invoke();

        }
        Hide();
    }

    public void ShowCreate()
    {
        blackBackground.SetActive(true);
        ShowAnimation();
        RemoveListeners();
        nameField.text = "";
        AddListeners();
    }


    public void Hide()
    {
        RemoveListeners();
        blackBackground.SetActive(false);
    }

    public void ShowEdit(Desk desk)
    {
        blackBackground.SetActive(true);
        ShowAnimation();
        currentDesk = desk;
        nameField.text = currentDesk.name;
        AddListenersEdit();
    }


    private RectTransform contextMenuRT;
    [SerializeField]
    private RectTransform hidePos, showPos;
    [SerializeField] private GameObject blackBackground;


    private void ShowAnimation()
    {
        contextMenuRT.DOLocalMove(hidePos.localPosition, 0f);
        contextMenuRT.DOLocalMove(showPos.localPosition, 0.5f);
    }

    


}
