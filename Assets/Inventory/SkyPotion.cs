﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyPotion : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    private void Start(){
    
    player= GameObject.FindGameObjectWithTag("Player").transform;
}

     public void TeleportPlayerIsland(){

     player.position= new Vector2(-39.50f,20.80f);
     Destroy(gameObject);      

     }
}
