using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    private void Start(){
    
    player= GameObject.FindGameObjectWithTag("Player").transform;
}

     public void TeleportPlayerStart(){

     player.position= new Vector2(0,0);
     Destroy(gameObject);      

     }
}
//Test