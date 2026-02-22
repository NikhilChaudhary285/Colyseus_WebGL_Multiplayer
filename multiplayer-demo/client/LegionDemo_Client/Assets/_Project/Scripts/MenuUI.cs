using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public TMP_InputField roomInput;
    public TMP_InputField playerNameInput;

    public async void CreateRoom()
    {
        string playerName = string.IsNullOrWhiteSpace(playerNameInput.text)
            ? "Player"
            : playerNameInput.text;

        await NetworkManager.Instance.CreateRoom(playerName);
    }

    public async void JoinRoom()
    {
        string playerName = string.IsNullOrWhiteSpace(playerNameInput.text)
            ? "Player"
            : playerNameInput.text;

        await NetworkManager.Instance.JoinRoomByCode(
            roomInput.text,
            playerName
        );
    }
}