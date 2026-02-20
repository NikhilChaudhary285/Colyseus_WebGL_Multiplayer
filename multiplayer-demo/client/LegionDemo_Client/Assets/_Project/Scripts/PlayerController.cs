using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isLocal;
    public Animator animator;

    float speed = 5f;

    void Update()
    {
        if (!isLocal) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v);
        transform.position += move * speed * Time.deltaTime;

        bool walking = move.magnitude > 0.1f;
        animator.SetBool("walk", walking);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("jump");
            NetworkManager.Instance.SendJump();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("sit", !animator.GetBool("sit"));
            NetworkManager.Instance.SendSit(animator.GetBool("sit"));
        }

        NetworkManager.Instance.SendMove(transform.position, transform.eulerAngles.y, walking);
    }
}