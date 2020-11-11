using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamScreen : MonoBehaviour
{
    [SerializeField] Text infoText;     // text shown to user
    MemberHUD[] members;        // array of HUDs in screen
    List<Pokemon> pkmns;


    public void Init(){
        // En lugar de hacer Serialize Field con los miembros y añadir manualmente desde el inspector,
        // buscamos las instancias de tipo MemberHUD del componente de forma dinámica
        members = GetComponentsInChildren<MemberHUD>();
    }

    public void SetTeamData(List<Pokemon> pkmns){
        // Esta función jala la información de los pokemon existentes en el Team del jugador
        this.pkmns = pkmns;

        for (int i = 0; i < members.Length; i++)
        {
            if (i < pkmns.Count){
                members[i].SetData(pkmns[i]);
            } else {
                //members[i].gameObject.SetActive(false);     // si se acaban los pokemon, deshabilitar el resto de las HUD
                members[i].SetEmpty();
            }
        }
        infoText.text = "Choose your fighter!";
    }

    public void UpdateTS(int selected){
        // Highlight with a color the name of current Selected Member
        for (int i = 0; i < pkmns.Count; i++)
        {
            if (i==selected){
                // highlight hud name
                members[i].HighlightName(true);
            } else {
                members[i].HighlightName(false);
            }
        }
    }

    public void SetInfoText(string text){
        infoText.text = text;
    }
}
