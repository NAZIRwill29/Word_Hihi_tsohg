using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microlight.MicroBar;
using UnityEngine.UI;

public class MicrobarSpriteAnim : MicrobarGraphic
{
    [SerializeField] List<microbarSR> microbarSRs = new();

    [System.Serializable]
    public class microbarSR
    {
        public SpriteRenderer[] srs;
    }

    protected override void ShowBar(int num)
    {
        for (int i = 0; i < microbarSRs.Count; i++)
        {
            foreach (var item in microbarSRs[i].srs)
            {
                item.enabled = false;
            }
        }
        foreach (var item in microbarSRs[num].srs)
        {
            item.enabled = true;
        }
    }
}
