using System.Collections.Generic;
using UnityEngine;

public class BackgroundElement_Scr : MonoBehaviour
{
    Bounds bounds;
    private void Start()
    {
        BackgroundManager_Scr.AddToSpawnedElements(gameObject);
        CalculateBounds();
        Destroy(gameObject, 10f);
    }
    private void Update()
    {
        transform.position += BackgroundManager_Scr.instance.backgroundSpeed * Time.deltaTime * new Vector3(0, 0, -1);
        transform.GizmoPointer(bounds.max + transform.position);
        transform.GizmoPointer(bounds.min + transform.position);
    }
    private void OnDestroy()
    {
        BackgroundManager_Scr.RemoveSpawnedElements(gameObject);
    }

    private void CalculateBounds()
    {
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }
        Vector3 localCenter = bounds.center - transform.position;
        bounds.center = localCenter;
        transform.rotation = currentRotation;
        Debug.Log(bounds.size);
    }
}
