using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Test_EnemyNumber_scr : MonoBehaviour
{
    public static Test_EnemyNumber_scr instance;

    [SerializeField] private GameObject textPrefab;
    
    [SerializeField] private Dictionary<int, TMP_Text> tmpList = new Dictionary<int, TMP_Text>(); 

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    
    public void AddNewLine(int id, int count)
    {
        GameObject newLine = Instantiate(textPrefab, transform);
        tmpList[id] = newLine.GetComponent<TMP_Text>();
        UpdateLine(id, count);
    }
    public void UpdateLine(int id, int count)
    {
        tmpList[id].text = "id: " + id + " ; count: " + count;
    }
}
