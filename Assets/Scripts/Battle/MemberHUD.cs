using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar HPBar;
    [SerializeField] Color highlightColor;

    Pokemon pkmn;

    public void SetData(Pokemon pokemon){

        pkmn= pokemon;
        nameText.text = pokemon.pBase.GetPName;
        levelText.text = "Lvl" + pokemon.pLvl;
        HPBar.SetHP((float) pokemon.HP / pokemon.MaxHP);
    }

    public void SetEmpty(){
        nameText.text = "---";
        levelText.text = "-";
        HPBar.SetHP((float) 0);
    }

    public void HighlightName(bool isSelected){
        if (isSelected){
            nameText.color = highlightColor; 
        } else {
            nameText.color = Color.black;
        }
    }

}
