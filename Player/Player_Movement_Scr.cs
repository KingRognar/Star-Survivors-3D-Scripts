using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_Scr : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10;
    private Vector3 previousPosition;
    private float actualXSpeed;

    void Update()
    {
        MovePlayerToMousePosition();
    }

    private void MovePlayerToMousePosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Camera.main.transform.position.y));
        previousPosition = transform.position;
        transform.position = Vector3.Lerp(transform.position, new Vector3(mouseWorldPos.x, 0, mouseWorldPos.z), movementSpeed * Time.deltaTime);
        actualXSpeed = (transform.position - previousPosition).x;
        transform.rotation = Quaternion.AngleAxis(-actualXSpeed * 60f, transform.forward);
    }

    public void Pushback()
    {
        //TODO:
    }
}
