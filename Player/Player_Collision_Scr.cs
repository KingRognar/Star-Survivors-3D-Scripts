using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Collision_Scr : MonoBehaviour
{
    [SerializeField] private UI_HP_Bar_Scr UI_HP_Bar;

    private void OnTriggerEnter(Collider other)//TODO: сделать интерфейсы для получения урона?
    {
        if (other.gameObject.CompareTag("Enemy"))
            TakeDamage(2);
        if (other.gameObject.CompareTag("Enemy_Proj"))
        {
            TakeDamage(other.gameObject.GetComponent<Enemy_BaseProj_Scr>().projDamage);
            Destroy(other.gameObject);
        }
        /*if (other.gameObject.CompareTag("Border"))
        {
            Debug.Log(other.gameObject.name);
        }*/
    }

    private void TakeDamage(int damage)
    {
        GetComponent<Enemy_Flash_Scr>().StartFlash();
        
        Player_Stats_Scr.Ship.curHp -= damage;
        if (Player_Stats_Scr.Ship.curHp <= 0)
            Die();

        if (UI_HP_Bar != null)
            UI_HP_Bar.UpdateHPBar();
        else
            Debug.Log("Missing HP Bar", this);
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
