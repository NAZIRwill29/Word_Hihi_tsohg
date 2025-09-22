using UnityEngine;

public class SwitchTriggerStay : Switch
{
    void OnTriggerStay2D(Collider2D other)
    {
        Toggle();
    }
}