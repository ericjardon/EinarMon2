using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState {
    Start, 
    PlayerAction, 
    PlayerMove, 
    EnemyMove, 
    Busy,       // busy used for transitions between states when nothing is happening
    TeamScreen
};

public class BattleSystem : MonoBehaviour
{
    // SerializeField sirve para poder definir el valor desde el Inspector de Unity
    [SerializeField] BattleUnit playerU;
    [SerializeField] BattleUnit enemyU;
    [SerializeField] BattleHud playerH;
    [SerializeField] BattleHud enemyH;
    [SerializeField] DialogBox dialogB;
    [SerializeField] TeamScreen teamScreen;

    public event Action<bool> OnDefeat;         // Emite un evento al terminar batalla que indica quién perdió
    BattleState state;
    bool isTrainerBattle;         // para distinguir wild pokemon de Rival Battle

    // índices de selección. En C#, value de un int es 0 por default
    int currentAction;
    int currentMove;
    int currentMember;

    // Objetos específicos a la batalla
    Team playerTeam;
    Pokemon enemyP;
    Team enemyTeam;

    public void StartBattle(Team playerTeam, Pokemon enemyP){
        isTrainerBattle = false;
        this.playerTeam = playerTeam;
        this.enemyP = enemyP;
        StartCoroutine(SetupBattle());
    }
    public void StartDuel(Team playerTeam, Team enemyTeam){
        isTrainerBattle = true;
        this.playerTeam = playerTeam;
        this.enemyTeam = enemyTeam;
        StartCoroutine(SetupDuel());
    }

    public void HandleUpdate(){
        // aquí llamamos a los Handlers de acuerdo con el estado actual. 
        if(state== BattleState.PlayerAction){
            StartCoroutine(HandleAS());     // handler Action Selector
        }else if(state==BattleState.PlayerMove){
            HandleMS();     // handler move selection
        }else if (state==BattleState.TeamScreen){
            HandleTS();     // handler Team selection  
        }
    }

    public IEnumerator SetupBattle(){
        // el método setup de una playerunit sirve para que se carguen sus atributos, sus sprites, etc.
        playerU.Setup(playerTeam.GetAlivePokemon()); // Llamamos a la funcion GetAlivePokemon para que el player inicie la batalla con el primer pokemon vivo que tenga   
        enemyU.Setup(enemyP);
        playerH.SetData(playerU.pkmn);
        enemyH.SetData(enemyU.pkmn);
        teamScreen.Init();

        dialogB.SetMoveNames(playerU.pkmn.Moves); 
        // escribir en la dialog box los movimientos existentes para el pokemon que tenemos

        // uso de yield return detiene ejecución de función actual y corre por completo la función indicada en return
        yield return dialogB.TD($"A wild {enemyU.pkmn.pBase.GetPName} appeared");   
        yield return new WaitForSeconds(1f);    // hardcodeamos 1 segundo para dar tiempo de lectura 
        // resumimos ejecución de SetupBattle
        PlayerAction();
    }

