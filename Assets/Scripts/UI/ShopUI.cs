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
				List<SlotData> itemDatas = new();
				List<GunData> gunDatas = new();

				for(var i = 0; i < level1Guns.Count; i++)
				{
					itemDatas.Add(level1Guns[i].GetComponent<SlotData>());
					gunDatas.Add(level1Guns[i].GetComponent<GunData>());
				}

				for(var i = 0; i < level1Guns.Count; i++)
				{
					InstantiateButton(itemDatas[i].itemData, gunDatas[i].gunStats);
				}
				break;
			case ButtonType.Powerup:
				InstantiateButton(new ItemData()
				{
					itemType = ItemDataType.Powerup,
					powerupType = PowerupType.Ammo
				}, new GunStats());
				InstantiateButton(new ItemData()
				{
					itemType = ItemDataType.Powerup,
					powerupType = PowerupType.Health
				}, new GunStats());
				InstantiateButton(new ItemData()
				{
					itemType = ItemDataType.Powerup,
					powerupType = PowerupType.Speed
				}, new GunStats());
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
	public void InstantiateButton(ItemData itemData, GunStats gunStats)
	{
		GameObject obj = null;
		switch (itemData.itemType)
		{
			case ItemDataType.Primary:
				obj = gunPrefab;

				string text = itemData.gunType.ToString();
				text = Regex.Replace(text, "(?<!^)([A-Z])", " $1"); // no idea what this means but thanks stack overflow. all i know is (?<!^) avoids the first letter 
				obj.GetComponentInChildren<TMP_Text>().text = text;
				break;
			case ItemDataType.Secondary:
				obj = gunPrefab;

				text = itemData.gunType.ToString();
				text = Regex.Replace(text, "(?<!^)([A-Z])", " $1");
				obj.GetComponentInChildren<TMP_Text>().text = text;
				break;
			case ItemDataType.Powerup:
				obj = powerupPrefab;
				obj.GetComponentInChildren<TMP_Text>().text = itemData.powerupType.ToString();
				break;
			//case ButtonType.Upgrade:
			//	gameObject = upgradePrefab;
			//	break;
		}
		prefabObject = Instantiate(obj, content.transform);
		prefabObject.GetComponent<Button>().onClick.AddListener(() => OpenBuyPanel(itemData, gunStats)); // the passed "obj" is NOT the gun gameobject so OpenBuyPanel breaks.
	}

	private void OpenBuyPanel(ItemData itemData, GunStats gunStats)
	{
		GameObject buyPanel = Instantiate(buyPanelPrefab, content.transform.parent);
		GameObject panel = buyPanel.transform.GetChild(0).gameObject;
		Button buyBtn = panel.transform.Find("Buy").gameObject.GetComponent<Button>();
		Button cancelBtn = panel.transform.Find("Cancel").gameObject.GetComponent<Button>();
		
		buyPanelImage = panel.GetComponentInChildren<Image>();
		buyPanelText = panel.transform.Find("Text (TMP)").GetComponent<TMP_Text>();
		
		buyBtn.onClick.AddListener(BuyItem);
		cancelBtn.onClick.AddListener(() => Destroy(buyPanel));

		string gun = itemData.primaryType.ToString();
		gun = Regex.Replace(gun, "(?<!^)([A-Z])", " $1");
		string price = gunStats.cost.ToString();
		buyPanelText.text = $" Would you like to buy {gun} for {price}?";
	}
}
