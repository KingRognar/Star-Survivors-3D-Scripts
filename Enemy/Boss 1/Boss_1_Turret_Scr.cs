using UnityEngine;

public class Boss_1_Turret_Scr : MonoBehaviour, IDamageable
{
    private float curHealth; 
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject smokeVfxObj;
    [SerializeField] private int turretID;
    [SerializeField] private float shotInterval; private float nextShotTime = 2f; 
    private float nextBurstTime = 2f; private float timeBtwenBursts = 1f; private int bulletsInBurst = 8; private int currentBurstCount = 0;
    [SerializeField] private GameObject bulletPrefab;
    private Transform nosel;

    private void Start()
    {
        Initialize();
    }
    void Update()
    {
        PlayerTracking();
        Attack();
    }

    private void PlayerTracking()
    {
        Vector3 adjustedPlayerPos = Player_Stats_Scr.instance.transform.position - transform.position; //TODO: handle destroyed player
        adjustedPlayerPos.y = 0;
        adjustedPlayerPos = Vector3.Cross(Vector3.down, adjustedPlayerPos);
        transform.rotation = Quaternion.LookRotation(adjustedPlayerPos, Vector3.up);
    }
    private void Attack()
    {
        if (nextBurstTime > Time.time)
            return;

        if (nextShotTime > Time.time)
            return;

        nextShotTime = Time.time + shotInterval;
        currentBurstCount++;
        Instantiate(bulletPrefab, nosel.position, nosel.rotation);

        if (currentBurstCount == bulletsInBurst)
        {
            nextBurstTime = Time.time + timeBtwenBursts;
            nextShotTime = Time.time + timeBtwenBursts;
            currentBurstCount = 0;
        }
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
        transform.parent.parent.parent.GetComponent<Boss_1_Scr>().TurretIsDestriyed(turretID);
        Destroy(gameObject);
    }



    private void Initialize()
    {
        curHealth = maxHealth;
        nosel = transform.GetChild(2);
    }

}
