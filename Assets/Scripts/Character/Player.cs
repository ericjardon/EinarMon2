using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed =5;
    public bool isMoving;
    private Vector2 movement;
    private Animator animator;
    public LayerMask solidObjectsLayer;
    public LayerMask tallGrassLayer;
    public LayerMask interactableLayer;
    public int teamId = -1;
    public Team playerTeam;
    // We use an identifier for the current player's pokemon Team.
    // Initially it is 0. 
    // When given a new pokemon, the id changes.
    // The GameLoader saves this int and retrieves the Corresponding team from the TeamManager Object, and reassigns it to the Player

    public event Action<bool> OnStartBattle;        // emite un evento que escucha GameManager para iniciar pelea, indica si rival o salvaje

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

            // Implementar interacción
            if (Input.GetKeyDown(KeyCode.Space)){
                Interact();
            }
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

        if (Physics2D.OverlapCircle(newPosition, 0.1f, solidObjectsLayer | interactableLayer) != null){
            // si no es null, significa que el espacio está ocupado
            // entonces no podemos avanzar y regresamos false
            return false;
        }
        return true;
    }

    private void generateEncounter(){
        if (Physics2D.OverlapCircle(transform.position, 0.2f, tallGrassLayer) != null){
            // Is in tall grass, so generate an encounter

            if (UnityEngine.Random.Range(0.0f,100.0f) <= 10.0){     // 10% de probabilidad de salir un pokemón salvaje
                animator.SetBool("isMoving", false);        // para que deje de animarse durante una batalla
                OnStartBattle(true);        // emite un evento para ser escuchado por GameManager. 
                // True porque es Wild Encounter
            }
        }
    }

    void Interact(){
        // determine facing direction by getting the x and y values of the animator
        var facingDir = new Vector3(animator.GetFloat("movX"), animator.GetFloat("movY"));
        var interactPos = transform.position + facingDir;
        // we will check such positoin for an object in the interactable layer
        var detector = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (detector != null){  // means there is an interactable object in front
            detector.GetComponent<Interactable>()?.Interact();
            // ?. is the null conditional operator. Only if the first operand is not null,
            // run the next function. That is, only if there is actually an interactable class.
            // this way the game doesn't crash if there is not an interctable class present
        }
    }

    public void LoadTeam(){
        // checa la id del Team, dependiendo de la id carga al Team el Team de Team Manager que corresponda.
        var manager = GameObject.FindWithTag("GameController");
        playerTeam.Pokemons =  manager.GetComponent<TeamManager>().getTeamPokemons(teamId);
        playerTeam.Start();     // inicializar los nuevos pokemones
    }
}