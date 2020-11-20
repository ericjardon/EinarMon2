using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Roaming,
    Battling,
    Chatting,
    Menu,
}

public class GameManager : MonoBehaviour
{
    public GameState state;

    public GameObject lastRival;
    public GameObject finalScreen;

    [SerializeField] Player playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Trainer rival;
    [SerializeField] Camera mapCamera;
    [SerializeField] GameObject InventoryUI;
    [SerializeField] StartMenu Menu;
    

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

    public void SetRival(Trainer rival){
        this.rival = rival;
    }

    public void sendMenu(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void EndBattle(bool playerWon) {
        // cambiamos el estado, desactivamos BattleSystem, activamos camara principal
        if (!playerWon){
            // recargar la escena
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);

        } else {
            state = GameState.Roaming;
            battleSystem.gameObject.SetActive(false);
            mapCamera.gameObject.SetActive(true);
            InventoryUI.SetActive(true);

            if (GameObject.FindWithTag("DefinitiveRival").GetComponent<Trainer>().defeated){
                state = GameState.Menu;
                battleSystem.gameObject.SetActive(false);
                mapCamera.gameObject.SetActive(false);
                InventoryUI.SetActive(false);
                finalScreen.SetActive(true);
            }
        }
  
    }

    void Update() {

        if (state == GameState.Roaming){
            Menu.gameObject.SetActive(false);
            playerController.HandleUpdate();
            InventoryUI.SetActive(true);
        }
        else if (state == GameState.Battling){
            Menu.gameObject.SetActive(false);
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Chatting){
            Menu.gameObject.SetActive(false);
            DialogManager.Instance.HandleUpdate();
        }   
        else if (state == GameState.Menu){
            InventoryUI.SetActive(false);           // desactivamos inventory UI
        }
    }

}
