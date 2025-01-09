using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
	[Header("Buy Panel")]
	public GameObject buyPanelPrefab;
	public Image buyPanelImage;
	public TMP_Text buyPanelText;
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

	[SerializeField] private WeaponsDatabase weaponsDatabase;
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
			OpenSection(ButtonType.Powerup);
		});
		upgradeBtn.onClick.AddListener(() => {
			//OpenSection(ButtonType.Upgrade);
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
				List<GameObject> level1Guns = weaponsDatabase.FindAllGameObjectsByLevel(1);
				
				for(var i = 0; i < level1Guns.Count; i++)
				{
					InstantiateButton(level1Guns[i]);
				}
				break;
			case ButtonType.Powerup:
				InstantiateButton(CreateItemObject(ItemDataType.Powerup, PowerupType.Ammo));
				
				InstantiateButton(CreateItemObject(ItemDataType.Powerup, PowerupType.Health));
				
				InstantiateButton(CreateItemObject(ItemDataType.Powerup, PowerupType.Speed));
				break;
			case ButtonType.Upgrade:
				break;
		}
	}
	private GameObject CreateItemObject(ItemDataType itemType, PowerupType powerupType)
	{
		GameObject itemObject = new GameObject("Shop Powerup Object");
		
		SlotData slotData = itemObject.AddComponent<SlotData>();
		
		slotData.itemData = new ItemData
		{
			itemType = itemType,
			powerupType = powerupType
		};

		return itemObject;
	}
	public void OpenShop()
	{
		UIManager.Instance.OpenInterface(InterfaceElements.Shop);
		exp.text = $"EXP: {PlayerController.Instance.exp}";
		GameManager.Instance.PauseGame();
		EmptyContent();
	}
	public void BuyItem(int cost, GameObject itemToBuy)
	{
		int playerExp = PlayerController.Instance.exp;
		if (playerExp - cost < 0) return;
		PlayerController.Instance.exp -= cost;
		
		ItemData itemData = itemToBuy.GetComponent<SlotData>().itemData;
		if (itemData.itemType == ItemDataType.Primary)
		{
			if (InventoryManager.Instance.ownedPrimaries[0].primaryType == PrimaryType.None)
				InventoryManager.Instance.ownedPrimaries.Clear();
			InventoryManager.Instance.ownedPrimaries.Add(itemData);
		}
		else
		{
			InventoryManager.Instance.ownedSecondaries.Add(itemData);
		}
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
	public void InstantiateButton(GameObject obj)
	{
		ItemData itemData = obj.GetComponent<SlotData>().itemData;
		GunStats gunStats = obj.GetComponent<GunData>().gunStats;
		GameObject buttonToInstantiate = null;
		switch (itemData.itemType)
		{
			case ItemDataType.Primary:
				buttonToInstantiate = gunPrefab;

				string text = itemData.gunType.ToString();
				text = Regex.Replace(text, "(?<!^)([A-Z])", " $1"); // no idea what this means but thanks stack overflow. all i know is (?<!^) avoids the first letter 
				buttonToInstantiate.GetComponentInChildren<TMP_Text>().text = text;
				break;
			case ItemDataType.Secondary:
				buttonToInstantiate = gunPrefab;

				text = itemData.gunType.ToString();
				text = Regex.Replace(text, "(?<!^)([A-Z])", " $1");
				buttonToInstantiate.GetComponentInChildren<TMP_Text>().text = text;
				break;
			case ItemDataType.Powerup:
				buttonToInstantiate = powerupPrefab;
				buttonToInstantiate.GetComponentInChildren<TMP_Text>().text = itemData.powerupType.ToString();
				break;
		}
		prefabObject = Instantiate(buttonToInstantiate, content.transform);
		prefabObject.GetComponent<Button>().onClick.AddListener(() =>
		{
			if (itemData.itemType == ItemDataType.Primary)
			{
				foreach (var primary in InventoryManager.Instance.ownedPrimaries)
				{	
					if (itemData.primaryType == primary.primaryType)
					{
						// Item is owned, no need to open the buy panel
						return;
					}
				}
				// Item is not found in ownedPrimaries, open the buy panel
				OpenBuyPanel(obj);
			}
			else
			{
				foreach (var secondary in InventoryManager.Instance.ownedSecondaries)
				{
					if (itemData.secondaryType == secondary.secondaryType)
					{
						return;
					};
				}
				OpenBuyPanel(obj);
			}
		});
	}

	private void OpenBuyPanel(GameObject obj)
	{
		ItemData itemData = obj.GetComponent<SlotData>().itemData;
		GunStats gunStats = obj.GetComponent<GunData>().gunStats;
		
		GameObject buyPanel = Instantiate(buyPanelPrefab, content.transform.parent);
		GameObject panel = buyPanel.transform.GetChild(0).gameObject;
		Button buyBtn = panel.transform.Find("Buy").gameObject.GetComponent<Button>();
		Button cancelBtn = panel.transform.Find("Cancel").gameObject.GetComponent<Button>();
		
		buyPanelImage = panel.GetComponentInChildren<Image>();
		buyPanelText = panel.transform.Find("Text (TMP)").GetComponent<TMP_Text>();

		string gun = itemData.primaryType != PrimaryType.None ? itemData.primaryType.ToString() : itemData.secondaryType.ToString();
		gun = Regex.Replace(gun, "(?<!^)([A-Z])", " $1");
		int price = gunStats.cost;
		buyPanelText.text = $" Would you like to buy {gun} for {price}?";

		buyBtn.onClick.AddListener(() =>
		{
			BuyItem(price, obj);
			Destroy(buyPanel);
		}); 
		cancelBtn.onClick.AddListener(() => Destroy(buyPanel));
	}
}
