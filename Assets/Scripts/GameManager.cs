using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Roaming,
    Battling
}

public class GameManager : MonoBehaviour
{
    GameState state;

    [SerializeField] Player playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera mapCamera;

    void Start() {
        playerController.OnStartBattle += Battle;
        battleSystem.OnDefeat += EndBattle;
        // notación indica: cuando script emita evento, += llamamos a nuestro método

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
    }

}
