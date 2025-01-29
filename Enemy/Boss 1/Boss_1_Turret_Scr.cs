using DG.Tweening;
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
    [HideInInspector] public bool isShooting = false;

    private void Start()
    {
        Initialize();
    }
    void Update()
    {
        if (!isShooting)
            return;
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
        if (curHealth <= 0)
            return;

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
        isShooting = false;

        float jumpTime = 1f;
        transform.parent = null;
        Vector3 jumpTarget = transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-10f, -8f));
        jumpTarget.y = -17f;
        transform.DOJump(jumpTarget, 20f, 1, jumpTime).SetEase(Ease.InQuad).onComplete = () => { 
            DebriesMaker_Scr.instance.ExplodeOnPos(transform.position);
            Destroy(gameObject); };
        transform.DORotate(new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)), jumpTime - 0.03f);
    }



    private void Initialize()
    {
        curHealth = maxHealth;
        nosel = transform.GetChild(2);
    }

}
