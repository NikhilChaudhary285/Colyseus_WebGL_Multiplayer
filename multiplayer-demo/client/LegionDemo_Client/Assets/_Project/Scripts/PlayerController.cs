using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public bool isLocal;

    float speed = 3.5f;
    float rotSpeed = 120f;

    Animator anim;
    CharacterController controller;

    // ===== GRAVITY / JUMP =====
    float verticalVelocity = 0f;
    float gravity = -25f;
    float jumpForce = 7f;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (anim) anim.applyRootMotion = false;

        if (!isLocal)
        {
            var ui = FindObjectOfType<SkinSwitcher>();
            if (ui) ui.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isLocal) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // ===== ROTATION =====
        if (Mathf.Abs(h) > 0.01f)
        {
            transform.Rotate(0f, h * rotSpeed * Time.deltaTime, 0f);
        }

        // ===== MOVE VECTOR =====
        Vector3 move = Vector3.zero;

        if (Mathf.Abs(v) > 0.01f)
        {
            move += transform.forward * v * speed;
        }

        // ===== GROUND CHECK =====
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f; // keeps grounded

            // ===== JUMP =====
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                if (anim) anim.SetTrigger("jump");
                NetworkManager.Instance.SendJump();
            }
        }

        // ===== APPLY GRAVITY =====
        verticalVelocity += gravity * Time.deltaTime;
        move.y = verticalVelocity;

        // ===== APPLY MOVEMENT =====
        controller.Move(move * Time.deltaTime);

        // ===== WALK ANIMATION =====
        bool walking = Mathf.Abs(v) > 0.15f;
        if (anim) anim.SetBool("walk", walking);

        // ===== SEND TO SERVER =====
        NetworkManager.Instance.SendMove(
            transform.position,
            transform.eulerAngles.y,
            walking
        );

        // ===== SIT =====
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (anim) anim.SetBool("sit", true);
            NetworkManager.Instance.SendSit(true);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            if (anim) anim.SetBool("sit", false);
            NetworkManager.Instance.SendSit(false);
        }

        // ===== SKINS =====
        if (Input.GetKeyDown(KeyCode.Alpha1)) NetworkManager.Instance.SendSkin(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) NetworkManager.Instance.SendSkin(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) NetworkManager.Instance.SendSkin(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) NetworkManager.Instance.SendSkin(3);
    }
}