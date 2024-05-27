using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float digJumpForce = 4f; // Nueva variable para la fuerza de salto al excavar hacia arriba
    public bool onGround;
    private Rigidbody2D rb;
    private InventoryManager inventoryManager;
    private float digCooldown = 0.3f;
    private float lastDigTime;
    public float maxRotationAngle = 45f;

    // Texto temporal
    public string tmp_text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus imperdiet, nulla et dictum interdum, nisi lorem egestas odio";

    public CaveLighting caveLighting;
    public float surfaceValue = 110;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
        lastDigTime = -digCooldown; // Allows digging immediately at start
        if (caveLighting == null)
        {
            Debug.LogError("CaveLighting no encontrado en la escena!");
        }
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

    public void DestroyBlock(Vector2 direction)
    {
        float horizontalDistance = 1f;
        float verticalDistance = 1f;
        Vector3 rayOriginOffset = Vector3.zero;
        RaycastHit2D[] hits = new RaycastHit2D[7]; // Inicializar el array con 7 elementos

        // Determinar la posición de origen de los rayos en función de la dirección
        if (direction == Vector2.left)
        {
            rayOriginOffset = new Vector3(-horizontalDistance, 0, 0);
        }
        else if (direction == Vector2.right)
        {
            rayOriginOffset = new Vector3(horizontalDistance, 0, 0);
        }
        else if (direction == Vector2.down)
        {
            rayOriginOffset = new Vector3(0, -verticalDistance, 0);
        }
        else if (direction == Vector2.up)
        {
            rayOriginOffset = new Vector3(0, verticalDistance, 0);
        }

        // Ajustar las posiciones de los rayos según la dirección
        if (direction == Vector2.left || direction == Vector2.right)
        {
            // Ajustar los rayos para originarse en la punta de la nave
            Vector3 tipOffset = new Vector3(direction.x * 1.5f, 0, 0); // Mover más hacia la punta
            hits[0] = Physics2D.Raycast(transform.position + tipOffset + (Vector3.up * verticalDistance * 1.5f), direction, horizontalDistance);
            hits[1] = Physics2D.Raycast(transform.position + tipOffset + (Vector3.up * verticalDistance), direction, horizontalDistance);
            hits[2] = Physics2D.Raycast(transform.position + tipOffset + (Vector3.up * verticalDistance / 2), direction, horizontalDistance);
            hits[3] = Physics2D.Raycast(transform.position + tipOffset, direction, horizontalDistance);
            hits[4] = Physics2D.Raycast(transform.position + tipOffset + (Vector3.down * verticalDistance / 2), direction, horizontalDistance);
            hits[5] = Physics2D.Raycast(transform.position + tipOffset + (Vector3.down * verticalDistance), direction, horizontalDistance);
            hits[6] = Physics2D.Raycast(transform.position + tipOffset + (Vector3.down * verticalDistance * 1.5f), direction, horizontalDistance);
        }
        else if (direction == Vector2.down || direction == Vector2.up)
        {
            hits[0] = Physics2D.Raycast(transform.position + rayOriginOffset + (Vector3.left * horizontalDistance * 1.5f), direction, verticalDistance);
            hits[1] = Physics2D.Raycast(transform.position + rayOriginOffset + (Vector3.left * horizontalDistance), direction, verticalDistance);
            hits[2] = Physics2D.Raycast(transform.position + rayOriginOffset + (Vector3.left * horizontalDistance / 2), direction, verticalDistance);
            hits[3] = Physics2D.Raycast(transform.position + rayOriginOffset, direction, verticalDistance);
            hits[4] = Physics2D.Raycast(transform.position + rayOriginOffset + (Vector3.right * horizontalDistance / 2), direction, verticalDistance);
            hits[5] = Physics2D.Raycast(transform.position + rayOriginOffset + (Vector3.right * horizontalDistance), direction, verticalDistance);
            hits[6] = Physics2D.Raycast(transform.position + rayOriginOffset + (Vector3.right * horizontalDistance * 1.5f), direction, verticalDistance);
        }

        // Visualizar los rayos
        foreach (var hit in hits)
        {
            Vector3 rayStart = transform.position + rayOriginOffset;
            Debug.DrawRay(rayStart, (Vector3)direction * 1.5f, Color.red, 1.5f); // Dibuja un rayo rojo en la escena
            CheckHitAndDestroy(hit);
        }
    }

    private void CheckHitAndDestroy(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);

            if (hit.collider.gameObject.name != "Tierra pasto grande" && hit.collider.gameObject.name != "dirt")
            {
                inventoryManager.AddItem(hit.collider.gameObject.name, 1, hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite, tmp_text);
            }

            Debug.DrawRay(hit.point, Vector2.down * 2f, Color.red, 1.5f);
            if (hit.collider.gameObject.CompareTag("Ground") || hit.collider.gameObject.CompareTag("Ore"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            Debug.Log("No se encontró terreno.");
        }
    }

    void FixedUpdate()
    {
        // Handle movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        bool isMovingLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        bool isMovingRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        // Apply horizontal movement
        Vector2 movement = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        // Apply rotation
        if (isMovingLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (isMovingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Apply jump
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Handle digging
        bool isDigging = false;

        if (Input.GetKey(KeyCode.S) && Time.time - lastDigTime >= digCooldown)
        {
            DestroyBlock(Vector2.down);
            lastDigTime = Time.time;
            float angle = transform.localScale.x > 0 ? -90f : 90; // Ajustar el ángulo basado en la dirección
            transform.rotation = Quaternion.Euler(0f, 0f, angle); // Rotar la nave 90 grados hacia abajo
            isDigging = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (onGround || isDigging) // Verificar si está en el suelo o ya está excavando
            {
                DestroyBlock(Vector2.up);
                rb.velocity = new Vector2(rb.velocity.x, digJumpForce); // Aplicar un pequeño salto
                lastDigTime = Time.time;
                float angle = transform.localScale.x > 0 ? 90f : -90f; // Ajustar el ángulo basado en la dirección
                transform.rotation = Quaternion.Euler(0f, 0f, angle); // Rotar la nave 90 grados hacia arriba
                isDigging = true;
            }
        }

        if (Input.GetKey(KeyCode.A) && Time.time - lastDigTime >= digCooldown)
        {
            DestroyBlock(Vector2.left);
            lastDigTime = Time.time;
        }

        if (Input.GetKey(KeyCode.D) && Time.time - lastDigTime >= digCooldown)
        {
            DestroyBlock(Vector2.right);
            lastDigTime = Time.time;
        }

        // Fix the rotation to ensure the player stays horizontal when not digging
        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        // Cave lighting logic
        if (transform.position.y < surfaceValue)
        {
            caveLighting.SetCaveStatus(true);
        }
        else
        {
            caveLighting.SetCaveStatus(false);
        }

        // Limit rotation angle
        float zRotation = transform.rotation.eulerAngles.z;
        if (zRotation > maxRotationAngle && zRotation < 360 - maxRotationAngle)
        {
            // Calculate target rotation
            float targetZRotation = Mathf.Clamp(zRotation, -maxRotationAngle, maxRotationAngle);
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetZRotation);

            // Interpolate between current rotation and target rotation
            float rotationSpeed = 5.0f; // Adjust this value to change the speed of rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
