using UnityEngine;

public class BackgroundElement_Scr : MonoBehaviour
{
    private void Start()
    {
        BackgroundManager_Scr.AddToSpawnedElements(gameObject);
        Destroy(gameObject, 10f);
    }
    private void Update()
    {
        transform.position += BackgroundManager_Scr.instance.backgroundSpeed * Time.deltaTime * new Vector3(0, 0, -1);
    }
    private void OnDestroy()
    {
        BackgroundManager_Scr.RemoveSpawnedElements(gameObject);
    }
}
