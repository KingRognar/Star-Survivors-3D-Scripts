using UnityEngine;

public class Boss_1_Spread_Scr : MonoBehaviour, IDamageable
{
    private float curHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject smokeVfxObj;
    [SerializeField] private int turretID;
    [SerializeField] private float shotInterval; private float nextShotTime = 2f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform[] nosels;
    [HideInInspector] public bool isShooting = false;

    private void Start()
    {
        Initialize();
    }
    void Update()
    {
        RotateTurret();
        Attack();
    }

    private void Attack()
    {
        if (nextShotTime > Time.time)
            return;
        nextShotTime += shotInterval;
        if (!isShooting)
            return;

        foreach (Transform nosel in nosels)
        {
            Instantiate(bulletPrefab, nosel.position, nosel.rotation);
        }
    }
    private void RotateTurret()
    {
        transform.Rotate(transform.up, 180 * Time.deltaTime, Space.Self);
    }
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        curHealth -= damage;

        GetComponentInChildren<Enemy_Flash_Scr>().StartFlash();
        Collider collider = GetComponent<Collider>();
        Vector3 collisionPoint = collider.ClosestPoint(dmgTakenFromPos);
        DebriesMaker_Scr.instance.HitFromPosAndDir(collisionPoint, dmgTakenFromPos - collisionPoint);

        if (curHealth <= 0)
            Explode();
    }
    private void Explode()
    {
        DebriesMaker_Scr.instance.ExplodeOnPos(transform.position);
        smokeVfxObj.SetActive(true);
        transform.parent.parent.parent.GetComponent<Boss_1_Scr>().TurretIsDestroyed(turretID);
        Destroy(gameObject);
    }

    private void Initialize()
    {
        curHealth = maxHealth;
        SetupNosels();
    }
    private void SetupNosels()
    {
        for (int i = 1; i < 6; i++)
        {
            nosels[i].SetPositionAndRotation(nosels[0].position, nosels[0].rotation);
            nosels[i].RotateAround(transform.position, Vector3.up, 60 * i);
        }
    }
}
