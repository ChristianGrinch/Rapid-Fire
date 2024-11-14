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
	public bool isShopOpen;
	public TMP_Text exp;
	public List<GunType> ownedGuns;
	public List<PowerupType> ownedPowerups;
	public enum ButtonType
	{
		Gun,
		Powerup,
		Upgrade
	}
	private void Start()
	{
		EmptyContent();
		gunBtn.onClick.AddListener(EmptyContent);
		powerupBtn.onClick.AddListener(EmptyContent);
		upgradeBtn.onClick.AddListener(EmptyContent);
	}
	public void OpenShop()
	{
		exp.text = $"EXP: {PlayerController.Instance.exp}";
		isShopOpen = true;
		GameManager.Instance.PauseGame();
		shopMenu.SetActive(true);
		EmptyContent();
	}
	public void BuyItem()
	{
		exp.text = $"EXP: {PlayerController.Instance.exp}";
	}
	public void CloseShop()
	{
		isShopOpen = false;
		shopMenu.SetActive(false);
		GameManager.Instance.ResumeGame();
	}
	public void EmptyContent()
	{
		for(var i = 0; i < content.transform.childCount; i++)
		{
			Destroy(content.transform.GetChild(i).gameObject);
		}
	}
	public void InstantiateButton(ButtonType buttonType)
	{
		GameObject gameObject = new();
		switch (buttonType)
		{
			case ButtonType.Gun:
				gameObject = gunPrefab;
				break;
			case ButtonType.Powerup:
				gameObject = powerupPrefab;
				break;
			case ButtonType.Upgrade:
				gameObject = upgradePrefab;
				break;
		}
		prefabObject = Instantiate(gameObject, content.transform);
	}
}
