using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    [SerializeField] private Transform pfRadarPing;  // Prefab del ping de radar
    private Transform sweetTransform;  // Transform del objeto 'Sweet' que podría ser la parte del radar
    public float rotationSpeed;  // Velocidad de rotación del radar
    private float radarDistance;  // Distancia máxima del radar
    private List<Collider2D> colliderList;  // Lista de colliders detectados por el radar
    private bool isRotating;  // Estado del radar, si está rotando o no

    public float cooldown = 7.0f;  // Tiempo de enfriamiento del radar
    private float cooldownTimer;  // Temporizador de enfriamiento
    public Text cooldownText;  // Texto en la UI para mostrar el temporizador

    private static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private void Start()
    {
        sweetTransform.gameObject.SetActive(false); // Desactivar el objeto del radar al inicio
        cooldownTimer = 0;
    }

    private void Awake()
    {
        sweetTransform = transform.Find("Sweet");
        rotationSpeed = 450f;
        radarDistance = 40f;
        colliderList = new List<Collider2D>();
        isRotating = false;
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownText.text = Mathf.CeilToInt(cooldownTimer).ToString() + "s";
            cooldownText.fontSize = 25;  // Tamaño de fuente cuando el radar está en enfriamiento
        }
        else
        {
            cooldownText.text = "Ready to use!";
            cooldownText.fontSize = 20;  // Tamaño de fuente cuando el radar está listo
            if (Input.GetKeyDown(KeyCode.F) && !isRotating)  // Activar el radar con la tecla F
            {
                isRotating = true;
                sweetTransform.gameObject.SetActive(true); // Activar el objeto del radar
                cooldownTimer = cooldown;
            }
        }
        if (isRotating)
        {
            float previousZ = (sweetTransform.eulerAngles.z % 360) - 180;
            sweetTransform.eulerAngles -= new Vector3(0, 0, rotationSpeed * Time.deltaTime);
            float currentRotation = (sweetTransform.eulerAngles.z % 360) - 180;

            if (currentRotation < 0 && previousZ > 0)
            {
                isRotating = false;
                sweetTransform.gameObject.SetActive(false); // Desactivar el objeto del radar cuando deja de rotar
                colliderList.Clear();
            }
            RaycastHit2D[] raycastHit2DArray = Physics2D.RaycastAll(transform.position, GetVectorFromAngle(sweetTransform.eulerAngles.z), radarDistance);
            foreach (RaycastHit2D raycastHit2D in raycastHit2DArray)
            {
                if (raycastHit2D.collider != null)
                {
                    if (!colliderList.Contains(raycastHit2D.collider))
                    {
                        colliderList.Add(raycastHit2D.collider);
                        SpriteRenderer spriteRenderer = raycastHit2D.collider.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null && spriteRenderer.sprite.name == "HealthIcon")
                        {
                            RadarPing radarPing = Instantiate(pfRadarPing, raycastHit2D.point, Quaternion.identity).GetComponent<RadarPing>();
                            radarPing.SetColor(new Color(0, 1, 0));
                        }
                        if (spriteRenderer != null && spriteRenderer.sprite.name == "MinimapBorder")
                        {
                            RadarPing radarPing = Instantiate(pfRadarPing, raycastHit2D.point, Quaternion.identity).GetComponent<RadarPing>();
                            radarPing.SetColor(new Color(1, 0, 0));
                        }
                    }
                }
            }
        }
    }
}
