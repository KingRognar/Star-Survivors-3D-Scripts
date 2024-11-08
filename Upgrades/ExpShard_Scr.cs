using UnityEngine;

public class ExpShard_Scr : MonoBehaviour
{
    private bool isInCollectorRange = false;
    [SerializeField] private float movementSpeed = 4f;
    private Transform playerTransform;
    private float t = 0;

    private void Update()
    {
        if (isInCollectorRange)
        {
            t = Mathf.MoveTowards(t, 1, Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, playerTransform.position, t);
            if (t >= 1)
                Collect();
        }
        else
        {
            transform.position += movementSpeed * Time.deltaTime * -Vector3.forward;
            if (transform.position.z <= -15)
                Disappear();
        }
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }
    private void Collect()
    {
        UpgradeSystem_Scr.instance.AwardEXP(2); //TODO: сделать вариативным
        Destroy(gameObject);
    }
    public void GetCaught()
    {
        isInCollectorRange = true;
        Collider collider = GetComponent<Collider>();
        collider.enabled = false;
        playerTransform = Player_Stats_Scr.instance.transform;
    }
}
