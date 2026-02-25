using UnityEngine;

public class RemotePlayer : MonoBehaviour
{
    Vector3 targetPos;
    float targetRot;
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        targetPos = transform.position;
    }

    public void ApplyServerState(Player player)
    {
        Debug.Log($"[REMOTE APPLY] {player.x},{player.y},{player.z} anim:{player.anim}");

        targetPos = new Vector3(player.x, player.y, player.z);
        targetRot = player.rotY;

        // 🔥 APPLY SERVER ANIMATION DIRECTLY
        anim?.SetBool("sit", player.sitting);
        anim?.SetBool("walk", player.anim == "walk");

        if (player.jumping)
        {
            anim?.ResetTrigger("jump");
            anim?.SetTrigger("jump");
        }
    }

    void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * 12f
        );

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, targetRot, 0),
            Time.deltaTime * 12f
        );
    }
}