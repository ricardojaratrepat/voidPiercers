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
    public InventoryManager inventoryManager;

    private bool textCooldownActive = false;


    private static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private IEnumerator ShowItemNeededMessage(string neededItems, float delay)
    {
        textCooldownActive = true;
        cooldownText.text = neededItems;
        yield return new WaitForSeconds(delay);
        cooldownText.text = "";
        textCooldownActive = false;
    }

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
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
            if (!textCooldownActive)
            {
                cooldownText.text = "Listo para usar!";
                cooldownText.fontSize = 20;  // Tamaño de fuente cuando el radar está listo
            }
            if (Input.GetKeyDown(KeyCode.F) && !isRotating)  // Activar el radar con la tecla F
            {
                // Comprobar si los materiales están disponibles
                bool isStoneAvailable = inventoryManager.IsAvailable("Piedra", 50);
                bool isCarbonAvailable = inventoryManager.IsAvailable("Carbon", 10);
                bool isIronAvailable = inventoryManager.IsAvailable("Iron", 5);
                bool isAlfaCrystalAvailable = inventoryManager.IsAvailable("Alfa Crystals", 1);

                if (isStoneAvailable && isCarbonAvailable)
                {
                    // Consumir los materiales
                    string stoneResult = inventoryManager.RemoveItem("Piedra", 50);
                    string carbonResult = inventoryManager.RemoveItem("Carbon", 10);

                    if (stoneResult == "removed" && carbonResult == "removed") 
                    {
                        isRotating = true;
                        sweetTransform.gameObject.SetActive(true); // Activar el objeto del radar
                        cooldownTimer = cooldown;
                    }
                }
                else if (isStoneAvailable && isIronAvailable)
                {
                    // Consumir los materiales
                    string stoneResult = inventoryManager.RemoveItem("Piedra", 50);
                    string ironResult = inventoryManager.RemoveItem("Iron", 5);

                    if (stoneResult == "removed" && ironResult == "removed") 
                    {
                        isRotating = true;
                        sweetTransform.gameObject.SetActive(true); // Activar el objeto del radar
                        cooldownTimer = cooldown;
                    }
                }
                else if (isStoneAvailable && isAlfaCrystalAvailable)
                {
                    // Consumir los materiales
                    string stoneResult = inventoryManager.RemoveItem("Piedra", 50);
                    string alfaCrystalResult = inventoryManager.RemoveItem("Alfa Crystals", 1);

                    if (stoneResult == "removed" && alfaCrystalResult == "removed") 
                    {
                        isRotating = true;
                        sweetTransform.gameObject.SetActive(true); // Activar el objeto del radar
                        cooldownTimer = cooldown;
                    }
                }
                else
                {
                    string neededItems = "";
                    if (!isStoneAvailable) neededItems += "Piedra 50, ";
                    if (!isCarbonAvailable && !isIronAvailable) neededItems += "Carbon 10, Iron 5 or Alfa Crystals 1, ";
                    neededItems = neededItems.TrimEnd(',', ' ');

                    StartCoroutine(ShowItemNeededMessage(neededItems, 2.0f));
                }
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
                    string tag = raycastHit2D.collider.tag;
                    if (tag == "Enemy" || tag.StartsWith("Ore") || tag == "Bench" || tag == "SpaceShip")
                    {
                        if (!colliderList.Contains(raycastHit2D.collider))
                        {
                            colliderList.Add(raycastHit2D.collider);
                            RadarPing radarPing = Instantiate(pfRadarPing, raycastHit2D.point, Quaternion.identity).GetComponent<RadarPing>();
                            Color pingColor = Color.white;  // Color por defecto
                            switch (tag)
                            {
                                case "Enemy":
                                    pingColor = Color.red;
                                    break;
                                case "Ore basic":
                                    pingColor = Color.green;
                                    break;
                                case "Ore medium":
                                    pingColor = Color.blue;
                                    break;
                                case "Ore rare":
                                    pingColor = Color.yellow;
                                    break;
                                case "Bench":
                                    pingColor = Color.cyan;
                                    break;
                                case "Ore legendary":
                                    pingColor = Color.magenta;
                                    break;
                                case "SpaceShip":
                                    pingColor = new Color(1f, 0.5f, 0f, 1f);
                                    break;
                            }
                            radarPing.SetColor(pingColor);
                        }
                    }
                }
            }
        }
    }
}
