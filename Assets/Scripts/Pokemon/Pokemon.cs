using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Pokemon
{

    [SerializeField] PokemonBase _base;
    [SerializeField] int level;

    public PokemonBase pBase { 
        get {
            return _base;
        }
     } 
    public int pLvl { 
        get {
            return level;
        }
     }

    public int HP {get; set;}
    public List<Move> Moves {get; set;}     // esa sintaxis crea la propiedad y añade metodos get y set en una sola línea

    // Instead of a Constructor we have an Init function: sets HP, Moves, according to level
    public void Init(){        
        this.HP = MaxHP;

        this.Moves = new List<Move>();   // dependiendo del nivel agregamos movimientos de la lista de learnableMoves

        foreach (var move in pBase.GetLearnableMoves()) {
            if (move.GetAtLevel() <= this.pLvl){
                Moves.Add(new Move(move.GetMoveBase()));
            }
            if (Moves.Count >=4){
                // ya no podemos agregar más movimientos
                break;
            }
        }

    }

    // Properties of the pokemon. Stats calculated as a function of level and Base Stats
    public int Attack {
        get { return Mathf.FloorToInt((pBase.GetAttack*pLvl) / 100f) +5; }
        // fórmula utilizada por los juegos de pkmn reales. FloorToInt redondea hacia abajo 
        // Formel, die von echten pkmn-Spielen verwendet wird. FloorToInt rundet ab
    }
    public int SpAttack {
        get { return Mathf.FloorToInt((pBase.GetSpAttk*pLvl) / 100f) +5; }
    }
    public int Defense {
        get { return Mathf.FloorToInt((pBase.GetDef*pLvl) / 100f) +5; }
    }
    public int SpDefense {
        get { return Mathf.FloorToInt((pBase.GetSpDef*pLvl) / 100f) +5; }
    }
    public int Speed {
        get { return Mathf.FloorToInt((pBase.GetSpeed*pLvl) / 100f) +5; }
    }

    public int MaxHP {
        get { return Mathf.FloorToInt((pBase.GetMaxHP*pLvl) / 100f) +10; }
    }

    public bool DealDamage(Move move, Pokemon pkmn){
        float subdamage= (2*pkmn.pLvl + 10) /250f;
        int damage= Mathf.FloorToInt(subdamage * move.Base.GetPower()*((float) pkmn.Attack/Defense)) +2;

        HP -= damage;

        if(HP <= 0){
            HP=0;
            return true;
        }
        return false;
    }

    public Move ChooseMove(){
        int randomMove= Random.Range(0, Moves.Count);
        return Moves[randomMove];
    }
}
