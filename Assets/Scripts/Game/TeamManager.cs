using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] List <Team> teams;
    [SerializeField] List<Trainer> trainers;    // lista que tiene referencia a todos los trainers en el juego por orden de encuentro

    public List<Trainer> Trainers {
        // Property de C#, en lugar de crear un método Get() explícito, solo llamamos Team.Pokemons y nos devuelve pokemons.
        get {
            return trainers;
        }
        set {
            trainers = value;
        }
    }

    void Start(){
        seeTeam(0);
    }
    public List<Pokemon> getTeamPokemons(int id){
        // returns the list of pokemons according to id
        return teams[id].Pokemons;
    }

    public void seeTeam(int id){
        foreach (var p in teams[id].Pokemons){
            Debug.Log(p.pBase.GetPName);
        }
    }

    public void SetTrainers(bool[] trainersDefeated){
        // la idea es que reciba trainers defeated por parte del GameLoader cuando éste lo llame.
        // trainersDefeated y trainers deben tener misma longitud e índices correspondientes.
        for (int i = 0; i < trainers.Count; i++)
        {
            trainers[i].setDefeated(trainersDefeated[i]);
        }
    }

}
