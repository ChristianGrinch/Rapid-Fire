using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyMenuUI : MonoBehaviour
{
    public Button easyDifficulty;
    public Button mediumDifficulty;
    public Button masterDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        easyDifficulty.onClick.AddListener(() => SetDifficulty(1));
        mediumDifficulty.onClick.AddListener(() => SetDifficulty(2));
        masterDifficulty.onClick.AddListener(() => SetDifficulty(3));
    }
    void SetDifficulty(int newDifficulty)
    {
        SaveManager.Instance.difficulty = newDifficulty;

        Debug.Log("Set difficulty to " + newDifficulty);
    }
}
