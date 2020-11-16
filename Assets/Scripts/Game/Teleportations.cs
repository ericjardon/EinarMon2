using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportations : MonoBehaviour, Interactable
{
    
    [SerializeField] DialogLines dialog;
    public float gymPosx;
    public float gymPosy;
    public float time;


    //GYM1 POSX = -54.52
    //GYM2 POSY = 65.56
    //time 2.1

    //Outside GYM x-5.49
    //Outside GYM Y-0.23
    //1.5


    // public bool givesPokemon;
   
        public void Interact(){
       
           StartCoroutine(DialogManager.Instance.DisplayDialog(dialog, false));

      
           Invoke("Teleport",time);
   

           
   }


   public void Teleport(){
        var playerGo = GameObject.FindWithTag("Player");
            playerGo.transform.position=new Vector2(gymPosx, gymPosy);
}
}
