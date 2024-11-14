using System;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;
public class InventoryManager : MonoBehaviour
{
	
}
public class InventoryData
{
	static int gunTypesLength = Enum.GetValues(typeof(GunType)).Length;

	List<bool> ownedGuns = new(gunTypesLength);
	List<List<int>> gunUpgrades = new(gunTypesLength);
	List<int> slotData; // 0 represents a gun. 1 represents a powerup. (add more as needed)
	public void SlotData()
	{

	}
}
