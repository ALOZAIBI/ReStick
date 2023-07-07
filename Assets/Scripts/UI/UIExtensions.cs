using UnityEngine;

public static class UIExtensions {
    public static void SetLeft(this RectTransform rt, float left) {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right) {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top) {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom) {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }


    //setting anchors through script moves it while keeping the bottom/left/right/top position the same(Changing through editor moves the position as well)
    public static void SetAnchorLeft(this RectTransform rt, float left) {
        rt.anchorMin = new Vector2(left, rt.anchorMin.y);
    }

    public static void SetAnchorRight(this RectTransform rt, float right) {
        rt.anchorMax = new Vector2(right, rt.anchorMax.y);
    }

    public static void SetAnchorTop(this RectTransform rt, float top) {
        rt.anchorMax = new Vector2(rt.anchorMax.x, top);
    }

    public static void SetAnchorBottom(this RectTransform rt, float bottom) {
        rt.anchorMin = new Vector2(rt.anchorMin.x, bottom);
    }

    public static float GetAnchorLeft(this RectTransform rt) {
        return rt.anchorMin.x;
    }

    public static float GetAnchorRight(this RectTransform rt) {
        return rt.anchorMax.x;
    }

    public static float GetAnchorTop(this RectTransform rt) {
        return rt.anchorMax.y;
    }

    public static float GetAnchorBottom(this RectTransform rt) {
        return rt.anchorMin.y;
    }
}