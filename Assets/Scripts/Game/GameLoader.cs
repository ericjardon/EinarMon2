using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public struct Einarmon {
    // un esquema que abstrae lo esencial del makerBlock: su posición e id.
    public int id;

    public Einarmon(int id) {
        this.id = id;
    }
}

[System.Serializable]
public struct PlayerPos {
    public float x;
    public float y;
    public float z;

    public PlayerPos(float x, float y, float z){
        this.x = x;
        this.y = y;
        this.z = z;
    }
}


public class GameLoader : MonoBehaviour
{
    public GameObject playerPrefab;
    public string path = @"C:\Users\ericj\Documents\Unity\SavedGames\MyGame.bin";     
    // TODO: accesible on Save pero no on LOAD
    
    public Transform playerPosition;    

    public int playerTeam;
    
    
    public void Save() {
        Vector3 pos = GameObject.FindWithTag("Player").transform.position;     // guarda posición actual de player
        
        //playerPokemons = FindWithTag("Player").GetComponent<Team>();

        PlayerPos position = new PlayerPos(pos.x, pos.y, pos.z);
        
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path, 
                                        FileMode.Create,  
                                        FileAccess.Write, 
                                        FileShare.None);
        formatter.Serialize(stream, position);
        stream.Close();
        // se guardan los datos en un archivo
    }

    public void Load(){
        
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path, 
                                        FileMode.Open,  
                                        FileAccess.Read, 
                                        FileShare.Read);
        
        var savedPos = (PlayerPos)formatter.Deserialize(stream);
        stream.Close();
        // cargar el archivo y leer los objetos de tipo Block que contiene
        GameObject player = GameObject.FindWithTag("Player");
        player.transform.position = new Vector3(savedPos.x, savedPos.y, savedPos.z);
        
        // instantiate the prefab indicated by the Block object's id

    }

    public void Erase(){
        // borra a los objetos anteriores antes de ser guardados. 
        GameObject player = GameObject.FindWithTag("Player");
        Destroy(player.gameObject);
    }

}