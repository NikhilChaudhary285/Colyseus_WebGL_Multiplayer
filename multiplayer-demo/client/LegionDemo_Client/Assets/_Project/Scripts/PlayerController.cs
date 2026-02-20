using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isLocal;

    float speed = 3.5f;
    float rotSpeed = 120f;

    void Start()
    {
        if (!isLocal || isLocal)
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

        // ROTATE
        transform.Rotate(0, h * rotSpeed * Time.deltaTime, 0);

        // MOVE
        transform.position += transform.forward * 10f * speed * Time.deltaTime;

        bool walking = Mathf.Abs(v) > 0.05f;

        // LOCAL ANIMATION CONTROL
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetBool("walk", walking);

        // SEND TO SERVER
        NetworkManager.Instance.SendMove(
            transform.position,
            transform.eulerAngles.y,
            walking
        );

        // JUMP
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (anim != null) anim.SetTrigger("jump");
            NetworkManager.Instance.SendJump();
        }

        // SIT
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (anim != null) anim.SetBool("sit", true);
            NetworkManager.Instance.SendSit(true);
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            if (anim != null) anim.SetBool("sit", false);
            NetworkManager.Instance.SendSit(false);
        }

        // SKINS
        if (Input.GetKeyDown(KeyCode.Alpha1)) NetworkManager.Instance.SendSkin(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) NetworkManager.Instance.SendSkin(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) NetworkManager.Instance.SendSkin(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) NetworkManager.Instance.SendSkin(3);
    }
}