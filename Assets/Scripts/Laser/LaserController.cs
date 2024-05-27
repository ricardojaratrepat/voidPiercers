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
        public LayerMask layerMask; // Añadir una variable para la máscara de capa

        void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            currentEndPosition = transform.position + Vector3.right * maxDistance; // Inicializar con alguna dirección
        }

        void Update()
        {
            if (Input.GetMouseButton(0)) // Verifica si el botón izquierdo del mouse está siendo presionado
            {
                if (LaserState.currentDuration < LaserState.maxDuration)
                {
                    LaserState.currentDuration += Time.deltaTime;
                    StartLaser();
                }
                else
                {
                    StopLaser();
                }
            }
            else
            {
                StopLaser();
            }
            Debug.Log(LaserState.currentDuration);

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
                    Debug.Log("Hit enemy");
                    Destroy(hit.collider.gameObject);
                }
            }

            // Interpolación para un movimiento más suave
            currentEndPosition = Vector3.Lerp(currentEndPosition, targetEndPosition, Time.deltaTime * 10f);

            lineRenderer.enabled = true; // Mostrar el láser
            lineRenderer.SetPosition(0, transform.position); // Posición inicial del láser
            lineRenderer.SetPosition(1, currentEndPosition); // Posición final del láser con retraso suave o punto de impacto
        }

        void StopLaser()
        {
            lineRenderer.enabled = false; // Ocultar el láser
        }
    }

}
