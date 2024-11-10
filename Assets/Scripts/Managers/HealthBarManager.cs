using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
	public static HealthBarManager Instance { get; private set; }
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
	public GameObject healthBarPrefab;
	public Canvas canvas;
	public GameObject GetHealthBarPrefab()
	{
		return healthBarPrefab;
	}
	public GameObject GetCanvas()
	{
		return canvas;
	}
}
