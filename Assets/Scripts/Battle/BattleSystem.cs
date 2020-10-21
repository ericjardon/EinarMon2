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

    public event Action<bool> OnDefeat;         // usa un parámetro booleano para saber quién perdió

    BattleState state;
    int currentAction;
    int currentMove; 

    public void StartBattle(){
        StartCoroutine(SetupBattle());
    }

    public void HandleUpdate(){
        if(state== BattleState.PlayerAction){
            HandleAS();
        }else if(state==BattleState.PlayerMove){
            HandleMS();
        }
    }

    public IEnumerator SetupBattle(){
        playerU.Setup();        // el método setup de una playerunit sirve para que se carguen sus atributos, sus sprites, etc.
        enemyU.Setup();
        playerH.SetData(playerU.pkmn);
        enemyH.SetData(enemyU.pkmn);
        // el atributo pkmn de una BattleUnit es ...

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

    IEnumerator ExcecuteEnemyMove(){
        state = BattleState.EnemyMove;
        var move =enemyU.pkmn.ChooseMove();
        yield return dialogB.TD($"{enemyU.pkmn.pBase.GetPName} used {move.Base.GetMoveName()}");
        yield return new WaitForSeconds(1f);
        bool isMorido =playerU.pkmn.DealDamage(move, enemyU.pkmn);

        yield return playerH.AffectHP();
        if (isMorido){
            yield return dialogB.TD($"{playerU.pkmn.pBase.GetPName} was defeated");

            // aquí sería llamar a alguna animación

            yield return new WaitForSeconds(2f);
            OnDefeat(false);     // jugador perdió la pelea
        }else{
            PlayerAction();
        }
    } 

    IEnumerator ExcecutePlayerMove(){
        state = BattleState.Busy;
        var move =playerU.pkmn.Moves[currentMove];
        yield return dialogB.TD($"{playerU.pkmn.pBase.GetPName} used {move.Base.GetMoveName()}");
        yield return new WaitForSeconds(1f);
        bool isMorido =enemyU.pkmn.DealDamage(move, playerU.pkmn);

        yield return enemyH.AffectHP();
        if (isMorido){
            yield return dialogB.TD($"{enemyU.pkmn.pBase.GetPName} was defeated");
            // aquí llamaríamos a la animación

            yield return new WaitForSeconds(2f);
            OnDefeat(true);     // jugador ganó la pelea

        }else{
            StartCoroutine(ExcecuteEnemyMove());
        }
        
    }

    

    void PlayerMove(){
        state = BattleState.PlayerMove;
        dialogB.ShowAS(false);
        dialogB.ShowDT(false);
        dialogB.ShowMS(true);
    }



    void HandleMS(){
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            if(currentMove < playerU.pkmn.Moves.Count-1)
                ++currentMove;
        }else if(Input.GetKeyDown(KeyCode.LeftArrow)){
            if(currentMove > 0)
                --currentMove;
        }else if(Input.GetKeyDown(KeyCode.DownArrow)){
            if(currentMove < playerU.pkmn.Moves.Count - 2)
                currentMove += 2;
        }else if(Input.GetKeyDown(KeyCode.UpArrow)){
            if(currentMove > 1)
                currentMove -= 2;
        }

        dialogB.UpdateMS(currentMove, playerU.pkmn.Moves[currentMove]);

        if(Input.GetKeyDown(KeyCode.Return)){
            dialogB.ShowMS(false);
            dialogB.ShowDT(true);
            StartCoroutine(ExcecutePlayerMove());
        }

    }

    void HandleAS(){
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            if(currentAction < 1)
                ++currentAction;
        }else if(Input.GetKeyDown(KeyCode.UpArrow)){
            if(currentAction>0)
                --currentAction;
        }

        dialogB.UpdateAS(currentAction);

        if(Input.GetKeyDown(KeyCode.Return)){
            if(currentAction==0){
                PlayerMove();
            }else if(currentAction==1){
                OnDefeat(true);     // termina la batalla por mientras
            }
        }
    }

}
