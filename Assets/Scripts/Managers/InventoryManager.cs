using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SlotData;
using static GunController;
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
	[Header("Player owned weapons")]
	public List<PrimaryType> ownedPrimaries;
	public List<SecondaryType> ownedSecondaries;
	public List<ItemData> selectedGuns;
	[Header("Other")]
	public GameObject storageContainer;
	public GameObject slotPrefab;
	public GameObject instantiatedSlot;
	public RawImage slotImage;
	private bool loadingSelectedGuns;
	private void Start()
	{
		foreach(var gun in selectedGuns)
		{
			loadingSelectedGuns = true;
			SetSlotData(gun);
		}
		loadingSelectedGuns = false;
	}
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
				// Makes sure not to render an empty slot
				if (primary == PrimaryType.None) break;

				instantiatedSlot = Instantiate(slotPrefab, storageContainer.transform);

				if(primary == PrimaryType.AssaultRifle)
				{
					// Specify all of the data that will be assigned to the slot:
					ItemData newItemData = new()
					{
						itemType = ItemDataType.Primary,
						primaryType = PrimaryType.AssaultRifle
					};

					instantiatedSlot.GetComponent<Button>().onClick.AddListener(() => SetSlotData(newItemData));
					RawImage rawImage = instantiatedSlot.GetComponentInChildren<RawImage>();
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
				// Makes sure not to render an empty slot
				if (secondary == SecondaryType.None) break;

				instantiatedSlot = Instantiate(slotPrefab, storageContainer.transform);

				if (secondary == SecondaryType.Pistol) 
				{
					// Specify all of the data that will be assigned to the slot:
					ItemData newItemData = new()
					{
						itemType = ItemDataType.Secondary,
						secondaryType = SecondaryType.Pistol 
					};

					instantiatedSlot.GetComponent<Button>().onClick.AddListener(() => SetSlotData(newItemData));
					RawImage rawImage = instantiatedSlot.GetComponentInChildren<RawImage>();
					rawImage.texture = InventoryUI.Instance.pistolRT;
					rawImage.color =  new(255, 255, 255, 255);
				}
				Debug.Log("Ran secondary");
			}
		}
	}
	public void SetSlotData(ItemData newItemData)
	{
		Debug.Log("SetSlotData ran.");
		switch (newItemData.itemType)
		{
			case ItemDataType.None:
				break;
			case ItemDataType.Primary:
				ItemData primaryData = WeaponsUI.Instance.primary.GetComponent<SlotData>().itemData;

				switch (newItemData.primaryType)
				{
					case PrimaryType.AssaultRifle:
						primaryData.itemType = ItemDataType.Primary;
						primaryData.primaryType = PrimaryType.AssaultRifle;
						InventoryUI.Instance.DisplayImage();
						if(!loadingSelectedGuns) selectedGuns.Add(primaryData);
						break;
				}
				break;
			case ItemDataType.Secondary:
				ItemData secondaryData = WeaponsUI.Instance.secondary.GetComponent<SlotData>().itemData;

				switch (newItemData.secondaryType)
				{
					case SecondaryType.Pistol:
						Debug.Log("Set Pistol data ran.");
						secondaryData.itemType = ItemDataType.Secondary;
						secondaryData.secondaryType = SecondaryType.Pistol;
						InventoryUI.Instance.DisplayImage();
						if (!loadingSelectedGuns) selectedGuns.Add(secondaryData);
						break;
				}
				break;
			case ItemDataType.Powerup:

				break;
			case ItemDataType.Armor:

				break;
		}
	}
}
