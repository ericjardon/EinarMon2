using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] PokemonBase pBase;
    [SerializeField] int lvl;
    [SerializeField] bool isPlayerUnit;

    public Pokemon pkmn { get; set; }

    public void Setup(){
        pkmn = new Pokemon(pBase,lvl);
        if (isPlayerUnit)
            GetComponent<Image>().sprite= pkmn.pBase.GetBack;
        else
            GetComponent<Image>().sprite= pkmn.pBase.GetFront;
        
    }
}
