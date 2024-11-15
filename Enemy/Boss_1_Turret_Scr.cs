using UnityEngine;

public class Boss_1_Turret_Scr : MonoBehaviour, IDamageable
{
    private float curHealth; 
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject smokeVfxObj;

    private void Start()
    {
        Initialize();
    }
    void Update()
    {
        PlayerTracking();
    }

    private void PlayerTracking()
    {
        Vector3 adjustedPlayerPos = Player_Stats_Scr.instance.transform.position - transform.position; //TODO: handle destroyed player
        adjustedPlayerPos.y = 0;
        adjustedPlayerPos = Vector3.Cross(Vector3.down, adjustedPlayerPos);
        transform.rotation = Quaternion.LookRotation(adjustedPlayerPos, Vector3.up);
    }
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        curHealth -= damage;

        GetComponentInChildren<Enemy_Flash_Scr>().StartFlash();
        DebriesMaker_Scr.instance.HitFromPosAndDir(transform.position, dmgTakenFromPos - transform.position);
        //GetComponent<Enemy_HitEffect_Scr>().SpawnParticles(dmgTakenFromPos);

        if (curHealth <= 0)
            Explode();
    }
    private void Explode()
    {
        DebriesMaker_Scr.instance.ExplodeOnPos(transform.position);
        smokeVfxObj.SetActive(true); //TODO: сделать, чтобы дым двигался чуть вправо
    }

    private void Initialize()
    {
        curHealth = maxHealth;
    }

}
