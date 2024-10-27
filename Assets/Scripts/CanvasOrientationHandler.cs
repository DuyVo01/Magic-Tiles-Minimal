using UnityEngine;
using UnityEngine.UI;

public class CanvasOrientationHandler : MonoBehaviour
{
    [SerializeField]
    private CanvasScaler canvasScaler;

    [SerializeField]
    private float portraitMatchValue;

    [SerializeField]
    private float landscapeMatchValue;

    ScreenOrientation lastOrientation;

    void Start()
    {
        lastOrientation = Screen.orientation;
        if (
            lastOrientation == ScreenOrientation.Portrait
            || lastOrientation == ScreenOrientation.PortraitUpsideDown
        )
        {
            canvasScaler.matchWidthOrHeight = portraitMatchValue;
        }
        else if (
            lastOrientation == ScreenOrientation.LandscapeLeft
            || lastOrientation == ScreenOrientation.LandscapeRight
        )
        {
            canvasScaler.matchWidthOrHeight = landscapeMatchValue;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastOrientation != Screen.orientation)
        {
            lastOrientation = Screen.orientation;

            if (
                lastOrientation == ScreenOrientation.Portrait
                || lastOrientation == ScreenOrientation.PortraitUpsideDown
            )
            {
                canvasScaler.matchWidthOrHeight = portraitMatchValue;
            }
            else if (
                lastOrientation == ScreenOrientation.LandscapeLeft
                || lastOrientation == ScreenOrientation.LandscapeRight
            )
            {
                canvasScaler.matchWidthOrHeight = landscapeMatchValue;
            }
        }
    }
}
