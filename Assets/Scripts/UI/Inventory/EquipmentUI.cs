using UnityEngine;
using UnityEngine.UI;
using static SlotData;

public class EquipmentUI : MonoBehaviour
{
	[Header("Slots")]
	public GameObject helmet;
	public Button helemetBtn;

	public GameObject chestplate;
	public Button chestplateBtn;

	public GameObject leggings;
	public Button leggingsBtn;

	public GameObject boots;
	public Button bootsBtn;

	[Header("Booleans")]
	public bool isEquipmentOpen;
	public bool isHelmetOpen;
	public bool isChestplateOpen;
	public bool isLeggingsOpen;
	public bool isBootsOpen;

	[Header("Equipment")]
	public GameObject equipmentPrefab;
	public GameObject instantiatedEquipment;
	public GameObject equipmentParent;
	private void Start()
	{
		helemetBtn.onClick.AddListener(() => ToggleEquipment(ArmorType.Helmet));
		chestplateBtn.onClick.AddListener(() => ToggleEquipment(ArmorType.Chestplate));
		leggingsBtn.onClick.AddListener(() => ToggleEquipment(ArmorType.Leggings));
		bootsBtn.onClick.AddListener(() => ToggleEquipment(ArmorType.Boots));
	}
	private void ToggleEquipment(ArmorType armorType)
	{
		switch (armorType)
		{
			case ArmorType.Helmet:
				HelmetCheck();
				break;
			case ArmorType.Chestplate:
				ChestplateCheck();
				break;
			case ArmorType.Leggings:
				LeggingsCheck();
				break;
			case ArmorType.Boots:
				BootsCheck();
				break;
			default:
				Debug.LogError("Null value for ArmorType.");
				break;
		}
	}
	private void HelmetCheck()
	{
		if (instantiatedEquipment == null)
		{
			isHelmetOpen = true;
			instantiatedEquipment = Instantiate(equipmentPrefab, equipmentParent.transform);
			instantiatedEquipment.transform.localPosition = new(-115, 40, 0);
			isEquipmentOpen = true;
		}
		else if (isChestplateOpen || isLeggingsOpen || isBootsOpen)
		{
			isHelmetOpen = true;
			Destroy(instantiatedEquipment);
			instantiatedEquipment = Instantiate(equipmentPrefab, equipmentParent.transform);
			instantiatedEquipment.transform.localPosition = new(-115, 40, 0);
			isChestplateOpen = false; isLeggingsOpen = false; isBootsOpen = false;
		}
		else
		{
			isHelmetOpen = false;
			Destroy(instantiatedEquipment);
		}
	}
	private void ChestplateCheck()
	{
		if (instantiatedEquipment == null)
		{
			isChestplateOpen = true;
			instantiatedEquipment = Instantiate(equipmentPrefab, equipmentParent.transform);
			instantiatedEquipment.transform.localPosition = new(105, 40, 0);
			isEquipmentOpen = true;
		}
		else if (isHelmetOpen || isLeggingsOpen || isBootsOpen)
		{
			isChestplateOpen = true;
			Destroy(instantiatedEquipment);
			instantiatedEquipment = Instantiate(equipmentPrefab, equipmentParent.transform);
			instantiatedEquipment.transform.localPosition = new(105, 40, 0);
			isHelmetOpen = false; isLeggingsOpen = false; isBootsOpen = false;
		}
		else
		{
			isChestplateOpen = false;
			Destroy(instantiatedEquipment);
		}
	}
	private void LeggingsCheck()
	{
		if (instantiatedEquipment == null)
		{
			isLeggingsOpen = true;
			instantiatedEquipment = Instantiate(equipmentPrefab, equipmentParent.transform);
			instantiatedEquipment.transform.localPosition = new(-115, -70, 0);
			isEquipmentOpen = true;
		}
		else if (isHelmetOpen || isChestplateOpen || isBootsOpen)
		{
			isLeggingsOpen = true;
			Destroy(instantiatedEquipment);
			instantiatedEquipment = Instantiate(equipmentPrefab, equipmentParent.transform);
			instantiatedEquipment.transform.localPosition = new(-115, -70, 0);
			isHelmetOpen = false; isChestplateOpen = false; isBootsOpen = false;
		}
		else
		{
			isLeggingsOpen = false;
			Destroy(instantiatedEquipment);
		}
	}
	private void BootsCheck()
	{
		if (instantiatedEquipment == null)
		{
			isBootsOpen = true;
			instantiatedEquipment = Instantiate(equipmentPrefab, equipmentParent.transform);
			instantiatedEquipment.transform.localPosition = new(105, -70, 0);
			isEquipmentOpen = true;
		}
		else if (isHelmetOpen || isChestplateOpen || isLeggingsOpen)
		{
			isBootsOpen = true;
			Destroy(instantiatedEquipment);
			instantiatedEquipment = Instantiate(equipmentPrefab, equipmentParent.transform);
			instantiatedEquipment.transform.localPosition = new(105, -70, 0);
			isHelmetOpen = false; isLeggingsOpen = false; isLeggingsOpen = false;
		}
		else
		{
			isBootsOpen = false;
			Destroy(instantiatedEquipment);
		}
	}
}
