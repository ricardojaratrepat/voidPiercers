using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Laser
{
    public class LaserController : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private readonly float maxDistance = 10f;
        private Vector3 currentEndPosition;
        public float laserDamage = 0.5f;
        public LayerMask layerMask; // Añadir una variable para la máscara de capa

        private InventoryManager inventoryManager;

        public AudioClip laserSound; // Sonido del láser
        private AudioSource audioSource; // AudioSource para reproducir el sonido

        void Start()
        {
            inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            currentEndPosition = transform.position + Vector3.right * maxDistance; // Inicializar con alguna dirección
            LaserState.currentDuration = 0.0f;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }

        void Update()
        {
            if (Input.GetMouseButton(0) && !inventoryManager.menuActivated) // Verifica si el botón izquierdo del mouse está siendo presionado
            {
                if (LaserState.currentDuration < LaserState.maxDuration)
                {
                    LaserState.currentDuration += Time.deltaTime;
                    StartLaser();
                }
                else
                {
                    StopLaser();
                    string removeMsg = inventoryManager.RemoveItem("Carbon", 12);

                    if (removeMsg == "removed")
                    {
                        LaserState.currentDuration = 0.0f;
                    }
                    else
                    {
                        Debug.Log(removeMsg);
                    }
                }
            }
            else
            {
                StopLaser();
            }
        }

        void StartLaser()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; // Asegúrate de que la posición Z sea 0 ya que estamos en 2D

            Vector3 direction = (mousePos - transform.position).normalized; // Normaliza la dirección
            Vector3 targetEndPosition = transform.position + direction * maxDistance; // Calcula la posición final con la distancia máxima

            // Hacer un Raycast para detectar colisiones, ignorando la capa del jugador
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, layerMask);
            if (hit)
            {
                // Si golpea algo, la posición final será el punto de impacto
                if (hit.collider.CompareTag("Enemy"))
                {
                    targetEndPosition = hit.point;
                    hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(laserDamage);
                }
            }

            // Interpolación para un movimiento más suave
            currentEndPosition = Vector3.Lerp(currentEndPosition, targetEndPosition, Time.deltaTime * 10f);

            lineRenderer.enabled = true; // Mostrar el láser
            lineRenderer.SetPosition(0, transform.position); // Posición inicial del láser
            lineRenderer.SetPosition(1, currentEndPosition); // Posición final del láser con retraso suave o punto de impacto

            // Reproducir el sonido del láser
            if (audioSource != null && laserSound != null)
            {
                audioSource.PlayOneShot(laserSound);
            }
        }

        void StopLaser()
        {
            lineRenderer.enabled = false; // Ocultar el láser
        }
    }
}
