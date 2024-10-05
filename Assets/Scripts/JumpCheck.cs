using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    public bool canJump;
    public static JumpCheck Instance { get; private set; }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) { canJump = true; }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) { canJump = false; }
    }
}
