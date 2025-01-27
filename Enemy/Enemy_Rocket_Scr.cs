using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

public class Enemy_Rocket_Scr : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth; private float curHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turningSpeed;
    [SerializeField] private float startupTime; private float moveT; private Vector3 startPos, startupTargetPos;
    [SerializeField] private GameObject burstVfx;

    private Transform playerTrans;

    //TODO: вылетает из отверстия по прямой (добавить микроверемя на старте, чтоб не могла поворачивать)
    //TODO: наносить игроку урон при столкновении
    //TODO: добавить lifetime, после которого ракета либо взрывается, либо не может поварачивать, мб доавить падение?
    //TODO: добавить выравнивание по высоте при старте

    private void Start()
    {
        playerTrans = Player_Stats_Scr.instance.transform;
        curHealth = maxHealth;
        DOTween.To(() => moveT, x => moveT = x, 1f, startupTime);
        startPos = transform.position;
        startupTargetPos = transform.position + movementSpeed * startupTime * transform.forward; startupTargetPos.y = 0;
    }
    void Update()
    {
        RocketMovement();
    }

    private void RocketMovement()
    {
        if (moveT != 1)
            Startup();
        else
            MoveToPlayer();

    }
    private void MoveToPlayer()
    {
        if (!burstVfx.activeInHierarchy)
        {
            burstVfx.SetActive(true);
        }

        transform.position += transform.forward * Time.deltaTime * movementSpeed;
        transform.position = new(transform.position.x, 0, transform.position.z);

        if (playerTrans == null)
            return;

        Vector3 playerPos = playerTrans.position;
        Quaternion lookAtPlayerRotation = Quaternion.LookRotation(playerPos - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtPlayerRotation, turningSpeed * Time.deltaTime);
    }
    private void Startup()
    {
        transform.position = Vector3.Lerp(startPos, startupTargetPos, moveT);

        Quaternion lookRotation = Quaternion.LookRotation(startupTargetPos - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turningSpeed * Time.deltaTime);
    }    
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        curHealth -= damage;

        GetComponent<Enemy_Flash_Scr>().StartFlash();
        Collider collider = GetComponent<Collider>();
        Vector3 collisionPoint = collider.ClosestPoint(dmgTakenFromPos);
        DebriesMaker_Scr.instance.HitFromPosAndDir(collisionPoint, dmgTakenFromPos - collisionPoint); //TODO: надо использовать collider.ClosestPoint вместо transform.position;
        Pushback(damage);

        if (curHealth <= 0)
            Explode();
    }
    private void Pushback(int damage)
    {
        transform.position += Vector3.forward * ((float)damage / maxHealth);
    }
    private void Explode()
    {
        DebriesMaker_Scr.instance.ExplodeOnPos(transform.position);
        Destroy(gameObject);
    }

}
