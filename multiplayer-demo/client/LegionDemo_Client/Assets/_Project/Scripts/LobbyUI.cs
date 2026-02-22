using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LobbyUI : MonoBehaviour
{
    public TMP_Text roomCodeText;
    public TMP_Text playerCountText;
    public TMP_Text waitingText;
    public Transform playerListParent;
    public GameObject playerListItemPrefab;
    public Button readyButton;

    bool isReady = false;

    void Start()
    {
        NetworkManager.Instance.OnPlayerListUpdated += UpdateLobby;
        NetworkManager.Instance.OnRoomJoined += UpdateRoomText;
    }

    public void ToggleReady()
    {
        isReady = !isReady;
        NetworkManager.Instance.SendReady(isReady);
        readyButton.GetComponentInChildren<TMP_Text>().text =
            isReady ? "Unready" : "Ready";
    }

    void UpdateLobby(List<Player> players)
    {
        roomCodeText.text = "Room: " + NetworkManager.Instance.CurrentRoomCode;
        playerCountText.text = players.Count + "/4 Players";

        waitingText.gameObject.SetActive(players.Count < 2);

        foreach (Transform t in playerListParent)
            Destroy(t.gameObject);

        foreach (var p in players)
        {
            var item = Instantiate(playerListItemPrefab, playerListParent);
            item.GetComponent<TMP_Text>().text =
                p.name + (p.ready ? " [Ready]" : " [Waiting]");
        }
    }

    void UpdateRoomText(string roomText)
    {
        roomCodeText.text = roomText;
    }

    void OnDestroy()
    {
        if (NetworkManager.Instance != null)
        {
            NetworkManager.Instance.OnPlayerListUpdated -= UpdateLobby;
            NetworkManager.Instance.OnRoomJoined -= UpdateRoomText;
        }
    }
}