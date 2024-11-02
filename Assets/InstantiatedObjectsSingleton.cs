using UnityEngine;

public class InstantiatedObjectsSingleton : MonoBehaviour
{
	public static InstantiatedObjectsSingleton Instance { get; private set; }
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
