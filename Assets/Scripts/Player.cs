using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed =5;
    private bool isMoving;
    private Vector2 movement;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    public LayerMask tallGrassLayer;

    public event Action OnStartBattle;

    void Awake(){           
        // Awake se llama cuando un GameObject es inicializado,
        // y se llama antes del Start() de cualquier otro objeto en la escena.
        animator = GetComponent<Animator>();
    }

    public void HandleUpdate(){      // escucha input de las teclas en cada frame

    // en lugar de Update usamos un método nuestro para que Unity no lo llame en automático.
    // esto nos ayuda para controlar entre el BattleSystem y el Free Roam.
        if (!isMoving){
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");        
            // GetAxisRaw solo devuelve 1, -1 o 0

            // Para que no exista movimiento diagonal. Da preferencia a movimiento horizontal
            if (movement.x != 0) movement.y = 0;

            if (movement != Vector2.zero) {     // si hay input en las teclas

                animator.SetFloat("movX", movement.x);
                animator.SetFloat("movY", movement.y);

                Vector3 newPosition = transform.position;
                newPosition.x += movement.x;
                newPosition.y += movement.y;

                // solo nos movemos si la posición está en una tile caminable
                if (canWalk(newPosition)){
                    StartCoroutine(Move(newPosition));
                }
            }
            animator.SetBool("isMoving", isMoving);
        }
    }

    /* Para que el desplazamiento se de suave y en un lapso de tiempo fijo al estilo pokemon usamos una Co-rutina.
    IEnumerator es una interface que soporta una iteración simple sobre una colección no-genérica. [docs.microsoft]
    A diferencia de una función normal que se completa en un solo frame Update, una corutina puede pausarse, regresar 
    a Unity y después continuar donde se quedó. Esto permite efectos suaves y contiguos. (Unity Manual - Coroutines) 
    */

    IEnumerator Move(Vector3 newPosition){      // para declarar una Corutina debemos usar el tipo IEnumerator
        isMoving = true;

        while((newPosition - transform.position).sqrMagnitude > Mathf.Epsilon){
            // mientras que la distancia entre la posición actual y la posición objetivo sea mayor a un valor muy pequeño:
            // Mathf.epsilon es el valor más pequeño que un float puede tener diferente de cero

            // mientras haya una mínima diferencia entre posision actual y objetivo, nos acercamos a objetivo
            transform.position = Vector3.MoveTowards(transform.position, newPosition, speed* Time.deltaTime);
            // Vector3 es una función built-in, recibe una posición inicial y una posición objetivo y acerca al objeto un poquito más a cada llamada.
            // "By updating an object’s position each frame using the position calculated by this function, 
            //you can move it towards the target smoothly." (Unity Documentation)
            yield return null;
            // yield return pause la ejecución de la corutina y la resume hasta la siguiente llamada de Update.
        }
        // Una vez terminado el loop, estamos lo suficientemente cerca para que no se vea un cambio brusco
        // y podemos asignar la posición actual a la posición objetivo directamente.

        transform.position = newPosition;   
        isMoving = false;   
        generateEncounter();    // a cada paso checamos si está en pasto alto y generamos un pokemon salvaje 1/10 veces
    }

    private bool canWalk(Vector3 newPosition){
        // Solo podemos avanzar si la tile no está ocupada por un objeto sólido.
        // podemos usar un Overlap Circle que es como un detector circular, funciona parecido a Raycast
        // Especificamos la posición, el radio del círculo y la layer en la que queremos que detecte.

        if (Physics2D.OverlapCircle(newPosition, 0.1f, solidObjectsLayer) != null){
            // si no es null, significa que el espacio está ocupado
            // entonces no podemos avanzar y regresamos false
            Debug.Log("Solid Object Detected");
            return false;
        }
        return true;
    }

    private void generateEncounter(){
        if (Physics2D.OverlapCircle(transform.position, 0.2f, tallGrassLayer) != null){
            // Is in tall grass, so generate an encounter

            if (UnityEngine.Random.Range(0.0f,100.0f) <= 10.0){     // 10% de probabilidad de salir un pokemón salvaje
                animator.SetBool("isMoving", false);        // para que deje de animarse durante una batalla
                OnStartBattle();        // emite un evento para ser escuchado por GameManager
            }
        }
    }
}
