using UnityEngine;

public class CanvasSingleton : MonoBehaviour
{
	public static CanvasSingleton Instance { get; private set; }
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
