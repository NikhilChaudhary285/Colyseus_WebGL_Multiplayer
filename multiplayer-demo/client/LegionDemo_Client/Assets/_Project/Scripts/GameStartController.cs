using UnityEngine;

public class GameStartController : MonoBehaviour
{
    public static GameStartController Instance;

    [Header("UI Panels")]
    public GameObject LobbyUI;
    public GameObject MenuUI;

    [Header("Gameplay Root")]
    public GameObject GameplayRoot;

    [Header("Optional Countdown UI")]
    public GameObject CountdownUI;

    bool gameStarted = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        if (NetworkManager.Instance != null)
            NetworkManager.Instance.OnMatchStarted += StartGame;

        if (GameplayRoot) GameplayRoot.SetActive(false);
    }

    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;

        Debug.Log("MATCH START RECEIVED");

        if (LobbyUI) LobbyUI.SetActive(false);
        if (MenuUI) MenuUI.SetActive(false);
        if (CountdownUI) CountdownUI.SetActive(false);
        if (GameplayRoot) GameplayRoot.SetActive(true);

        Debug.Log("GAMEPLAY ENABLED");
    }

    void OnDestroy()
    {
        if (NetworkManager.Instance != null)
            NetworkManager.Instance.OnMatchStarted -= StartGame;
    }
}