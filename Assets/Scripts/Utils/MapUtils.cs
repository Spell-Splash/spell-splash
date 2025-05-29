using UnityEngine;

public static class MapUtils
{
    public static void GetMapBounds(GameObject mapObject, out Vector2 min, out Vector2 max)
    {
        SpriteRenderer renderer = mapObject.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            min = bounds.min;
            max = bounds.max;
        }
        else
        {
            Debug.LogError("Map object must have a SpriteRenderer.");
            min = Vector2.zero;
            max = Vector2.zero;
        }
    }
}
