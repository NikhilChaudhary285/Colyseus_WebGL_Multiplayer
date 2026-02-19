using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isLocal;

    void Update()
    {
        if (!isLocal) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float rot = transform.eulerAngles.y;

        NetworkManager.Instance.room.Send("input", new
        {
            moveX = x,
            moveZ = z,
            rotY = rot,
            jump = Input.GetKey(KeyCode.Space),
            sit = Input.GetKey(KeyCode.C),
            skin = 0
        });
    }
}
