using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoPanelUI : MonoBehaviour
{
    public static VideoPanelUI Instance { get; private set; }
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
    [Header("Panel")]
    public GameObject videoPanel;
	public TMP_Dropdown screenMode;
    void Start()
    {
		screenMode.onValueChanged.AddListener(ChangeScreenMode);
		if (screenMode.options.Count == 0)
		{
			screenMode.options.Add(new TMP_Dropdown.OptionData("Exclusive Fullscreen"));
			screenMode.options.Add(new TMP_Dropdown.OptionData("Fullscreen Window"));
			screenMode.options.Add(new TMP_Dropdown.OptionData("Maximized Window"));
			screenMode.options.Add(new TMP_Dropdown.OptionData("Windowed"));
		}
	}
	public void ChangeScreenMode(int selectedIndex)
	{
		SettingsMenuUI.Instance.didModifySettings = true;
		switch (selectedIndex)
		{
			case 0:
				Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
				break;
			case 1:
				Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
				break;
			case 2:
				Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
				break;
			case 3:
				Screen.fullScreenMode = FullScreenMode.Windowed;
				break;
		}
	}
}
