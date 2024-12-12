using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SlotData;
using static GunController;
using System.Linq;
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
	public List<ItemData> selectedGuns = new(2)
	{
		new ItemData(),
		new ItemData()
	};
	[Header("Other")]
	public GameObject storageContainer;
	public GameObject slotPrefab;
	public GameObject instantiatedSlot;
	public RawImage slotImage;
	private bool loadingSelectedGuns;
	public WeaponsDatabase weaponsDatabase;
	private void Start()
	{
		List<ItemData> selectedGunsCopy = selectedGuns.ToList();
		foreach(var gun in selectedGunsCopy)
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
						ammo = primary.ammo,
						gameObject = weaponsDatabase.FindGameObjects("Weapons/Primary/Assault Rifle")[0],
						isWeaponAutomatic = true
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
								ammo = secondary.ammo,
								gameObject = weaponsDatabase.FindGameObjects("Weapons/Secondary/Pistol")[0],
								isWeaponAutomatic = false
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
								ammo = secondary.ammo,
								gameObject = weaponsDatabase.FindGameObjects("Weapons/Secondary/Sub Machine Gun")[0],
								isWeaponAutomatic = true
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
		ItemData itemData;
		switch (newItemData.itemType)
		{
			case ItemDataType.None:
				break;
			case ItemDataType.Primary:
				switch (newItemData.primaryType)
				{
					case PrimaryType.AssaultRifle:
						if (loadingSelectedGuns && selectedGuns[0].itemType == ItemDataType.None) return;
						itemData = new ItemData
						{
							itemType = ItemDataType.Primary,
							primaryType = PrimaryType.AssaultRifle,
							gunType = GunType.AssaultRifle,
							gameObject = weaponsDatabase.FindGameObjects("Weapons/Primary/Assault Rifle")[0],
							isWeaponAutomatic = true

						};
						selectedGuns[0] = itemData;
						InventoryUI.Instance.DisplayImage();
						break;
				}
				break;
			case ItemDataType.Secondary:
				selectedGuns[1].itemType = ItemDataType.Secondary;

				switch (newItemData.secondaryType)
				{
					case SecondaryType.Pistol:
						if (loadingSelectedGuns && selectedGuns[0].itemType == ItemDataType.None) return;
						itemData = new ItemData
						{
							itemType = ItemDataType.Secondary,
							secondaryType = SecondaryType.Pistol,
							gunType = GunType.Pistol,
							gameObject = weaponsDatabase.FindGameObjects("Weapons/Secondary/Pistol")[0],
							isWeaponAutomatic = false
						};
						selectedGuns[1] = itemData;
						InventoryUI.Instance.DisplayImage();
						break;
					case SecondaryType.SubMachineGun:
						if (loadingSelectedGuns && selectedGuns[0].itemType == ItemDataType.None) return;
						itemData = new ItemData
						{
							itemType = ItemDataType.Secondary,
							secondaryType = SecondaryType.SubMachineGun,
							gunType = GunType.SubMachineGun,
							gameObject = weaponsDatabase.FindGameObjects("Weapons/Secondary/Sub Machine Gun")[0],
							isWeaponAutomatic = true
						};
						selectedGuns[1] = itemData;
						InventoryUI.Instance.DisplayImage();
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
