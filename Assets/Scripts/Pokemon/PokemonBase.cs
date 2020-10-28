using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "PKMN", menuName="Pokemon/Create new Pokemon" )]

public class PokemonBase : ScriptableObject
{
    // Usar Serialize Field nos deja editar los atributos desde el editor de Unity sin hacerlos públicos
    [SerializeField] string pkmnName;

    [TextArea]  // Al declarar TextArea el string puede ser editado en una Área de Texto en el editor.
    [SerializeField] string description;
    
    [SerializeField] Sprite front;
    [SerializeField] Sprite back;
    [SerializeField] PKMNType type;

    // Pokemon Base Stats. A partir de estos valores y el nivel se calculan las stats actuales del pokemon
    [SerializeField] int maxHP;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int spAttack;
    [SerializeField] int spDefense;
    [SerializeField] int speed;
    [SerializeField] List<LearnableMove> learnableMoves;


    public string GetPName {
        get { return pkmnName; }
    }
    public string GetDesc{
        get { return description; }
    }
    public Sprite GetFront{
        get { return front; }
    }
    public Sprite GetBack{
        get { return back; }
    }
    public PKMNType GetType{
        get { return type; }
    }
    public int GetMaxHP{
        get { return maxHP; }
    }
    public int GetAttack{
        get { return attack; }
    }
    public int GetSpAttk{
        get { return spAttack; }
    }
    public int GetDef{
        get { return defense; }
    }
    public int GetSpDef{
        get { return spDefense; }
    }
    public int GetSpeed{
        get {return speed; }
    }
    public List<LearnableMove> GetLearnableMoves(){
        return learnableMoves;
    }

    /*Otra opción para exponer variables privadas es usando C# Properties. Ejemplo:
    public string Name{
        get {return name;}
    }
    */

}

[System.Serializable]
public class LearnableMove 
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int atLevel;

    public MoveBase GetMoveBase(){
        return moveBase;
    }
    public int GetAtLevel(){
        return atLevel;
    }

}

public enum PKMNType
{
    NoType,
    Mecha,
    Demon,
    Normal,
    Fire,
    Grass,
    Water
}