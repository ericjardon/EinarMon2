using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar HPBar;

    Pokemon pkmn;

    public void SetData(Pokemon pokemon){

        pkmn= pokemon;
        nameText.text = pokemon.pBase.GetPName;
        levelText.text = "Lvl" + pokemon.pLvl;
        HPBar.SetHP((float) pokemon.HP / pokemon.MaxHP);
    }

    public IEnumerator AffectHP(){
        yield return HPBar.AnimateHPBar((float) pkmn.HP / pkmn.MaxHP);
    }


}
