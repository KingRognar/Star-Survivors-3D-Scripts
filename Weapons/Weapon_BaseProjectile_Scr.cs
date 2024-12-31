using UnityEngine;

public class Weapon_BaseProjectile_Scr : MonoBehaviour
{
    public float projectileSpeed = 10f;

    [SerializeField] private AudioClip[] hitAudioClips;

    //TODO: ����������� � OnBecameInvisible - �� �����-�� ������� ����������� ������ ��� 50 

    void Update()
    {
        ProjectileMovement();
    }

    protected virtual void ProjectileMovement()
    {
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
    }

/*    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }*/
}
