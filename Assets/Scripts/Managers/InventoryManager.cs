using UnityEngine;
public class InventoryManager : MonoBehaviour
{
	public static InventoryManager Instance { get; private set; }
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
}
