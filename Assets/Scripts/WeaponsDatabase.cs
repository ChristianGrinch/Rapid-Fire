using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
	public GameObject FindGameObjectByLevel(int level, string path)
	{
		List<GameObject> gameObjects = FindGameObjects(path);

		foreach(GameObject item in gameObjects)
		{
			string[] splitName = item.name.Split(" ");
			string lastPart = splitName[splitName.Length - 1];

			if(lastPart == level.ToString())
			{
				return item;
			}
			else
			{
				Debug.LogError("Couldn't find an item by level!");
				return null;
			}
		}
		Debug.LogError("Unknown error occured (Location: FindGameObjectByLevel in WeaponsDatabase)");
		return null;
	}
	public List<GameObject> FindAllGameObjects()
	{
		GameObject[] gameObjects = Resources.LoadAll<GameObject>("/Weapons");
		return new List<GameObject>(gameObjects);
	}
	public List<GameObject> FindAllGameObjectsByLevel(int level)
	{
		List<GameObject> allGameObjects = FindAllGameObjects();
		List<GameObject> selectedGameObjects = new();

		foreach(var gameObject in allGameObjects)
		{
			string[] splitName = gameObject.name.Split(" ");
			string lastPart = splitName[splitName.Length - 1];

			if (lastPart == level.ToString())
			{
				selectedGameObjects.Add(gameObject);
			}
		}

		return selectedGameObjects;
	}
}
