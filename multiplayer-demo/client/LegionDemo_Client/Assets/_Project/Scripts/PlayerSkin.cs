using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public Material[] skins;
    Renderer[] renderers;

    void Awake()
    {
        SetupRenderers();
    }

    // Helper to ensure renderers are found even if Awake hasn't finished
    void SetupRenderers()
    {
        if (renderers == null || renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<Renderer>();
        }
    }

    public void ApplySkin(int id)
    {
        // 1. Safety checks for the skin array
        if (skins == null || skins.Length == 0) return;

        // 2. Ensure renderers are actually assigned
        SetupRenderers();
        if (renderers == null || renderers.Length == 0) return;

        // 3. Clamp the ID so it never goes out of bounds
        int skinIndex = Mathf.Clamp(id, 0, skins.Length - 1);

        foreach (var item in renderers)
        {
            if (item != null) // Extra safety check
            {
                item.material = skins[skinIndex];
            }
        }
    }
}