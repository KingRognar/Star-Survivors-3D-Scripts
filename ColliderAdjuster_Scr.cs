using UnityEngine;

public class ColliderAdjuster_Scr : MonoBehaviour
{
    void Update()
    {
        AdjustCollider();
    }

    private void AdjustCollider()
    {
        //TODO: add colliderScaler

        Vector3 newColliderPos = transform.position;
        newColliderPos = Camera.main.WorldToScreenPoint(newColliderPos);
        newColliderPos.z = Camera.main.transform.position.y;
        newColliderPos = Camera.main.ScreenToWorldPoint(newColliderPos);
        newColliderPos = newColliderPos - transform.position;

        GetComponent<SphereCollider>().center = newColliderPos;
    }
}
