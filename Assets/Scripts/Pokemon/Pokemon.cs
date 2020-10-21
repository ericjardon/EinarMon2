using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    public PokemonBase pBase { get; set; } 
    public int pLvl { get; set; }

    public int HP {get; set;}
    public List<Move> Moves {get; set;}     // esa sintaxis crea la propiedad y añade metodos get y set en una sola línea


    public Pokemon(PokemonBase pBase, int pLvl){
        this.pBase = pBase;
        this.pLvl = pLvl;
        this.HP = MaxHP;

        // recién creado no tiene movimientos.
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
