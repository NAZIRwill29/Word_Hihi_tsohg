using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public string dialog;

    public void SetDialog()
    {
        UIHandler.instance.m_NonPlayerDialogue_DialogText.text = dialog;
    }
}
