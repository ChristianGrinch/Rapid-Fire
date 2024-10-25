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
		//switch (Screen.fullScreenMode)
		//{
		//	case FullScreenMode.ExclusiveFullScreen:
		//		screenMode.value = 0;
		//		break;
		//	case FullScreenMode.FullScreenWindow:
		//		screenMode.value = 1;
		//		break;
		//	case FullScreenMode.MaximizedWindow:
		//		screenMode.value = 2;
		//		break;
		//	case FullScreenMode.Windowed:
		//		screenMode.value = 3;
		//		break;
		//}
	}
	private void ChangeScreenMode(int selectedIndex)
	{
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
