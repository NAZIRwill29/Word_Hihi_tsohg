using UnityEngine;

public static class LayerInteraction
{
    public static bool IsLayerInteractable(Collider2D other, LayerMask interactableLayers)
    {
        if (other == null)
        {
            Debug.LogWarning("[LayerInteraction] Collider2D is null.");
            return false;
        }

        int otherLayer = other.gameObject.layer;
        if ((interactableLayers.value & (1 << otherLayer)) != 0)
        {
            return true;
        }

        string layerName = LayerMask.LayerToName(otherLayer);
        //Debug.Log($"[LayerInteraction] Layer '{(string.IsNullOrEmpty(layerName) ? otherLayer.ToString() : layerName)}' is not interactable.");
        return false;
    }

    public static bool IsLayerInteractable(Collider other, LayerMask interactableLayers)
    {
        if (other == null)
        {
            Debug.LogWarning("[LayerInteraction] Collider2D is null.");
            return false;
        }

        int otherLayer = other.gameObject.layer;
        if ((interactableLayers.value & (1 << otherLayer)) != 0)
        {
            return true;
        }

        string layerName = LayerMask.LayerToName(otherLayer);
        //Debug.Log($"[LayerInteraction] Layer '{(string.IsNullOrEmpty(layerName) ? otherLayer.ToString() : layerName)}' is not interactable.");
        return false;
    }
}
