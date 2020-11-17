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
    //public string path = @"C:\Users\ericj\Documents\Unity\SavedGames\MyGame.bin";
    private string path;
    public int numOfTrainers;
    private TeamManager tmanager;
    
    
    void Start(){
        tmanager = GameObject.FindWithTag("GameController").GetComponent<TeamManager>();

        string [] pathNames = {Application.persistentDataPath, "PlayerLog.bin"} ; //  %userprofile%\AppData\Local\Packages\<productname>\LocalState // Alexis\AppData\Local\Packages\EinarMon2\LocalState
        path = Path.Combine(pathNames);
        // For debugging:
        int firstid = tmanager.Trainers[0].trainerId;
        bool firstbool = tmanager.Trainers[0].defeated;
        Debug.Log("First trainer: " + firstid + "  "+ firstbool);
    }

    public void Save(){
        Vector3 pos = GameObject.FindWithTag("Player").transform.position;     // guarda posición actual de player
        
        var player = GameObject.FindWithTag("Player");
        int id = player.GetComponent<Player>().teamId;
        
        //TeamManager tm = GameObject.FindWithTag("GameController").GetComponent<TeamManager>();

        bool[] trainersDefeated = new bool[numOfTrainers];        // will iterate ofer TeamManager's trainers array and check their defeated value, save it to an array
        
        Debug.Log("TM's Trainers count = " + tmanager.Trainers.Count);
        
        for (int i = 0; i < tmanager.Trainers.Count; i++)
        {
            Debug.Log("Cycle: " + i + " defeated:" + tmanager.Trainers[i]);
            trainersDefeated[i] = tmanager.Trainers[i].defeated;
        }

        PlayerLog plog = new PlayerLog(pos.x, pos.y, pos.z, id, trainersDefeated);
        
        IFormatter formatter = new BinaryFormatter();
        Debug.Log("Now Saving...");
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

        bool[] savedTrainers = savedLog.trainersDefeated;

        Debug.Log(savedTrainers.Length);
        for (int i = 0; i < savedTrainers.Length; i++)
        {
            Debug.Log("Loading defeated val" + i + ", " + savedTrainers[i]);
        }

        Debug.Log("Setting Trainers...");
        tmanager.SetTrainers(savedTrainers);
    }

}