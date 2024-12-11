using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsDatabase", menuName = "Database/Weapons")]
public class WeaponsDatabase : ScriptableObject
{
	[Header("Primary")]
	public List<GameObject> assaultRifles;
	[Header("Secondary")]
	public List<GameObject> pistols;
	public List<GameObject> subMachineGuns;
	public List<GameObject> FindGameObjects(string path)
	{
		//Weapons/Secondary/Pistol
		GameObject[] gameObjects = Resources.LoadAll<GameObject>(path);

		return new List<GameObject>(gameObjects);
	}
}
