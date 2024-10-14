using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_CircleBot_Scr : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(transform.forward, Player_Stats_Scr.CircleBots.rotationSpeed * Time.deltaTime, Space.Self);
    }
}
