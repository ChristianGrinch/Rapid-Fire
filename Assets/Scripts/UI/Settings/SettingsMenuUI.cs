using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuUI : MonoBehaviour
{
    public static SettingsMenuUI Instance { get; private set; }
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
    [Header("Menu")]
    public GameObject settingsMenu;
}
