using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public static PauseMenuUI Instance { get; private set; }
    public Button saveBtn;
    public Button quitGameBtn;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        saveBtn.onClick.AddListener(() => SaveManager.Instance.SavePlayer(SaveManager.Instance.currentSave));
        quitGameBtn.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.QuitGameConfirm));
    }
}
