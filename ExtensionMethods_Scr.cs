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
        Vector3 returnVector = camera.ScreenToWorldPoint(new Vector3(
            Random.Range(0 + widthPadding, screenWidth - widthPadding),
            Random.Range(0 + heightPadding, screenHeight - heightPadding),
            camera.transform.position.y));
        returnVector.y = 0;
        return returnVector;
    }
    public static Vector3 GetRandomPointOnHorizontalLine(this Camera camera, int widthPadding, int lineHeight)
    {
        int screenWidth = camera.pixelWidth;
        Vector3 returnVector = camera.ScreenToWorldPoint(new Vector3(
            Random.Range(0 + widthPadding, screenWidth - widthPadding),
            lineHeight, camera.transform.position.y));
        returnVector.y = 0;
        return returnVector;
    }
    #endregion

    #region Camera Demensions In World
    public static float GetHeightInWorld(this Camera camera)
    {
        return camera.ScreenToViewportPoint(new Vector3(0, camera.pixelHeight, camera.transform.position.y)).y;
    }
    #endregion

    #region Camera Corners In World
    public static Vector3 GetUpperLeftCorner(this Camera camera)
    {
        return camera.ScreenToViewportPoint(new Vector3(0, camera.pixelHeight, camera.transform.position.y));
    }
    public static Vector3 GetBottomLeftCorner(this Camera camera)
    {
        return camera.ScreenToViewportPoint(new Vector3(0, 0, camera.transform.position.y));
    }
    #endregion
}
