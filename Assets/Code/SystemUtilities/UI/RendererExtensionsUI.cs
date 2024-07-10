using UnityEngine;

public static class RendererExtensionsUI
{
    private static int CountCornersVisibleFrom(RectTransform rectTransform, Camera camera)
    {
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height);
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        int visibleCorners = 0;
        Vector3 tempScreenSpaceCorner;

        for (var i = 0; i < objectCorners.Length; i++)
        {
            tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); 
            if (screenBounds.Contains(tempScreenSpaceCorner)) 
            {
                visibleCorners++;
            }
        }
        return visibleCorners;
    }

    public static bool IsFullyVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        return CountCornersVisibleFrom(rectTransform, camera) == 4;
    }

    public static bool IsVisibleFrom(this RectTransform rectTransform, Camera camera)
    {
        return CountCornersVisibleFrom(rectTransform, camera) > 0;
    }

    public static bool Overlaps(RectTransform a, RectTransform b)
    {
        return WorldRect(a).Overlaps(WorldRect(b));
    }
    public static bool Overlaps(RectTransform a, RectTransform b, bool allowInverse)
    {
        return WorldRect(a).Overlaps(WorldRect(b), allowInverse);
    }

    public static Rect WorldRect(RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;
        return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
    }
}
