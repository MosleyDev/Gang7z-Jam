using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    public bool isOnLava;

    Rigidbody2D rb;
    SpringJoint2D sj;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sj = GetComponent<SpringJoint2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            isOnLava = true;

            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            sj.enabled = true;
            sj.connectedAnchor = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f);
        }
    }
}
