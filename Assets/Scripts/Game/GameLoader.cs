using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public struct PlayerLog {
    public float x;
    public float y;
    public float z;
    public int id;

    public PlayerLog(float x, float y, float z, int teamId){
        this.x = x;
        this.y = y;
        this.z = z;
        this.id = teamId;
    }
}


public class GameLoader : MonoBehaviour
{
    public GameObject playerPrefab;
    public string path = @"C:\Users\ericj\Documents\Unity\SavedGames\MyGame.bin";     
    // TODO: accesible on Save pero no on LOAD
    
    public void Save(){
        Vector3 pos = GameObject.FindWithTag("Player").transform.position;     // guarda posición actual de player
        
        var player = GameObject.FindWithTag("Player");
        int id = player.GetComponent<Player>().teamId;

        PlayerLog plog = new PlayerLog(pos.x, pos.y, pos.z, id);
        
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path, 
                                        FileMode.Create,  
                                        FileAccess.Write, 
                                        FileShare.None);
        formatter.Serialize(stream, plog);
        stream.Close();
        // se guardan los datos en un archivo
    }

    public void Load(){
        
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(path, 
                                        FileMode.Open,  
                                        FileAccess.Read, 
                                        FileShare.Read);
        
        var savedLog = (PlayerLog)formatter.Deserialize(stream);
        stream.Close();
        // cargar el archivo y leer los objetos de tipo Block que contiene
        
        GameObject playerObj = GameObject.FindWithTag("Player");
        Player player = playerObj.GetComponent<Player>();
        
        playerObj.transform.position = new Vector3(savedLog.x, savedLog.y, savedLog.z);
        player.teamId =savedLog.id;
        player.LoadTeam();
        
        // instantiate the prefab indicated by the Block object's id

    }

    public void Erase(){
        // borra a los objetos anteriores antes de ser guardados. 
        GameObject player = GameObject.FindWithTag("Player");
        Destroy(player.gameObject);
    }

}