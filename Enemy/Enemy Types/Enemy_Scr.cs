using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Enemy_Flash_Scr))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy_Scr : MonoBehaviour, IDamageable
{
    public int EnemyId;
    public float movementSpeed = 2f;
    public float maxHealth = 10f;
    protected float curHealth;
    public int expAward = 2;
    bool behaviourDisabled = false;

    private void Awake()
    {
        Initialize();
    }
    private void Start()
    {
        AddCountToDirector(EnemyId);
    }
    private void Update()
    {
        if (behaviourDisabled)
            return;
        EnemyMovement();
    }

    #region ----Enemy Behaviour
    /// <summary>
    /// Метод для получения врагом урона
    /// </summary>
    /// <param name="damage">Количесвто полученного урона</param>
    /// <param name="dmgTakenFromPos">Позиция с которой был нанесён урон</param>
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        if (curHealth < 0)
            return;

        curHealth -= damage;

        GetComponent<Enemy_Flash_Scr>().StartFlash();
        Collider collider = GetComponent<Collider>();
        Vector3 collisionPoint = collider.ClosestPoint(dmgTakenFromPos);
        DebriesMaker_Scr.instance.HitFromPosAndDir(collisionPoint, dmgTakenFromPos - collisionPoint); //TODO: надо использовать collider.ClosestPoint вместо transform.position;
        //GetComponent<Enemy_HitEffect_Scr>().SpawnParticles(dmgTakenFromPos);

        if (curHealth <= 0)
            Die();
        else
            Pushback(damage);
    }
    /// <summary>
    /// Метод, вызываемый при смерти врага
    /// </summary>
    protected virtual void Die()
    {
        DebriesMaker_Scr.instance.ExplodeOnPos(transform.position);
        UpgradeSystem_Scr.instance.InstantiateExpShards(transform.position, expAward); //TODO: нужна ли анимация?
        //Disappear();
        GroundCrash2();
    }
    protected void GroundCrash()
    {
        behaviourDisabled = true;
        SubCountToDirector(EnemyId);
        Vector3 jumpTarget = transform.position + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-10f, -8f));
        jumpTarget.y = BackgroundManager_Scr.instance.spawnPosition.y;
        float jumpTime = -jumpTarget.y /20;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOJump(jumpTarget, 20f, 1, jumpTime).SetEase(Ease.InQuad));
        sequence.Join(transform.DORotate(new Vector3(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)), jumpTime));
        sequence.AppendCallback(() => { 
            DebriesMaker_Scr.instance.ExplodeOnPos(transform.position);
            Destroy(gameObject);
        });
    }
    protected void GroundCrash2()
    {
        //behaviourDisabled = true;

        Vector3 direction = Random.onUnitSphere;
        float force = Random.Range(2f, 4f);
        SubCountToDirector(EnemyId);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddRelativeForce(direction * force, ForceMode.Impulse);
        DebriesMaker_Scr.instance.AddSmoke(0.4f ,transform);
    }
    /// <summary>
    /// Метод для удаления объекта врага
    /// </summary>
    protected void Disappear()
    {
        SubCountToDirector(EnemyId);
        Destroy(gameObject);
    }
    /// <summary>
    /// Метод, отталкивающий врага при получении урона, на расстояние зависящее от его макс. ХП и полученного урона
    /// </summary>
    /// <param name="damage">Полученный урон</param>
    protected virtual void Pushback(int damage)
    {
        transform.position += Vector3.forward * ((float)damage / maxHealth);
    }
    /// <summary>
    /// Метод, выполняющий передвижения врага
    /// </summary>
    protected virtual void EnemyMovement()
    {
        transform.position += movementSpeed * Time.deltaTime * -transform.forward;
        if (transform.position.z <= -15)
            Disappear();
    }
    protected virtual void EnemyAttack()
    {

    }
    #endregion

    protected virtual void Initialize()
    {
        curHealth = maxHealth;
        //Destroy(gameObject, 15f);
    }
    protected void AddCountToDirector(int id)
    {
        if (Enemy_Director_Scr.enemyCountByID.ContainsKey(id))
        {
            Enemy_Director_Scr.enemyCountByID[id]++;
            Test_EnemyNumber_scr.instance.UpdateLine(id, Enemy_Director_Scr.enemyCountByID[id]);
        }
        else
        {
            Enemy_Director_Scr.enemyCountByID.Add(id, 1);
            Test_EnemyNumber_scr.instance.AddNewLine(id, Enemy_Director_Scr.enemyCountByID[id]);
        }
    }
    protected void SubCountToDirector(int id)
    {
        if (Enemy_Director_Scr.enemyCountByID[id] <= 0)
            return;
        Enemy_Director_Scr.enemyCountByID[id]--;
        Test_EnemyNumber_scr.instance.UpdateLine(id, Enemy_Director_Scr.enemyCountByID[id]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Obstacle"))
            return;

        DebriesMaker_Scr.instance.ExplodeOnPos(transform.position);
        Destroy(gameObject);
    }
}