    public IEnumerator SetupDuel(){
        playerU.Setup(playerTeam.GetAlivePokemon());
        enemyU.Setup(enemyTeam.GetAlivePokemon());
        playerH.SetData(playerU.pkmn);
        enemyH.SetData(enemyTeam.GetAlivePokemon());
        teamScreen.Init();

        dialogB.SetMoveNames(playerU.pkmn.Moves); 
        yield return dialogB.TD($"Rival has challenged you to battle");   
        yield return new WaitForSeconds(1f);   
        PlayerAction();
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
            OnDefeat(true);
        }
        else{
            StartCoroutine(ExcecuteEnemyMove());
        }
    }

    IEnumerator ExcecutePlayerMoveDuel(){
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

            if (enemyTeam.GetAlivePokemon() != null){
                // El jugador puede escoger el siguiente pokemon a sacar
                state = BattleState.Busy;
                StartCoroutine(SwitchRivalPkmn(enemyTeam.GetAlivePokemon()));
            }else{
                enemyTeam.gameObject.GetComponent<Trainer>().isDefeated();
                yield return dialogB.TD("Rival: What just happened?");
                yield return new WaitForSeconds(2.0f);
                OnDefeat(true);     // jugador ganó la pelea
            }
        }
        else{
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
            var NextAlivePokemon = playerTeam.GetAlivePokemon();
            // Llamamos a la funcion GetAlivePokemon para saber si quedan Pokemones vivos en el Team
            if (NextAlivePokemon != null){
                // El jugador puede escoger el siguiente pokemon a sacar
                ViewTeamScreen();
            }else{
                yield return dialogB.TD("You have met your fate. Game Over.");
                yield return new WaitForSeconds(3.0f);
                OnDefeat(false);     // si no quedan pokemon vivos, el jugador pierde la pelea
            }    
        }
        else{
            PlayerAction();     // es turno del player de atacar
        }
    } 

    /*--- CHANGE OF STATE ---*/

    public void PlayerAction(){
        state= BattleState.PlayerAction;
        StartCoroutine(dialogB.TD("Choose an action"));
        dialogB.ShowAS(true);
    }
    
    void ViewTeamScreen() {
        state = BattleState.TeamScreen;
        teamScreen.SetTeamData(playerTeam.Pokemons);
        teamScreen.gameObject.SetActive(true);
    }

    void PlayerMove(){
        state = BattleState.PlayerMove;
        dialogB.ShowAS(false);
        dialogB.ShowDT(false);
        dialogB.ShowMS(true);
    }

    /* --- HANDLERS ESPECÍFICOS DE ESTADO DE BATALLA ---*/

    // Move Selector
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
            if (isTrainerBattle){
                StartCoroutine(ExcecutePlayerMoveDuel());
            } else {
                StartCoroutine(ExcecutePlayerMove());
            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace)){
            dialogB.ShowMS(false);
            dialogB.ShowDT(true);   // habilitamos dialog text y deshabilitamos move selector
            PlayerAction();
        }
    }

    // Action Selector
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
                    // termina la batalla, por mientras
            }
        }
    }

    // Team Selector
    void HandleTS(){
        // Misma lógica de selección que Move Selection
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            ++currentMember;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            --currentMember;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            currentMember += 2;     // mueve dos espacios para quedar directamente abajo
        } else if (Input.GetKeyDown(KeyCode.UpArrow)){
            currentMember -= 2;
        }

        currentMember = Mathf.Clamp(currentMember, 0, playerTeam.Pokemons.Count - 1);
        teamScreen.UpdateTS(currentMember);

        if (Input.GetKeyDown(KeyCode.Backspace)){
            teamScreen.gameObject.SetActive(false);
            PlayerAction();     
        }
        else if (Input.GetKeyDown(KeyCode.Return)){
            
            var selected = playerTeam.Pokemons[currentMember];
            if (selected.HP <= 0){
                // no podemos escoger pokemon que no estén vivos, mostrar mensaje
                teamScreen.SetInfoText($"Your {selected.pBase.GetPName} is out of combat!");
                return;
            }
            if (selected == playerU.pkmn){
                teamScreen.SetInfoText($"Your {selected.pBase.GetPName} is already in battle!");
                return;
            }

            // Else, if selection is valid, perform the pokemon switch
            teamScreen.gameObject.SetActive(false);
            state = BattleState.Busy;
            StartCoroutine(SwitchPokemon(selected));

        }
    }

    IEnumerator SwitchPokemon(Pokemon nextPokemon){
        if (playerU.pkmn.HP > 0){
            // Es un switch voluntario, pokemon actual sigue vivo
            yield return dialogB.TD($"Enough, {playerU.pkmn.pBase.GetPName}");
            // TODO: AQUÍ IRÍA ANIMACIÓN DE SALIDA
            yield return new WaitForSeconds(1f);
        }

        playerU.Setup(nextPokemon); 
        playerH.SetData(nextPokemon);
        dialogB.SetMoveNames(nextPokemon.Moves); 

        yield return dialogB.TD($"Attack, {nextPokemon.pBase.GetPName}!");
        yield return new WaitForSeconds(1f);
        
        StartCoroutine(ExcecuteEnemyMove());        // turno del enemigo después de hacer un cambio
    }

    IEnumerator SwitchRivalPkmn(Pokemon nextPokemon){
        Debug.Log("Switching Rival pkmn");
        enemyU.Setup(nextPokemon); 
        enemyH.SetData(nextPokemon);

        yield return dialogB.TD("Rival: you still haven't defeated me!");
        yield return new WaitForSeconds(1f);
        
        PlayerAction();        // turno del jugador después de derrotar pokemon rival
    }

}