using UnityEngine;

public class Weapon_LazerDrone_Master_Scr : MonoBehaviour
{
    [SerializeField] private GameObject leftDrone, rightDrone;

    private void Awake()
    {
        Instantiate(leftDrone, transform.position, Quaternion.identity);
        Instantiate(rightDrone, transform.position, Quaternion.identity);
    }
}
