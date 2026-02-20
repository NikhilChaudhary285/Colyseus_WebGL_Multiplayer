using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isLocal;

    float speed = 3.5f;
    float rotSpeed = 120f;

    Animator anim;

    // ===== JUMP PHYSICS =====
    float verticalVelocity = 0f;
    float gravity = -22f;
    float jumpForce = 7f;
    bool isGrounded = true;

    void Start()
    {
        anim = GetComponent<Animator>();

        // Force-disable root motion (extra safety)
        if (anim) anim.applyRootMotion = false;

        // Disable skin UI for remote players only
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

        // ===== HORIZONTAL MOVEMENT =====
        Vector3 move = Vector3.zero;

        if (Mathf.Abs(v) > 0.01f)
        {
            move += transform.forward * v * speed * Time.deltaTime;
        }

        // ===== GRAVITY =====
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        move.y = verticalVelocity * Time.deltaTime;

        transform.position += move;

        // ===== SIMPLE GROUND CHECK =====
        if (transform.position.y <= 0f)
        {
            Vector3 p = transform.position;
            p.y = 0f;
            transform.position = p;

            verticalVelocity = 0f;
            isGrounded = true;
        }

        // ===== WALK DETECTION =====
        bool walking = Mathf.Abs(v) > 0.15f;

        if (anim)
        {
            anim.SetBool("walk", walking);
        }

        // ===== SEND TO SERVER =====
        NetworkManager.Instance.SendMove(
            transform.position,
            transform.eulerAngles.y,
            walking
        );

        // ===== JUMP =====
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity = jumpForce;
            isGrounded = false;

            if (anim) anim.SetTrigger("jump");
            NetworkManager.Instance.SendJump();
        }

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