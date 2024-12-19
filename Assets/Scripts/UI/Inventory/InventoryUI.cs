using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SlotData;

public class InventoryUI : MonoBehaviour
{
	public static InventoryUI Instance { get; private set; }
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
	[Header("Menu")]
	public GameObject inventoryMenu;

	[Header("Slots")]
	public List<GameObject> slots;
	[Header("Render Textures")]
	public RenderTexture pistolRT;
	public RenderTexture assaultRifleRT;
	public RenderTexture subMachineGunRT;

	public RenderTexture speedPowerupRT;

	[Header("Other")]
	public bool isInventoryOpen;
	public GameObject invPlayerCamera;

	private void Update()
	{
		//DisplayImage();

		if (Input.GetKeyDown(KeyCode.E))
		{
			switch (isInventoryOpen)
			{
				case false:
					OpenInventory();
					break;
				case true:
					CloseInventory();
					break;
			}
		}
		invPlayerCamera.transform.position = new(PlayerController.Instance.transform.position.x - 2.5f, PlayerController.Instance.transform.position.y, PlayerController.Instance.transform.position.z + 2.5f);
	}
	public void OpenInventory()
	{
		isInventoryOpen = true;
		UIManager.Instance.SetMenuStatus(Menus.Inventory, true);
		PowerupsUI.Instance.UpdateCounts();
	}
	public void CloseInventory()
	{
		UIManager.Instance.SetMenuStatus(Menus.Inventory, false);
		WeaponsUI.Instance.CloseContainers();
		Destroy(EquipmentUI.Instance.instantiatedEquipment);
		isInventoryOpen = false;
	}
	public void DisplayImage()
	{
		foreach (var slot in slots)
		{
			ItemData itemData = slot.GetComponent<SlotData>().itemData;
			switch (itemData.itemType)
			{
				case ItemDataType.Primary:
					switch (itemData.primaryType)
					{
						case PrimaryType.AssaultRifle:
							RawImage rawImage = slot.GetComponentInChildren<RawImage>();
							rawImage.texture = assaultRifleRT;

							Color color = rawImage.color;
							color.a = 1f;
							rawImage.color = color;
							break;
					}
					break;
				case ItemDataType.Secondary:
					switch (itemData.secondaryType)
					{
						case SecondaryType.Pistol:
							RawImage rawImage = slot.GetComponentInChildren<RawImage>();
							rawImage.texture = pistolRT;

							Color color = rawImage.color;
							color.a = 1f;
							rawImage.color = color;
							break;
						case SecondaryType.SubMachineGun:
							rawImage = slot.GetComponentInChildren<RawImage>();
							rawImage.texture = subMachineGunRT;

							color = rawImage.color;
							color.a = 1f;
							rawImage.color = color;
							break;
					}
					break;
				case ItemDataType.Powerup:
					switch (itemData.powerupType)
					{
						case PowerupType.Speed:
							RawImage rawImage = slot.GetComponentInChildren<RawImage>();
							rawImage.texture = speedPowerupRT;

							Color color = rawImage.color;
							color.a = 1f;
							rawImage.color = color;
							break;
					}
					break;
				case ItemDataType.Armor:
					break;
				case ItemDataType.None:
					break;
				default:
					break;
			}
		}
	}
}
