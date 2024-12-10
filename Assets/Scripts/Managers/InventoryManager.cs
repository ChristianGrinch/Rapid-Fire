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
	public List<ItemData> ownedPrimaries;
	public List<ItemData> ownedSecondaries;
	public List<ItemData> selectedGuns = new(2);
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
				if (primary.primaryType == PrimaryType.None) break;

				instantiatedSlot = Instantiate(slotPrefab, storageContainer.transform);

				if(primary.primaryType == PrimaryType.AssaultRifle)
				{
					// Specify all of the data that will be assigned to the slot:
					ItemData newItemData = new()
					{
						itemType = ItemDataType.Primary,
						primaryType = PrimaryType.AssaultRifle,
						gunType = GunType.AssaultRifle,
						ammo = primary.ammo
					};

					instantiatedSlot.GetComponent<Button>().onClick.AddListener(() => SetSlotData(newItemData));
					ActivateSlotImage(InventoryUI.Instance.assaultRifleRT);
				}
				Debug.Log("Ran primary");
			}
		} 
		else if(containerType == "Secondary")
		{
			foreach (var secondary in ownedSecondaries)
			{
				if (secondary.secondaryType == SecondaryType.None) break;

				instantiatedSlot = Instantiate(slotPrefab, storageContainer.transform);

				switch (secondary.secondaryType)
				{
					case SecondaryType.Pistol:
						{
							ItemData newItemData = new()
							{
								itemType = ItemDataType.Secondary,
								secondaryType = SecondaryType.Pistol,
								gunType = GunType.Pistol,
								ammo = secondary.ammo
							};

							instantiatedSlot.GetComponent<Button>().onClick.AddListener(() => SetSlotData(newItemData));
							ActivateSlotImage(InventoryUI.Instance.pistolRT);
							break;
						}

					case SecondaryType.SubMachineGun:
						{
							ItemData newItemData = new()
							{
								itemType = ItemDataType.Secondary,
								secondaryType = SecondaryType.SubMachineGun,
								gunType = GunType.SubMachineGun,
								ammo = secondary.ammo
							};

							instantiatedSlot.GetComponent<Button>().onClick.AddListener(() => SetSlotData(newItemData));
							ActivateSlotImage(InventoryUI.Instance.subMachineGunRT);
							break;
						}
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
				primaryData.itemType = ItemDataType.Primary;

				switch (newItemData.primaryType)
				{
					case PrimaryType.AssaultRifle:
						primaryData.primaryType = PrimaryType.AssaultRifle;
						InventoryUI.Instance.DisplayImage();
						if(!loadingSelectedGuns && selectedGuns[0].primaryType != PrimaryType.AssaultRifle) selectedGuns.Add(primaryData);
						selectedGuns[0].gameObject = FindGameObject(primaryData.gunType);
						break;
				}
				break;
			case ItemDataType.Secondary:
				ItemData secondaryData = WeaponsUI.Instance.secondary.GetComponent<SlotData>().itemData;
				secondaryData.itemType = ItemDataType.Secondary;

				switch (newItemData.secondaryType)
				{
					case SecondaryType.Pistol:
						secondaryData.secondaryType = SecondaryType.Pistol;
						InventoryUI.Instance.DisplayImage();
						if (!loadingSelectedGuns && selectedGuns[1].secondaryType != SecondaryType.Pistol) selectedGuns.Add(secondaryData);
						selectedGuns[1].gameObject = FindGameObject(secondaryData.gunType);
						break;
					case SecondaryType.SubMachineGun:
						secondaryData.secondaryType = SecondaryType.SubMachineGun;
						InventoryUI.Instance.DisplayImage();
						if (!loadingSelectedGuns && selectedGuns[1].secondaryType != SecondaryType.SubMachineGun) selectedGuns.Add(secondaryData);
						selectedGuns[1].gameObject = FindGameObject(secondaryData.gunType);
						break;
				}
				break;
			case ItemDataType.Powerup:

				break;
			case ItemDataType.Armor:

				break;
		}
	}
	public void ActivateSlotImage(RenderTexture renderTexture)
	{
		RawImage rawImage = instantiatedSlot.GetComponentInChildren<RawImage>();
		rawImage.texture = renderTexture;
		rawImage.color = new(255, 255, 255, 255);
	}
	public GameObject FindGameObject(GunType gunType)
	{
		string weaponsPath = "Resources/Weapons";
		string primaryPath = weaponsPath + "/Primary";
		string secondaryPath = weaponsPath + "/Secondary";
		string path = "";

		switch (gunType)
		{
			case GunType.Pistol:
				path = secondaryPath + "/Pistol/Pistol Level 1.prefab";
				break;

			case GunType.AssaultRifle:
				path = primaryPath + "/Assault Rifle/Assault Rifle Level 1";
				break;

			case GunType.SubMachineGun:
				path = secondaryPath + "/Sub Machine Gun/Sub Machine Gun Level 1";
				break;

			default:
				Debug.LogError("Invalid gun type!");
				return null;
		}

		GameObject gunPrefab = Resources.Load<GameObject>(path);

		if (gunPrefab == null)
		{
			Debug.LogError($"Gun prefab not found at path: {path}");
		}

		return gunPrefab;
	}
}
