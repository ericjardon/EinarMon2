using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move 
{
    public MoveBase Base { get; set; }     // sintaxis breve para crear properties rápidamente,
    // con esta sintaxis es solo accesible y modificable programáticamente pero no desde el inspector de Unity 

    public int PP {get; set;}   // mismo caso que arriba: queremos sea programaticamente modificable pero no desde el inspector
    
    // convención con C# prooperties es nombres en mayúscula inicial.

    //Constructor
    public Move(MoveBase mBase) {
        Base = mBase;
        PP = Base.GetPP();
    }

}
