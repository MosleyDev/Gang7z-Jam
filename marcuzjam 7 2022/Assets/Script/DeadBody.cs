using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpringJoint2D>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            GetComponent<SpringJoint2D>().enabled = true;
            GetComponent<SpringJoint2D>().connectedAnchor = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f);
        }
    }
}
