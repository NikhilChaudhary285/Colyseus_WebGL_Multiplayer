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
        anim?.SetBool("sit", player.sitting);
        if (!player.sitting)
        {  // NEW: Only set walk if not sitting
            anim?.SetBool("walk", player.anim == "walk");
        }
        if (player.jumping)
        {
            anim?.ResetTrigger("jump");
            anim?.SetTrigger("jump");
        }
    }

    void Update()
    {
        if (targetPos == Vector3.zero) return; // safety

        float distance = Vector3.Distance(transform.position, targetPos);

        float posSpeed = distance > 1.5f ? 25f : 12f;   // snap faster when far
        float rotSpeed = distance > 1.5f ? 18f : 12f;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * posSpeed);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, targetRot, 0),
            Time.deltaTime * rotSpeed
        );

        // Optional: debug when big correction happens
        if (distance > 2f)
        {
            Debug.Log($"[REMOTE CORRECTION] Snapping {distance:F2}m → {targetPos}");
        }
    }
}