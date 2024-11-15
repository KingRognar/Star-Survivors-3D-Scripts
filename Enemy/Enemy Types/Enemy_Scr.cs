using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy_Flash_Scr))]
[RequireComponent(typeof(Enemy_HitEffect_Scr))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy_Scr : MonoBehaviour, IDamageable
{
    public int EnemyId;
    public float movementSpeed = 2f;
    public float maxHealth = 10f;
    protected float curHealth;
    public int expAward = 2;

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
        EnemyMovement();
    }

    #region ----Enemy Behaviour
    /// <summary>
    /// ����� ��� ��������� ������ �����
    /// </summary>
    /// <param name="damage">���������� ����������� �����</param>
    /// <param name="dmgTakenFromPos">������� � ������� ��� ������ ����</param>
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        curHealth -= damage;

        GetComponent<Enemy_Flash_Scr>().StartFlash();
        DebriesMaker_Scr.instance.HitFromPosAndDir(transform.position, dmgTakenFromPos - transform.position);
        //GetComponent<Enemy_HitEffect_Scr>().SpawnParticles(dmgTakenFromPos);
        Pushback(damage);

        if (curHealth <= 0)
            Die();
    }
    /// <summary>
    /// �����, ���������� ��� ������ �����
    /// </summary>
    protected virtual void Die()
    {
        DebriesMaker_Scr.instance.ExplodeOnPos(transform.position);
        UpgradeSystem_Scr.instance.InstantiateExpShards(transform.position, expAward); //TODO: ����� �� ��������?
        Disappear();
    }
    /// <summary>
    /// ����� ��� �������� ������� �����
    /// </summary>
    protected void Disappear()
    {
        SubCountToDirector(EnemyId);
        Destroy(gameObject);
    }
    /// <summary>
    /// �����, ������������� ����� ��� ��������� �����, �� ���������� ��������� �� ��� ����. �� � ����������� �����
    /// </summary>
    /// <param name="damage">���������� ����</param>
    protected virtual void Pushback(int damage)
    {
        transform.position += Vector3.forward * ((float)damage / maxHealth);
    }
    /// <summary>
    /// �����, ����������� ������������ �����
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
}
