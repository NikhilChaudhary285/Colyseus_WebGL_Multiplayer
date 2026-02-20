using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public Material[] skins;
    Renderer rend;

    void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    public void ApplySkin(int id)
    {
        if (skins == null || skins.Length == 0) return;
        if (id < 0 || id >= skins.Length) return;

        rend.material = skins[id];
    }
}