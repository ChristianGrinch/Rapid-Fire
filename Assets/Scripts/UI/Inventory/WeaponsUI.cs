using UnityEngine;
using UnityEngine.UI;

public class WeaponsUI : MonoBehaviour
{
	[Header("Slots")]
	public GameObject primary;
	public Button primaryBtn;
	public GameObject secondary;
	public Button secondaryBtn;
	[Header("Other")]
	public GameObject weapons;
	public GameObject instantiatedWeapons;
	public GameObject weaponsParent;
	public bool isPrimaryOpen;
	public bool isSecondaryOpen;
	private void Start()
	{
		primaryBtn.onClick.AddListener(TogglePrimary);
		secondaryBtn.onClick.AddListener(ToggleSecondary);
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
		if(instantiatedWeapons != null)
		{
			CloseSecondary();
		}
		isPrimaryOpen = true;
		instantiatedWeapons = Instantiate(weapons, weaponsParent.transform);
		instantiatedWeapons.transform.localPosition = new(-155, 50, 0);
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
		instantiatedWeapons = Instantiate(weapons, weaponsParent.transform);
		instantiatedWeapons.transform.localPosition = new(-155, -75, 0);
	}
	private void CloseSecondary()
	{
		isSecondaryOpen = false;
		Destroy(instantiatedWeapons);
	}
}
