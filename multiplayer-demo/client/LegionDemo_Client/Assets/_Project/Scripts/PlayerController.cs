using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isLocal;

    float speed = 4f;
    float rotSpeed = 120f;
    CharacterController cc;

    void Start()
    {
        cc = gameObject.AddComponent<CharacterController>();
        if (!isLocal)
        {
            // disable UI if not local player
            var ui = FindObjectOfType<SkinSwitcher>();
            if (ui) ui.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isLocal) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // ROTATE
        transform.Rotate(0, h * rotSpeed * Time.deltaTime, 0);

        // MOVE
        Vector3 move = transform.forward * v * speed;
        cc.SimpleMove(move);

        bool walking = Mathf.Abs(v) > 0.1f;

        // SEND MOVE TO SERVER
        NetworkManager.Instance.SendMove(
            transform.position,
            transform.eulerAngles.y,
            walking
        );

        // JUMP
        if (Input.GetKeyDown(KeyCode.Space))
            NetworkManager.Instance.SendJump();

        // SIT TOGGLE
        if (Input.GetKeyDown(KeyCode.C))
            NetworkManager.Instance.SendSit(true);

        if (Input.GetKeyUp(KeyCode.C))
            NetworkManager.Instance.SendSit(false);

        // SKIN CHANGE (1–4 keys demo)
        if (Input.GetKeyDown(KeyCode.Alpha1)) NetworkManager.Instance.SendSkin(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) NetworkManager.Instance.SendSkin(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) NetworkManager.Instance.SendSkin(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) NetworkManager.Instance.SendSkin(3);
    }
}