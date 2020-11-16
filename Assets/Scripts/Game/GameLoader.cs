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
    public bool[] trainersDefeated;

    public PlayerLog(float x, float y, float z, int teamId, bool[] trainersDefeated){
        this.x = x;
        this.y = y;
        this.z = z;
        this.id = teamId;
        this.trainersDefeated = trainersDefeated;
    }
}


public class GameLoader : MonoBehaviour
{
    //public TeamManager tm;      // set the object from the inspector
    public string path = @"C:\Users\ericj\Documents\Unity\SavedGames\MyGame.bin";
    public int numOfTrainers = 2;


    public void Save(){
        Vector3 pos = GameObject.FindWithTag("Player").transform.position;     // guarda posición actual de player
        
        var player = GameObject.FindWithTag("Player");
        int id = player.GetComponent<Player>().teamId;
        
        TeamManager tm = GameObject.FindWithTag("GameController").GetComponent<TeamManager>();

        bool[] trainersDefeated = new bool[numOfTrainers];        // will iterate ofer TeamManager's trainers array and check their defeated value, save it to an array
        Debug.Log("Trainers in TM = " + tm.Trainers.Count);
        for (int i = 0; i < numOfTrainers; i++)
        {
            trainersDefeated[i] = tm.Trainers[i].defeated;
        }


        PlayerLog plog = new PlayerLog(pos.x, pos.y, pos.z, id, trainersDefeated);
        
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
 
    }

}