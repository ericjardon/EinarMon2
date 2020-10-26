using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Roaming,
    Battling,
    Chatting,
}

public class GameManager : MonoBehaviour
{
    GameState state;

    [SerializeField] Player playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera mapCamera;

    void Start() {
        // Aquí indicamos a qué eventos emitidos por otras clases nos estamos suscribiendo
        // i.e. 'Escuchando'

        playerController.OnStartBattle += Battle;
        battleSystem.OnDefeat += EndBattle;
        DialogManager.Instance.OnStartDialog += () => {
            // si empieza un diálogo, cambiamos estado a chatting
            state = GameState.Chatting;
        };
        DialogManager.Instance.OnEndDialog += () => {
            if (state == GameState.Chatting){
                state = GameState.Roaming;
            }
        };
        
        // notación indica: cuando script emita evento, += corremos algo

    }

    void Battle() {
        state = GameState.Battling;
        battleSystem.gameObject.SetActive(true);    // activamos la vista de modo pelea
        mapCamera.gameObject.SetActive(false);      // desactivamos la vista el terreno

        battleSystem.StartBattle();

    }

    void EndBattle(bool playerWon) {
        // cambiamos el estado, desactivamos BattleSystem, activamos camara principal
        state = GameState.Roaming;
        battleSystem.gameObject.SetActive(false);
        mapCamera.gameObject.SetActive(true);

    }

    void Update() {
        if (state == GameState.Roaming){
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battling){
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Chatting){
            DialogManager.Instance.HandleUpdate();
        }
    }

}
