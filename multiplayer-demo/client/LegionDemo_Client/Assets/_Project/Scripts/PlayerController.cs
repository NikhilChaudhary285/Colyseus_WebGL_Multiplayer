using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public bool isLocal;

    float speed = 3.5f;
    float rotSpeed = 120f;

    Animator anim;
    CharacterController controller;

    float verticalVelocity = 0f;
    float gravity = -25f;
    float jumpForce = 7f;

    float sendTimer;
    const float sendRate = 1f / 30f; // 🔥 30 updates/sec

    private Vector3 lastSentPosition;
    private float lastSentRotationY = -999f;
    private string lastSentAnim = "";

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        if (anim) anim.applyRootMotion = false;

        lastSentPosition = transform.position;
        lastSentRotationY = transform.eulerAngles.y;
        lastSentAnim = "idle";
    }

    void Update()
    {
        if (!isLocal) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) > 0.01f)
            transform.Rotate(0f, h * rotSpeed * Time.deltaTime, 0f);

        Vector3 move = Vector3.zero;

        if (Mathf.Abs(v) > 0.01f)
            move += transform.forward * v * speed;

        // GROUND
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                anim?.SetTrigger("jump");
                Debug.Log("[LOCAL] Jump pressed");
                NetworkManager.Instance.SendJump();
            }
        }

        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        bool walking = Mathf.Abs(v) > 0.15f;
        anim?.SetBool("walk", walking);

        sendTimer += Time.deltaTime;
        if (sendTimer >= sendRate)
        {
            sendTimer = 0f;

            Vector3 currentPos = transform.position;
            float currentRot = transform.eulerAngles.y;
            string currentAnim = walking ? "walk" : "idle";

            bool positionChanged = Vector3.Distance(currentPos, lastSentPosition) > 0.02f;
            bool rotationChanged = Mathf.Abs(currentRot - lastSentRotationY) > 1.5f;
            bool animChanged = currentAnim != lastSentAnim;

            if (positionChanged || rotationChanged || animChanged)
            {
                NetworkManager.Instance.SendMove(currentPos, currentRot, walking);

                lastSentPosition = currentPos;
                lastSentRotationY = currentRot;
                lastSentAnim = currentAnim;
            }
        }

        // SIT
        if (Input.GetKeyDown(KeyCode.C))
        {
            anim?.SetBool("sit", true);
            NetworkManager.Instance.SendSit(true);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            anim?.SetBool("sit", false);
            NetworkManager.Instance.SendSit(false);
        }

        // ===== SKINS =====
        if (Input.GetKeyDown(KeyCode.Alpha1)) NetworkManager.Instance.SendSkin(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) NetworkManager.Instance.SendSkin(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) NetworkManager.Instance.SendSkin(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) NetworkManager.Instance.SendSkin(3);
    }
}