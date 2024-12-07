using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SlotData;
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
	[Header("Other")]
	public List<PrimaryType> ownedPrimaries;
	public List<SecondaryType> ownedSecondaries;
	public GameObject storageContainer;
	public GameObject slotPrefab;
	public GameObject instantiatedSlot;
	public RawImage slotImage;
	public void FindStorageContainer(string name, GameObject parent)
	{
		storageContainer = parent.transform.Find(name).gameObject;
		FillStorageContainer(name.Split(" ")[0]);
	}
	public void FillStorageContainer(string containerType)
	{
		if(containerType == "Primary")
		{
			foreach (var primary in ownedPrimaries)
			{
				instantiatedSlot = Instantiate(slotPrefab, storageContainer.transform);

				if(primary == PrimaryType.AssaultRifle)
				{
					RawImage rawImage = instantiatedSlot.GetComponent<RawImage>();
					rawImage.texture = InventoryUI.Instance.assaultRifleRT;
					rawImage.color = new(255, 255, 255, 255);
				}

				Debug.Log("Ran primary");
			}
		} 
		else if(containerType == "Secondary")
		{
			foreach (var secondary in ownedSecondaries)
			{
				instantiatedSlot = Instantiate(slotPrefab, storageContainer.transform);

				if (secondary == SecondaryType.Pistol) 
				{
					RawImage rawImage = instantiatedSlot.GetComponentInChildren<RawImage>();
					rawImage.texture = InventoryUI.Instance.pistolRT;
					rawImage.color =  new(255, 255, 255, 255);
				}
				Debug.Log("Ran secondary");
			}
		}
	}
}
