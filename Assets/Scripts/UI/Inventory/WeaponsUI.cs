using UnityEngine;
using UnityEngine.UI;

public class WeaponsUI : MonoBehaviour
{
	public static WeaponsUI Instance { get; private set; }
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
	[Header("Slots")]
	public GameObject primary;
	public Button primaryBtn;

	public GameObject secondary;
	public Button secondaryBtn;
	[Header("Slot data")]
	public SlotData primaryData;
	public SlotData secondaryData;
	[Header("Other")]
	public GameObject weaponsPrefab;
	public GameObject instantiatedWeapons;
	public GameObject weaponsParent;
	public bool isPrimaryOpen;
	public bool isSecondaryOpen;
	private void Start()
	{
		primaryData = primary.GetComponent<SlotData>();
		secondaryData = secondary.GetComponent<SlotData>();

		primaryBtn.onClick.AddListener(TogglePrimary);
		secondaryBtn.onClick.AddListener(ToggleSecondary);
		primaryData.itemData = InventoryManager.Instance.selectedGuns[0];
		secondaryData.itemData = InventoryManager.Instance.selectedGuns[1];
	}
	private void TogglePrimary()
	{
		if (isPrimaryOpen)
		{
			ClosePrimary();
		}
		else
		{
			OpenPrimary();
		}
	}
	private void ToggleSecondary()
	{
		if (isSecondaryOpen)
		{
			CloseSecondary();
		}
		else
		{
			OpenSecondary();
		}
	}
	private void OpenPrimary()
	{
		if (instantiatedWeapons != null)
		{
			CloseSecondary();
		}
		isPrimaryOpen = true;
		instantiatedWeapons = Instantiate(weaponsPrefab, weaponsParent.transform);
		instantiatedWeapons.transform.localPosition = new(-155, 50, 0);
		instantiatedWeapons.name = "Primary Container";
		InventoryManager.Instance.FindStorageContainer(instantiatedWeapons.name, weaponsParent);
	}
	public void CloseContainers()
	{
		ClosePrimary();
		CloseSecondary();
	}
	private void ClosePrimary()
	{
		isPrimaryOpen = false;
		Destroy(instantiatedWeapons);
	}
	private void OpenSecondary()
	{
		if (instantiatedWeapons != null)
		{
			ClosePrimary();
		}
		isSecondaryOpen = true;
		instantiatedWeapons = Instantiate(weaponsPrefab, weaponsParent.transform);
		instantiatedWeapons.transform.localPosition = new(-155, -75, 0);
		instantiatedWeapons.name = "Secondary Container";
		InventoryManager.Instance.FindStorageContainer(instantiatedWeapons.name, weaponsParent);
	}
	private void CloseSecondary()
	{
		isSecondaryOpen = false;
		Destroy(instantiatedWeapons);
	}
}
