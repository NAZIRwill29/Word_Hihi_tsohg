using UnityEngine;

public class SwitchTriggerEnter : Switch
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Toggle();
    }
}