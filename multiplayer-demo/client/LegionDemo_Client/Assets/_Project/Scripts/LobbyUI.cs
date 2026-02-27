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
    public Button startButton;       // Host only

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

    public void ToggleReady()
    {
        isReady = !isReady;
        NetworkManager.Instance.SendReady(isReady);
        readyButton.GetComponentInChildren<TMP_Text>().text = isReady ? "Unready" : "Ready";
    }

    public void StartMatchPressed()
    {
        NetworkManager.Instance.RequestStartGame();
    }

    void UpdateLobby(List<Player> players)
    {
        var net = NetworkManager.Instance;
        roomCodeText.text = "Room: " + net.CurrentRoomCode;
        playerCountText.text = players.Count + "/4 Players";
        waitingText.gameObject.SetActive(players.Count < 2);

        if (startButton != null)
            startButton.gameObject.SetActive(net.IsHost);

        // Clear and rebuild list
        foreach (Transform t in playerListParent) Destroy(t.gameObject);
        foreach (var p in players)
        {
            var item = Instantiate(playerListItemPrefab, playerListParent);
            item.GetComponent<TMP_Text>().text = p.name + (p.ready ? " :) Ready" : " ...Waiting");
        }
    }

    void UpdateRoomText(string roomText) => roomCodeText.text = "Room: " + roomText;
    void UpdateCountdown(int seconds)
    {
        if (!countdownText) return;
        countdownText.gameObject.SetActive(seconds > 0);
        countdownText.text = "Match starts in " + seconds;
    }

    void HandleMatchStart() => gameObject.SetActive(false);

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