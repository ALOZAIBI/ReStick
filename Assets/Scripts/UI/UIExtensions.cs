using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//More than just UIEXtensions
public static class UIExtensions {
    #region RectTransform
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

    public static float GetLeft(this RectTransform rt) {
        return rt.offsetMin.x;
    }

    public static float GetRight(this RectTransform rt) {
        return -rt.offsetMax.x;
    }

    public static float GetTop(this RectTransform rt) {
        return -rt.offsetMax.y;
    }

    public static float GetBottom(this RectTransform rt) {
        return rt.offsetMin.y;
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

    public static void SetAnchorsStretch(this RectTransform rt) {
        rt.SetAnchorBottom(0);
        rt.SetAnchorLeft(0);
        rt.SetAnchorRight(1);
        rt.SetAnchorTop(1);
    }

    public static void SetStretchToAnchors(this RectTransform rt) {
        rt.SetLeft(0);
        rt.SetRight(0);
        rt.SetTop(0);
        rt.SetBottom(0);
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
    #endregion

    public static void RefreshLayoutGroupsImmediateAndRecursive(this GameObject root) {
        foreach (var layoutGroup in root.GetComponentsInChildren<LayoutGroup>()) {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }
    }
    //Sets alpha of an image
    public static void SetAlpha(this Image img,float alpha) {
        Color temp = img.color;
        temp.a = alpha;
        img.color = temp;
    }

    public static void SetAlpha(this SpriteRenderer img, float alpha) {
        Color temp = img.color;
        temp.a = alpha;
        img.color = temp;
    }

    public static void SetAlpha(this SlicedFilledImage img, float alpha) {
        Color temp = img.color;
        temp.a = alpha;
        img.color = temp;
    }
    public static void SetAlpha(this TextMeshProUGUI text, float alpha) {
        Color temp = text.color;
        temp.a = alpha;
        text.color = temp;
    }

    //Slighlty darkens the color of an image
    public static void Darken(this Image img) {
        Color temp = img.color;
        temp.r *= 0.8f;
        temp.g *= 0.8f;
        temp.b *= 0.8f;
        img.color = temp;
    }
    //Ability calculations extension stuff
    public static float getAmtValueFromName(this List<float> array, Ability ability,string valueName) {
        int index = ability.valueNames.IndexOf(valueName);
        if(index == -1) {
            Debug.LogError("Value name not found in ability: " + valueName);
            return 0;
        }
        return array[index];
    }
    public static float getSumOfValues(this List<float> array) {
        float sum = 0;
        foreach (float value in array) {
            sum += value;
        }
        return sum;
    }

    public static void addAbility(this Character character,Ability ability) {
        character.abilities.Add(ability);
        ability.transform.parent = UIManager.singleton.playerParty.activeAbilities.transform;
        character.applyArchetypeLook();
    }

    public static void addItem(this Character character, Item item) {
        character.items.Add(item);
        item.transform.parent = UIManager.singleton.playerParty.activeItems.transform;
        item.applyStats(character);
    }

}