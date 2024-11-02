using UnityEngine;

public class SettingsCanvasSingleton : MonoBehaviour
{
	public static SettingsCanvasSingleton Instance { get; private set; }
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
