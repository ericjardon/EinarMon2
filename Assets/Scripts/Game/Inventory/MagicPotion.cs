using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPotion : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    private void Start(){
    
    player= GameObject.FindGameObjectWithTag("Player").transform;
}

     public void TeleportPlayerTrainer(){

     player.position= new Vector2(0.5f,80f);
     Destroy(gameObject);      

     }
}
