using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, Interactable
{
    [SerializeField] DialogLines giftDialog;
    [SerializeField] DialogLines dialog;
    // public bool givesPokemon;
    [SerializeField] Pokemon GiftPokemon;
    private Team playerTeam;     
   
   public void Interact(){
       if (GiftPokemon != null){
           StartCoroutine(DialogManager.Instance.DisplayDialog(giftDialog, false));
           Debug.Log("NPC giving pokemon...");
           GameObject.FindGameObjectWithTag ("Player").GetComponent<Team>().ReceivePokemon(GiftPokemon);
           GiftPokemon = null;
       } else {
           StartCoroutine(DialogManager.Instance.DisplayDialog(dialog, false));
       }
   }
}
