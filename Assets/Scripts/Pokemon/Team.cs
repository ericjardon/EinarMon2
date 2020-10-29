using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Team : MonoBehaviour
{
    // Receive a list of wild pokemons
    [SerializeField] List <Pokemon> pokemons;

    public List<Pokemon> Pokemons {
        // Property de C#, en lugar de crear un método Get() explícito, solo llamamos .Pokemons y nos devuelve pokemons.
        get {
            return pokemons;
        }
    }

    private void Start(){
        foreach (var p in pokemons)
        {
            // Initialize all the wild pokemons
            p.Init();
        }
    }
    
    public Pokemon GetAlivePokemon(){
        // We are using the link dependency to get the HP and determine what pokemons are alive
        return pokemons.Where(x => x.HP > 0).FirstOrDefault();
        
    }
}
