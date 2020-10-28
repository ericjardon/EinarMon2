using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    
    [SerializeField] bool isPlayerUnit;

    public Pokemon pkmn { get; set; }

    public void Setup(Pokemon pokemon){
        pkmn = pokemon;
        if (isPlayerUnit)
            GetComponent<Image>().sprite= pkmn.pBase.GetBack;
        else
            GetComponent<Image>().sprite= pkmn.pBase.GetFront;
        
    }
}
