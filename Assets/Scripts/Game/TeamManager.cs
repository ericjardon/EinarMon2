using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] List <Team> teams;

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

}
