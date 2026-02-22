using UnityEngine;

public class GameStartController : MonoBehaviour
{
    public static GameStartController Instance;

    [Header("UI Panels")]
    public GameObject LobbyUI;
    public GameObject MenuUI;

    [Header("Gameplay Root")]
    public GameObject GameplayRoot;

    void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        // Disable UI
        if (LobbyUI) LobbyUI.SetActive(false);
        if (MenuUI) MenuUI.SetActive(false);

        // Enable gameplay objects
        if (GameplayRoot) GameplayRoot.SetActive(true);

        Debug.Log("GAME STARTED");
    }
}