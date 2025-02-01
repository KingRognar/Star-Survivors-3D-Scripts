using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DebriesMaker_Scr : MonoBehaviour
{
    public static DebriesMaker_Scr instance;

    private Transform canvasTrans;
    [Space(20)]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private GameObject ScreenFlashPrefab;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        canvasTrans = GameObject.Find("Canvas").transform;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            ScreenFlash();
    }

    public void ExplodeOnPos(Vector3 explosionPos)
    {
        Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
    }
    public async Task BigObjectExplosions(Collider collider,Transform trans, float radius, int explMin, int explMax)
    {
        int explCount = Random.Range(explMin, explMax);

        for (int i = 0; i < explCount; i++)
        {
            Vector3 explosionPos = Random.onUnitSphere * radius;
            explosionPos = collider.ClosestPoint(explosionPos + trans.position);
            ExplodeOnPos(explosionPos);

            int delay = (int)(Random.Range(0, 0.1f) * 1000);
            await Task.Delay(delay);
        }
    }
    public void HitFromPosAndDir(Vector3 pos, Vector3 direction)
    {
        Instantiate(hitPrefab, pos, Quaternion.FromToRotation(Vector3.up, direction));
    }
    public void ScreenFlash()
    {
        Instantiate(ScreenFlashPrefab, canvasTrans);
    }
    public void ScreenFlash(float fadeInTime, float pauseTime, float fadeOutTime)
    {
        GameObject gm = Instantiate(ScreenFlashPrefab, canvasTrans);
        gm.GetComponent<UI_ScreenFlash2_Scr>().SetTimes(fadeInTime, pauseTime, fadeOutTime);
    }



}
