using UnityEngine;

public class ScreenBorder_Scr : MonoBehaviour
{
    BoxCollider rightCollider, leftCollider, topCollider, bottomCollider;

    private void Start()
    {
        GetChildColliders();
        SetupColliders();
    }

    private void GetChildColliders()
    {
        rightCollider = transform.GetChild(0).GetComponent<BoxCollider>();
        leftCollider = transform.GetChild(1).GetComponent<BoxCollider>();
        topCollider = transform.GetChild(2).GetComponent<BoxCollider>();
        bottomCollider = transform.GetChild(3).GetComponent<BoxCollider>();
    }
    private void SetupColliders()
    {
        Vector3 bottomLeftCorner = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.y));
        Vector3 upperRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.transform.position.y));
        Vector3 centerPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, Camera.main.transform.position.y));

        rightCollider.transform.position = new Vector3(upperRightCorner.x + 0.5f, 0, 0);
        leftCollider.transform.position = new Vector3(bottomLeftCorner.x - 0.5f, 0, 0);
        topCollider.transform.position = new Vector3(0, 0, upperRightCorner.z + 0.5f);
        bottomCollider.transform.position = new Vector3(0, 0, bottomLeftCorner.z - 0.5f);

        float screenWidthInWorld = upperRightCorner.x - bottomLeftCorner.x;
        float screenHeightInWorld = upperRightCorner.z - bottomLeftCorner.z;

        rightCollider.size = new Vector3(1, 1, screenHeightInWorld);
        leftCollider.size = new Vector3(1, 1, screenHeightInWorld);
        topCollider.size = new Vector3(screenWidthInWorld, 1, 1);
        bottomCollider.size = new Vector3(screenWidthInWorld, 1, 1);
    }
}
