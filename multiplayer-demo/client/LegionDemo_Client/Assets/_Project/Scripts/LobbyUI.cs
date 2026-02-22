using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyUI : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text roomCodeText;
    public TMP_Text playerCountText;
    public TMP_Text waitingText;
    public TMP_Text countdownText;

    [Header("Player List")]
    public Transform playerListParent;
    public GameObject playerListItemPrefab;

    [Header("Buttons")]
    public Button readyButton;
    public Button startButton; // host only

    bool isReady = false;

    void Start()
    {
        var net = NetworkManager.Instance;

        net.OnPlayerListUpdated += UpdateLobby;
        net.OnRoomJoined += UpdateRoomText;
        net.OnCountdown += UpdateCountdown;
        net.OnMatchStarted += HandleMatchStart;

        if (countdownText) countdownText.gameObject.SetActive(false);
        if (startButton) startButton.gameObject.SetActive(false);
    }

    // =============================
    // READY BUTTON
    // =============================
    public void ToggleReady()
    {
        isReady = !isReady;
        NetworkManager.Instance.SendReady(isReady);

        readyButton.GetComponentInChildren<TMP_Text>().text =
            isReady ? "Unready" : "Ready";
    }

    // =============================
    // HOST START BUTTON
    // =============================
    public void StartMatchPressed()
    {
        NetworkManager.Instance.RequestStartGame();
    }

    // =============================
    // UPDATE LOBBY UI
    // =============================
    void UpdateLobby(List<Player> players)
    {
        var net = NetworkManager.Instance;

        roomCodeText.text = "Room: " + net.CurrentRoomCode;
        playerCountText.text = players.Count + "/4 Players";

        waitingText.gameObject.SetActive(players.Count < 2);

        // HOST BUTTON VISIBILITY
        if (startButton != null)
            startButton.gameObject.SetActive(net.IsHost);

        // CLEAR OLD LIST
        foreach (Transform t in playerListParent)
            Destroy(t.gameObject);

        // BUILD PLAYER LIST
        foreach (var p in players)
        {
            var item = Instantiate(playerListItemPrefab, playerListParent);
            item.GetComponent<TMP_Text>().text =
                p.name + (p.ready ? "  ✔ Ready" : "  … Waiting");
        }
    }

    // =============================
    // ROOM CODE UPDATE
    // =============================
    void UpdateRoomText(string roomText)
    {
        roomCodeText.text = "Room: " + roomText;
    }

    // =============================
    // COUNTDOWN DISPLAY
    // =============================
    void UpdateCountdown(int seconds)
    {
        if (!countdownText) return;

        if (seconds <= 0)
        {
            countdownText.gameObject.SetActive(false);
            return;
        }

        countdownText.gameObject.SetActive(true);
        countdownText.text = "Match starts in " + seconds;
    }

    // =============================
    // MATCH STARTED
    // =============================
    void HandleMatchStart()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (NetworkManager.Instance == null) return;

        var net = NetworkManager.Instance;

        net.OnPlayerListUpdated -= UpdateLobby;
        net.OnRoomJoined -= UpdateRoomText;
        net.OnCountdown -= UpdateCountdown;
        net.OnMatchStarted -= HandleMatchStart;
    }
}