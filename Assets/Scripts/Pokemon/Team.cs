﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Team : MonoBehaviour
{
    // Receive a list of wild pokemons
    [SerializeField] List <Pokemon> pokemons;

    public List<Pokemon> Pokemons {
        // Property de C#, en lugar de crear un método Get() explícito, solo llamamos Team.Pokemons y nos devuelve pokemons.
        get {
            return pokemons;
        }
        set {
            pokemons = value;
        }
        // poner un setter
    }
    
    public void Start(){
        foreach (var p in pokemons)
        {
            // Initialize all pokemons: set HP, moves, level
            p.Init();
        }
    }
    
    public Pokemon GetAlivePokemon(){
        // We are using the system.linq dependency to query the first pokemons who is not fainted
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();  
        // if no more pokemons alive, returns null
    }

    public void ReceivePokemon(Pokemon pkmn){
        
        int newId = gameObject.GetComponent<Player>().teamId + 1;
        Debug.Log(newId + ", past Id" + gameObject.GetComponent<Player>().teamId);
        gameObject.GetComponent<Player>().teamId = newId;

        Debug.Log("Changing Team Id:" + gameObject.GetComponent<Player>().teamId);

        Debug.Log("Player receiving Pokemon...");
        if (pokemons.Count < 6){
            pkmn.Init();
            this.pokemons.Add(pkmn);
        } else {
            Debug.Log("Your Einarmon Team is full!");
        }

    }
}