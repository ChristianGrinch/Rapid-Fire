using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 playerPos;
    private Vector3 offset = new Vector3(0, 10, 0);

    private int lowerClampY = 10;
    private int upperClampY = 20;

    private float zoomSpeed = 5f;
    private float targetZoom;

    // Start is called before the first frame update
    void Start()
    {
        targetZoom = offset.y;
		StartCoroutine(AssignPlayer());
    }
	private IEnumerator AssignPlayer()
	{
		yield return null;
		player = GameManager.Instance.player;
	}

	void Update()
    {
        HandleInput();
        UpdateCameraPosition();
    }

    void HandleInput()
    {
        float mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll != 0)
        {
            targetZoom -= mouseScroll;
        }
        targetZoom = Mathf.Clamp(targetZoom, lowerClampY, upperClampY);
        offset.y = Mathf.Lerp(offset.y, targetZoom, Time.deltaTime * zoomSpeed);
    }
    void UpdateCameraPosition()
    {
        playerPos = player.transform.position;
        transform.position = playerPos + offset;
    }
}
