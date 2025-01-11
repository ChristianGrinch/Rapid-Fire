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

	[Header("Selected Guns Data")] 
	public GunStats primaryData;
	public GunStats secondaryData;
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
		switch (newItemData.itemType)
		{
			case ItemDataType.None:
				break;
			case ItemDataType.Primary:
				selectedGuns[0].itemType = ItemDataType.Primary;
				
				switch (newItemData.primaryType)
				{
					case PrimaryType.AssaultRifle:
						if (loadingSelectedGuns && selectedGuns[0].itemType == ItemDataType.None) return;
						// used for when loading the game for the first time and the data for the slot is saved, but the gameobject is not due to msgpack not being able to serialize gameobjects
						if (!newItemData.gameObject) newItemData.gameObject = weaponsDatabase.FindGameObjects("Weapons/Primary/Assault Rifle")[0];

						selectedGuns[0] = newItemData;
						// must also set data for the visible slots because the primary/secondary data is needed to display items accordingly
						WeaponsUI.Instance.primaryData.itemData = newItemData;
						InventoryUI.Instance.DisplayImage();
						
						primaryData = selectedGuns[0].gameObject.GetComponent<GunData>().gunStats;
						break;
				}
				break;
			case ItemDataType.Secondary:
				selectedGuns[1].itemType = ItemDataType.Secondary;
				
				switch (newItemData.secondaryType)
				{
					case SecondaryType.Pistol:
						if (loadingSelectedGuns && selectedGuns[0].itemType == ItemDataType.None) return;
						if (!newItemData.gameObject) newItemData.gameObject = weaponsDatabase.FindGameObjects("Weapons/Secondary/Pistol")[0];

						selectedGuns[1] = newItemData;
						WeaponsUI.Instance.secondaryData.itemData = newItemData;
						InventoryUI.Instance.DisplayImage();
						
						secondaryData = selectedGuns[1].gameObject.GetComponent<GunData>().gunStats;
						break;
					case SecondaryType.SubMachineGun:
						if (loadingSelectedGuns && selectedGuns[0].itemType == ItemDataType.None) return;
						if (!newItemData.gameObject) newItemData.gameObject = weaponsDatabase.FindGameObjects("Weapons/Secondary/Sub Machine Gun")[0];

						selectedGuns[1] = newItemData;
						WeaponsUI.Instance.secondaryData.itemData = newItemData;
						InventoryUI.Instance.DisplayImage();
						
						secondaryData = selectedGuns[1].gameObject.GetComponent<GunData>().gunStats;
						break;
				}
				break;
			case ItemDataType.Powerup:
				PowerupsUI powerupsUI = PowerupsUI.Instance;

				switch (newItemData.powerupType)
				{
					case PowerupType.Ammo:
						powerupsUI.ammoCount.text = powerupsUI.ammoSlotData.powerupCounts[2].ToString();
						break;
					case PowerupType.Health:
						powerupsUI.healthCount.text = powerupsUI.healthSlotData.powerupCounts[0].ToString();
						break;
					case PowerupType.Speed:
						Debug.Log("ran speed setslotdata");
						powerupsUI.speedSlotData.powerupCounts[1] = PlayerController.Instance.speedPowerupCount;
						powerupsUI.speedCount.text = powerupsUI.speedSlotData.powerupCounts[1].ToString();
						InventoryUI.Instance.DisplayImage();
						break;
				}
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
}
