using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using UnityEngine.UI;

public class MicrobarImageAnim : MicrobarGraphic
{
    [SerializeField] List<CanvasGroup> microbarCGs;

    protected override void ShowBar(int num)
    {
        foreach (var item in microbarCGs)
        {
            item.alpha = 0;
        }
        microbarCGs[num].alpha = 1;
    }
}
