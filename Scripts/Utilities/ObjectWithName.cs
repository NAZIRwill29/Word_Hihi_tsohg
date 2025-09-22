using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectWithName
{
    public NameDataFlyweight NameData;
}

[System.Serializable]
public class PlayModeObjWithName : ObjectWithName
{
    public List<GameObject> GameObjects;
    public CanvasGroup CanvasGroup;
    public void EnableGameObj(bool isTrue)
    {
        if (CanvasGroup != null)
            CanvasGroupFunc.ModifyCG(CanvasGroup, isTrue ? 1 : 0, isTrue, isTrue);
        foreach (GameObject item in GameObjects)
        {
            item.SetActive(isTrue);
        }
    }
}

[System.Serializable]
public class GameObjectWithName : ObjectWithName
{
    public GameObject GameObject;
}

[System.Serializable]
public class SpriteWithName : ObjectWithName
{
    public Sprite Sprite;
}

[System.Serializable]
public class AnimatorWithName : ObjectWithName
{
    public Animator Animator;
}

[System.Serializable]
public class CanvasGroupWithName : ObjectWithName
{
    public CanvasGroup CanvasGroup;
}

[System.Serializable]
public class ButtonWithName : ObjectWithName
{
    public Button Button;
}

[System.Serializable]
public class GlobalLightDataWithName : ObjectWithName
{
    public GlobalLightDataFlyweight GlobalLightDataFlyweight;
}
