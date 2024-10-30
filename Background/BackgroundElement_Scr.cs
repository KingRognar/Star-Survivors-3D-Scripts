using UnityEngine;

public class BackgroundElement_Scr : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 10f);
    }
    void Update()
    {
        transform.position += BackgroundManager_Scr.backgroundSpeed * Time.deltaTime * new Vector3(0, 0, -1);
    }
}
