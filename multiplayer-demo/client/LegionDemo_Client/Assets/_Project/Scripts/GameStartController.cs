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
        // Safe singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Subscribe to network events
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnMatchStarted += StartGame;
        }

        // Ensure gameplay is disabled initially
        if (GameplayRoot) GameplayRoot.SetActive(false);
    }

    // =============================
    // CALLED WHEN MATCH STARTS
    // =============================
    public void StartGame()
    {
        if (gameStarted) return;
        gameStarted = true;

        Debug.Log("MATCH START RECEIVED");

        // Hide lobby/menu
        if (LobbyUI) LobbyUI.SetActive(false);
        if (MenuUI) MenuUI.SetActive(false);

        // Hide countdown if present
        if (CountdownUI) CountdownUI.SetActive(false);

        // Enable gameplay
        if (GameplayRoot) GameplayRoot.SetActive(true);

        Debug.Log("GAMEPLAY ENABLED");
    }

    void OnDestroy()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnMatchStarted -= StartGame;
        }
    }
}