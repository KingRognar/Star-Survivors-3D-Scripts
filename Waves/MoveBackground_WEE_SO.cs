using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move Background WEE", menuName = "Scriptable Objects/New Move Background WEE", order = 5)]
[Serializable] public class MoveBackground_WEE_SO : WaveEndEvent_SO
{
    public float newHeight;
    public override void OnWaveEnd()
    {
        BackgroundManager_Scr.instance.SetNewHeight(newHeight);
    }
}
