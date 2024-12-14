using UnityEngine;

public class GunData : MonoBehaviour
{
	public GunStats gunStats;

	public void AssignGunStats(GunStats newGunStats)
	{
		gunStats = newGunStats;
	}
}
[System.Serializable]
public class GunStats
{
	// Negative values (-1) means that stat does not apply to the weapon
	public GameObject gameObject;
	public int cost;
	public int level;
	public int levelCap; // Inclusive
	public int damage;
	public float firerate;
	public int range;
	public int ammoCapacity;
	public float reloadDuration;
	public int accuracy; // Higher is better. 100% accuracy means always perfect shots. (0-100)
	public float bulletSpeed;
}