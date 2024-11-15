using UnityEngine;

public class ExpShard_Scr : MonoBehaviour
{
    [SerializeField] private int expAmount;
    [SerializeField] private float movementSpeed = 4f;

    private bool isInCollectorRange = false;
    private Transform playerTransform;
    private float t = 0;

    private void Update()
    {
        if (isInCollectorRange)
        {
            FloatToPlayer();
        }
        else
        {
            FloatDown();
        }
    }

    private void FloatDown()
    {
        transform.position += movementSpeed * Time.deltaTime * -Vector3.forward;
        if (transform.position.z <= -15)
            Disappear();
    }
    private void FloatToPlayer()
    {
        t = Mathf.MoveTowards(t, 1, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, playerTransform.position, t);
        if (t >= 0.2f)
            Collect();
    }
    /// <summary>
    /// Метод используемый для уничтожния объекта без начисления опыта
    /// </summary>
    private void Disappear()
    {
        Destroy(gameObject);
    }
    private void Collect()
    {
        UpgradeSystem_Scr.instance.AwardEXP(expAmount);
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
