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
    [SerializeField] Trainer rival;
    [SerializeField] Camera mapCamera;
    [SerializeField] GameObject InventoryUI;

    void Start() {
        // Aquí indicamos a qué eventos emitidos por otras clases nos estamos suscribiendo
        // i.e. 'Escuchando'
        playerController.OnStartBattle += Battle;

        battleSystem.OnDefeat += EndBattle;
        DialogManager.Instance.OnStartDialog += () => {
            // si empieza un diálogo, cambiamos estado a chatting
            InventoryUI.SetActive(false);
            state = GameState.Chatting;
        };
        DialogManager.Instance.OnEndDialog += (triggerBattle) => {
            if (state == GameState.Chatting){
                state = GameState.Roaming;
                if (triggerBattle)
                    Battle(false);
            }
        };
        // notación indica: cuando script emita evento, += corremos algo
    }

    void Battle(bool wildEncounter) {
        if (wildEncounter){
            state = GameState.Battling;
            battleSystem.gameObject.SetActive(true);    // activamos la vista de modo pelea
            mapCamera.gameObject.SetActive(false);      // desactivamos la vista el terreno
            InventoryUI.SetActive(false);
            
            var Team = playerController.GetComponent<Team>();
            var enemyP = FindObjectOfType<AreaPokemons>().GetRandomPkmn();
            Debug.Log("Jaló team y enemy p correctamente");
            battleSystem.StartBattle(Team, enemyP);
        }
        else {
            // batalla con rival
            state = GameState.Battling;
            battleSystem.gameObject.SetActive(true);
            mapCamera.gameObject.SetActive(false);
            var Team = playerController.GetComponent<Team>();
            var enemyTeam = rival.GetComponent<Team>();
            battleSystem.StartDuel(Team, enemyTeam);
        }
    }

    void EndBattle(bool playerWon) {
        // cambiamos el estado, desactivamos BattleSystem, activamos camara principal
        state = GameState.Roaming;
        battleSystem.gameObject.SetActive(false);
        mapCamera.gameObject.SetActive(true);
        InventoryUI.SetActive(true);

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
