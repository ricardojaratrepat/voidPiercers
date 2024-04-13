using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;
    private Rigidbody2D rb;

    private InventoryManager inventoryManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
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
        float horizontalDistance = 1f;

        RaycastHit2D hitFarLeft = Physics2D.Raycast(transform.position + Vector3.left * horizontalDistance * 2, Vector2.down, 2f);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + Vector3.left * horizontalDistance, Vector2.down, 2f);
        RaycastHit2D hitCenter = Physics2D.Raycast(transform.position, Vector2.down, 10f);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + Vector3.right * horizontalDistance, Vector2.down, 2f);
        RaycastHit2D hitFarRight = Physics2D.Raycast(transform.position + Vector3.right * horizontalDistance * 2, Vector2.down, 2f);
        CheckHitAndDestroy(hitFarLeft);
        CheckHitAndDestroy(hitLeft);
        CheckHitAndDestroy(hitCenter);
        CheckHitAndDestroy(hitRight);
        CheckHitAndDestroy(hitFarRight);
    }

    private void CheckHitAndDestroy(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            
            if (hit.collider.gameObject.name != "Tierra pasto grande" && hit.collider.gameObject.name != "dirt")
            {
                inventoryManager.AddItem(hit.collider.gameObject.name, 1, hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite);
            }

            Debug.DrawRay(hit.point, Vector2.down * 2f, Color.red, 2f);
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            Debug.Log("No se encontró terreno debajo.");
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
            DestroyBlockBelow();
        }

        rb.velocity = movement;
        

    }
}