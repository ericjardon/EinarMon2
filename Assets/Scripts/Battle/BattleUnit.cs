using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    
    [SerializeField] bool isPlayerUnit;

    public Pokemon pkmn { get; set; }

    public void Setup(Pokemon pokemon){
    // el método setup de una playerunit sirve para que se carguen sus atributos, sus sprites, etc.
    
        pkmn = pokemon;
        if (isPlayerUnit)
        // debería aquí correr la animación de ENTRADA de un sprite de player
            GetComponent<Image>().sprite= pkmn.pBase.GetBack;
        else
        // debería aquí correr la animación de ENTRADA de un sprite de rival
            GetComponent<Image>().sprite= pkmn.pBase.GetFront;
        
    }
}
