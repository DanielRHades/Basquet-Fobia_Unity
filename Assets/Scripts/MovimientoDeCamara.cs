using System.Collections;
using System.Collections.Generic;
/*
Resumen del Script de Movimiento de C�mara:

Este script controla la c�mara en un juego de Unity, haciendo que siga a dos jugadores (etiquetados como "Player1" y "Player2"). 
La c�mara ajusta su posici�n entre un punto m�ximo (maxPoint) y un punto medio basado en la distancia entre los jugadores.

Funcionalidades Clave:
1. **Buscar Jugadores**: El script busca a los jugadores en cada actualizaci�n mediante sus etiquetas.
2. **Calcular Posici�n Media**: Calcula el punto medio entre los dos jugadores.
3. **Calcular Distancia**: Determina la distancia entre los jugadores.
4. **Interpolaci�n**: Utiliza `Mathf.InverseLerp` para ajustar la posici�n de la c�mara entre `maxPoint` y el `midpoint` seg�n la distancia.
5. **Movimiento Suave**: La c�mara se mueve suavemente hacia su nueva posici�n utilizando `Vector3.Lerp`.
6. **Orientaci�n de la C�mara**: La c�mara siempre mira hacia el punto medio entre los jugadores.

Configuraciones Importantes en el Inspector:
- `minDistance`: Distancia m�nima para acercar la c�mara (ej. 2).
- `maxDistance`: Distancia m�xima para alejar la c�mara (ej. 10).
- `offset`: Offset constante desde el punto medio (ej. 5).
- No se asi funciona bien con los valores del codigo, estan como cambiados xd

Este script permite que la c�mara se ajuste din�micamente a la posici�n de los jugadores, manteni�ndolos siempre en el encuadre adecuado.
*/

using UnityEngine;

public class MovimientoDeCamara : MonoBehaviour
{
    public Camera camera; // Asigna la c�mara desde el Inspector
    public Transform maxPoint; // Objeto vac�o como el punto m�ximo
    public float minDistance = 20f; // Distancia m�nima para acercar la c�mara
    public float maxDistance = 0f; // Distancia m�xima para alejar la c�mara
    public float offset = 0f; // Offset desde el punto medio

    private Transform player1; // Jugador 1
    private Transform player2; // Jugador 2

    void LateUpdate()
    {
        // Buscar jugadores por tag en cada actualizaci�n
        player1 = GameObject.FindGameObjectWithTag("Player1")?.transform;
        player2 = GameObject.FindGameObjectWithTag("Player2")?.transform;

        // Verificar que ambos jugadores est�n activos
        if (player1 == null || player2 == null) return;

        // Calcular la posici�n media entre los dos jugadores
        Vector3 midpoint = (player1.position + player2.position) / 2;

        // Calcular la distancia entre los jugadores
        float distance = Vector3.Distance(player1.position, player2.position);

        // Invertir la l�gica: si est�n cerca, queremos que la c�mara est� m�s cerca del midpoint
        float t = Mathf.InverseLerp(minDistance, maxDistance, distance);
        Vector3 targetPosition = Vector3.Lerp(maxPoint.position, midpoint, t);

        // Establecer la nueva posici�n de la c�mara usando el offset
        Vector3 newCameraPosition = targetPosition - (targetPosition - midpoint).normalized * offset;

        // Mover la c�mara suavemente a la nueva posici�n
        camera.transform.position = Vector3.Lerp(camera.transform.position, newCameraPosition, Time.deltaTime * 5f);

        // Hacer que la c�mara mire hacia el punto medio
        camera.transform.LookAt(midpoint);
    }
}
