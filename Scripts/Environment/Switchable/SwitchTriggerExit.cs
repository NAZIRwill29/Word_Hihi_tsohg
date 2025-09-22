using UnityEngine;

public class SwitchTriggerExit : Switch
{
    void OnTriggerExit2D(Collider2D other)
    {
        Toggle();
    }
}