using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SlotData;

public class ShopUI : MonoBehaviour
{
	public static ShopUI Instance { get; private set; }
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
	public GameObject shopMenu;
	[Header("Prefabs")]
	public GameObject gunPrefab;
	public GameObject powerupPrefab;
	public GameObject upgradePrefab;
	[Header("Header")]
	public GameObject header;
	public HorizontalLayoutGroup headerLayoutGroup;
	[Header("Header Buttons")]
	public Button gunBtn;
	public Button powerupBtn;
	public Button upgradeBtn;
	[Header("Content")]
	public GameObject content;
	public GridLayoutGroup contentLayoutGroup;
	[Header("Instantiated Prefab Objects")]
	private GameObject prefabObject;
	private Image prefabImage;
	private TMP_Text prefabText;
	[Header("Other")]
	public TMP_Text exp;
	public List<PowerupType> ownedPowerups;
	public int numberOfGuns;
	WeaponsDatabase weaponsDatabase;
	public enum ButtonType
	{
		Gun,
		Powerup,
		Upgrade
	}
	private void Start()
	{
		EmptyContent();
		gunBtn.onClick.AddListener(() => {
			OpenSection(ButtonType.Gun);
		});
		powerupBtn.onClick.AddListener(() => {
			//OpenSection(ButtonType.);
		});
		upgradeBtn.onClick.AddListener(() => {
			//OpenSection(ButtonType.);
		});

		numberOfGuns = Enum.GetNames(typeof(PrimaryType)).Length - 1 + Enum.GetNames(typeof(SecondaryType)).Length - 1;
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			if (UIManager.Instance.IsInterfaceOpen(InterfaceElements.Shop))
			{
				CloseShop();
			}
			else if(!UIManager.Instance.IsGamePaused())
			{
				if (UIManager.Instance.IsInterfaceOpen(InterfaceElements.Inventory)) UIManager.Instance.CloseInterface(InterfaceElements.Inventory);
				OpenShop();
			}
		}
	}
	public void OpenSection(ButtonType type)
	{
		EmptyContent();
		switch (type)
		{
			case ButtonType.Gun:
				//var i = 0;
				//i += InventoryManager.Instance.ownedPrimaries.Count + InventoryManager.Instance.ownedSecondaries.Count;
				//if (InventoryManager.Instance.ownedPrimaries[0].gunType == GunController.GunType.None) i--; // freaky way to check if the item in owned primaries is just a filler slot and has no weapon
				
				List<GameObject> level1Guns = weaponsDatabase.FindAllGameObjectsByLevel(1);
				List<GunData> gunDatas = new();
				List<ItemData> itemDatas;

				for(var i = 0; i < level1Guns.Count; i++)
				{
					gunDatas.Add(level1Guns[i].GetComponent<GunData>());
				}

				itemDatas = WeaponsDatabase.ConvertGunDataToItemData(gunDatas);

				for(var i = 0; i < level1Guns.Count; i++)
				{
					InstantiateButton(itemDatas[i]);
				}
				break;
			case ButtonType.Powerup:
				break;
			case ButtonType.Upgrade:
				break;
		}
	}
	public void OpenShop()
	{
		UIManager.Instance.OpenInterface(InterfaceElements.Shop);
		exp.text = $"EXP: {PlayerController.Instance.exp}";
		GameManager.Instance.PauseGame();
		EmptyContent();
	}
	public void BuyItem()
	{
		exp.text = $"EXP: {PlayerController.Instance.exp}";
	}
	public void CloseShop()
	{
		UIManager.Instance.CloseInterface(InterfaceElements.Shop);
		GameManager.Instance.ResumeGame();
	}
	public void EmptyContent()
	{
		for(var i = 0; i < content.transform.childCount; i++)
		{
			Destroy(content.transform.GetChild(i).gameObject);
		}
	}
	public void InstantiateButton(ItemData itemData)
	{
		GameObject gameObject = new();
		switch (itemData.itemType)
		{
			case ItemDataType.Primary:
				gameObject = gunPrefab;
				gameObject.GetComponentInChildren<TMP_Text>().text = itemData.gunType.ToString();
				break;
			case ItemDataType.Secondary:
				gameObject = powerupPrefab;
				gameObject.GetComponentInChildren<TMP_Text>().text = itemData.gunType.ToString();
				break;
			case ItemDataType.Powerup:
				gameObject = powerupPrefab;
				gameObject.GetComponentInChildren<TMP_Text>().text = itemData.powerupType.ToString();
				break;
			//case ButtonType.Upgrade:
			//	gameObject = upgradePrefab;
			//	break;
		}
		prefabObject = Instantiate(gameObject, content.transform);
	}
}
