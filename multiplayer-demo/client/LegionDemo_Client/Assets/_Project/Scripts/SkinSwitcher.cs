using UnityEngine;

public class SkinSwitcher : MonoBehaviour
{
    public GameObject[] skins;

    public void SetSkin(int id)
    {
        for (int i = 0; i < skins.Length; i++)
            skins[i].SetActive(i == id);

        NetworkManager.Instance.SendSkin(id);
    }
}