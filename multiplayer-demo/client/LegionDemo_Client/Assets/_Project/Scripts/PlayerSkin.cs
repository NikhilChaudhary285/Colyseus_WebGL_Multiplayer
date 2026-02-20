using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public Material[] skins;
    Renderer [] renderers;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void ApplySkin(int id)
    {
        if (skins == null || skins.Length == 0) return;
        if (id < 0 || id >= skins.Length) return;

        foreach (var item in renderers)
        {
            item.material = skins[id];
        }
    }
}