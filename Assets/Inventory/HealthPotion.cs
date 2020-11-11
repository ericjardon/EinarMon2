using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    private void Start(){
    
    player= GameObject.FindGameObjectWithTag("Player");
}

     public void RestorePokemon(){
        player.GetComponent<Team>().Start();
        Destroy(gameObject);   
     }
}
//Test