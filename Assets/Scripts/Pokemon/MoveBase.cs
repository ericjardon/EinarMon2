using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sin el CreateAssetMenu attribute no podemos crear instancias de la clase
[CreateAssetMenu(fileName="Move", menuName = "Pokemon/Create new Move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string moveName;
    [TextArea]
    [SerializeField] string description;

    [SerializeField] PKMNType type;

    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int pp;


    public string GetMoveName(){
        return moveName;
    }
    public string GetDesc(){
        return description;
    }
    public PKMNType GetType(){
        return type;
    }
    public int GetPower(){
        return power;
    }
    public int GetAcc(){
        return accuracy;
    }
    public int GetPP(){
        return pp;
    }
}
