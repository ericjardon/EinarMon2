using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamScreen : MonoBehaviour
{
    [SerializeField] Text infoText;

    MemberHUD[] members;

    public void Init(){
        // En lugar de hacer Serialize Field con los miembros y añadir manualmente desde el inspector,
        // buscamos las instancias de tipo MemberHUD del componente de forma dinámica
        members = GetComponentsInChildren<MemberHUD>();
    }

    // Esta función jala la información de los pokemon en el Team del jugador.
    // Solo mostrará la información de los pokemon con los que se cuenta, incluso si son menos de 6
    public void SetTeamData(List<Pokemon> pkmns){
        for (int i = 0; i < members.Length; i++)
        {
            if (i < pkmns.Count){
                members[i].SetData(pkmns[i]);
            } else {
                members[i].gameObject.SetActive(false);     // si se acaban los pokemon, deshabilitar el resto de las HUD
            }
        }
        infoText.text = "Choose your fighter!";
    }
}
