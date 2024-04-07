using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }   
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            onGround = true;
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ground"))
        {
            onGround = false;
        }
    }

    public void DestroyBlockBelow()
    {


        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 10f);
        Debug.Log(hit.collider.gameObject.name);
        Debug.DrawRay(transform.position, Vector2.down * 10f, Color.red, 1f);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
        {   Debug.Log(hit.collider.gameObject.name);
            Destroy(hit.collider.gameObject);
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxis("Jump");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector2 movement = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
        if (moveHorizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveHorizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (vertical > 0.1f || jump > 0.1f)
        {
            if (onGround)
                movement.y = jumpForce;
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S key was pressed.");
            DestroyBlockBelow();
        }

        rb.velocity = movement;
        

    }
}