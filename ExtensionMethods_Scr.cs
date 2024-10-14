using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods_Scr
{
    #region Random Points On Screen
    public static Vector3 GetRandomPointFromScreen(this Camera camera, int widthPadding, int heightPadding)
    {
        int screenWidth = camera.pixelWidth;
        int screenHeight = camera.pixelHeight;
        return camera.ScreenToWorldPoint(new Vector3(
            Random.Range(0 + widthPadding, screenWidth - widthPadding),
            Random.Range(0 + heightPadding, screenHeight - heightPadding),
            -camera.transform.position.z));
    }
    public static Vector3 GetRandomPointOnHorizontalLine(this Camera camera, int widthPadding, int lineHeight)
    {
        int screenWidth = camera.pixelWidth;
        int screenHeight = camera.pixelHeight;
        return camera.ScreenToWorldPoint(new Vector3(
            Random.Range(0 + widthPadding, screenWidth - widthPadding),
            lineHeight, -camera.transform.position.z));
    }
    #endregion

    #region Camera Demensions In World
    public static float GetHeightInWorld(this Camera camera)
    {
        return camera.ScreenToViewportPoint(new Vector3(0, camera.pixelHeight, -camera.transform.position.z)).y;
    }
    #endregion

    #region Camera Corners In World
    public static Vector3 GetUpperLeftCorner(this Camera camera)
    {
        return camera.ScreenToViewportPoint(new Vector3(0, camera.pixelHeight, -camera.transform.position.z));
    }
    public static Vector3 GetBottomLeftCorner(this Camera camera)
    {
        return camera.ScreenToViewportPoint(new Vector3(0, 0, -camera.transform.position.z));
    }
    #endregion
}
