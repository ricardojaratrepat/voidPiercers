using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private Transform pfRadarPing;
    private Transform sweetTransform; 
    private float rotationSpeed;
    private float radarDistance;
    private List<Collider2D> colliderList;
    private bool isRotating;

    private static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private void Awake()
    {
        sweetTransform = transform.Find("Sweet");
        rotationSpeed = 180f;
        radarDistance = 40f;
        colliderList = new List<Collider2D>();
        isRotating = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isRotating)
        {
            isRotating = true;
            sweetTransform.gameObject.SetActive(true); // Activar el objeto del radar al comenzar a rotar
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
