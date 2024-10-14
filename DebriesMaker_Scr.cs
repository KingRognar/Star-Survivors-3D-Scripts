using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebriesMaker_Scr : MonoBehaviour
{
    public static DebriesMaker_Scr instance;

    [SerializeField] private GameObject explosionPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ExplodeOnPos(Vector3 explosionPos)
    {
        Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
    }
}
