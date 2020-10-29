using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState {Start, PlayerAction, PlayerMove, EnemyMove, Busy};

public class BattleSystem : MonoBehaviour
{
    // SerializeField sirve para hacer público algo en nuestro Editor (?). 
    [SerializeField] BattleUnit playerU;
    [SerializeField] BattleHud playerH;

    [SerializeField] BattleUnit enemyU;
    [SerializeField] BattleHud enemyH;
    [SerializeField] DialogBox dialogB;
    [SerializeField] TeamScreen teamScreen;

    public event Action<bool> OnDefeat;         // usa un parámetro booleano para saber quién perdió

    BattleState state;
    int currentAction;
    int currentMove; 
    bool isTrainerBattle=false;

    Team pkmnTeam;
    Pokemon enemyP;


    public void StartBattle(Team pkmnTeam, Pokemon enemyP){
        this.pkmnTeam = pkmnTeam;
        this.enemyP = enemyP;
        StartCoroutine(SetupBattle());
    }

    public void HandleUpdate(){
        if(state== BattleState.PlayerAction){
            StartCoroutine(HandleAS());
        }else if(state==BattleState.PlayerMove){
            HandleMS();
        }
    }

    public IEnumerator SetupBattle(){
        // el método setup de una playerunit sirve para que se carguen sus atributos, sus sprites, etc.
        playerU.Setup(pkmnTeam.GetAlivePokemon()); // Llamamos a la funcion GetAlivePokemon para que el player inicie la batalla con el primer pokemon vivo que tenga   
        enemyU.Setup(enemyP);
        playerH.SetData(playerU.pkmn);
        enemyH.SetData(enemyU.pkmn);
        
        
        teamScreen.Init();

        dialogB.NameMoves(playerU.pkmn.Moves); 
        // escribir en la dialog box los movimientos existentes para el pokemon que tenemos

        yield return dialogB.TD($"A wild {enemyU.pkmn.pBase.GetPName} appeared");
        // hasta que no se termine de hacer lo de arriba no podemos correr esta línea. Es llamada asíncrona.
                
        yield return new WaitForSeconds(1f);    // hardcodeamos 1 segundo para dar tiempo de lectura 

        PlayerAction();
    }

    public void PlayerAction(){
        state= BattleState.PlayerAction;
        StartCoroutine(dialogB.TD("Choose an action"));
        dialogB.ShowAS(true);
    }


    void ViewTeamScreen() {
        teamScreen.SetTeamData(pkmnTeam.Pokemons);
        teamScreen.gameObject.SetActive(true);
    }


    IEnumerator ExcecutePlayerMove(){
        state = BattleState.Busy;
        var move =playerU.pkmn.Moves[currentMove];
        move.PP --; // reducir el PP del movimiento y actualizar en HUD
        yield return dialogB.TD($"{playerU.pkmn.pBase.GetPName} used {move.Base.GetMoveName()}");
        yield return new WaitForSeconds(1f);
        bool isDefeated =enemyU.pkmn.DealDamage(move, playerU.pkmn);

        yield return enemyH.AffectHP();
        if (isDefeated){
            yield return dialogB.TD($"{enemyU.pkmn.pBase.GetPName} was defeated");
            // aquí llamaríamos a la animación

            yield return new WaitForSeconds(2f);
            OnDefeat(true);     // jugador ganó la pelea

        }else{
            StartCoroutine(ExcecuteEnemyMove());
        }
    }

    IEnumerator ExcecuteEnemyMove(){
        state = BattleState.EnemyMove;
        var move =enemyU.pkmn.ChooseMove();
        move.PP --;
        
        yield return dialogB.TD($"{enemyU.pkmn.pBase.GetPName} used {move.Base.GetMoveName()}");
        yield return new WaitForSeconds(1f);
        
        bool isDefeated =playerU.pkmn.DealDamage(move, enemyU.pkmn);

        yield return playerH.AffectHP();
        if (isDefeated){
            yield return dialogB.TD($"{playerU.pkmn.pBase.GetPName} was defeated");

            // aquí sería llamar a alguna animación

            yield return new WaitForSeconds(2f);
            var NextAlivePokemon = pkmnTeam.GetAlivePokemon();
            if (NextAlivePokemon != null){
                // el método setup de una playerunit sirve para que se carguen sus atributos, sus sprites, etc.
                playerU.Setup(NextAlivePokemon); // Llamamos a la funcion GetAlivePokemon para que el player inicie la batalla con el primer pokemon vivo que tenga
                playerH.SetData(NextAlivePokemon);
                dialogB.NameMoves(NextAlivePokemon.Moves); 
                // escribir en la dialog box los movimientos existentes para el pokemon que tenemos

                yield return dialogB.TD($"Attack {NextAlivePokemon.pBase.GetPName}");
                PlayerAction();
            }else{
                OnDefeat(false);     // false porque jugador perdió la pelea
            }
            
        }else{
            // si derrota al pokemon agual y ya no hay pokemon restantes en el Team
            PlayerAction();
        }
    } 


    void PlayerMove(){
        state = BattleState.PlayerMove;
        dialogB.ShowAS(false);
        dialogB.ShowDT(false);
        dialogB.ShowMS(true);
    }


    /*Handler para selección de Move*/
    void HandleMS(){
        if (Input.GetKeyDown(KeyCode.RightArrow)){
                    ++currentMove;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            --currentMove;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            currentMove += 2;     // mueve dos espacios para quedar directamente abajo
        } else if (Input.GetKeyDown(KeyCode.UpArrow)){
            currentMove -= 2;
        }
        // Solo hay 4 opciones, entonces 4 índices. Podemos usar Mathf.Clamp() para restringir el rango de valores
        // que puede tener currentAction: entre 0 y 3. 
        currentMove = Mathf.Clamp(currentMove, 0, playerU.pkmn.Moves.Count - 1);

        dialogB.UpdateMS(currentMove, playerU.pkmn.Moves[currentMove]);     // resalta la selección de color

        if(Input.GetKeyDown(KeyCode.Return)){
            dialogB.ShowMS(false);
            dialogB.ShowDT(true);
            StartCoroutine(ExcecutePlayerMove());
        }
        if (Input.GetKeyDown(KeyCode.Backspace)){
            dialogB.ShowMS(false);
            dialogB.ShowDT(true);   // habilitamos dialog text y deshabilitamos move selector
            PlayerAction();
        }
    }

    /*Handler para selección de Acción*/
    IEnumerator HandleAS(){

        if (Input.GetKeyDown(KeyCode.RightArrow)){
            ++currentAction;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            --currentAction;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            currentAction += 2;     // mueve dos espacios para quedar directamente abajo
        } else if (Input.GetKeyDown(KeyCode.UpArrow)){
            currentAction -= 2;
        }
        currentAction = Mathf.Clamp(currentAction, 0,3);       // misma lógica que para Move Selector

        dialogB.UpdateAS(currentAction);

        if(Input.GetKeyDown(KeyCode.Return)){
            if(currentAction==0){
                PlayerMove();
            } else if (currentAction==2){
                ViewTeamScreen();
                
            } else if(currentAction==3){
                // Cuando el player este luchando contra un entrenador enemigo no pueda escapar
                if (isTrainerBattle){
                    yield return dialogB.TD("You can't run away from your destiny");
                    yield return new WaitForSeconds(1f);
                    PlayerAction();
                }else{
                    OnDefeat(true); 
                }
                    // termina la batalla por mientras
            }
        }
    }
}
