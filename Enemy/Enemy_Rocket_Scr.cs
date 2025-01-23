using UnityEngine;

public class Enemy_Rocket_Scr : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth; private float curHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turningSpeed;

    private Transform playerTrans;

    //TODO: �������� �� ��������� �� ������ (�������� ����������� �� ������, ���� �� ����� ������������)
    //TODO: �������� ������ ���� ��� ������������
    //TODO: �������� lifetime, ����� �������� ������ ���� ����������, ���� �� ����� ������������, �� ������� �������?
    //TODO: �������� ������������ �� ������ ��� ������

    private void Start()
    {
        playerTrans = Player_Stats_Scr.instance.transform;
        curHealth = maxHealth;
    }
    void Update()
    {
        RocketMovement();
    }

    private void RocketMovement()
    {
        transform.position += transform.forward * Time.deltaTime * movementSpeed;

        Vector3 playerPos = playerTrans.position;
        Quaternion LookAtPlayerRotation = Quaternion.LookRotation(playerPos - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, LookAtPlayerRotation, turningSpeed * Time.deltaTime);
    }
    public void TakeDamage(int damage, Vector3 dmgTakenFromPos)
    {
        curHealth -= damage;

        GetComponent<Enemy_Flash_Scr>().StartFlash();
        Collider collider = GetComponent<Collider>();
        Vector3 collisionPoint = collider.ClosestPoint(dmgTakenFromPos);
        DebriesMaker_Scr.instance.HitFromPosAndDir(collisionPoint, dmgTakenFromPos - collisionPoint); //TODO: ���� ������������ collider.ClosestPoint ������ transform.position;
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
