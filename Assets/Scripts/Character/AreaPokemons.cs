using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaPokemons : MonoBehaviour
{
    [SerializeField] List<Pokemon> wildPokemon;

    public Pokemon GetRandomPkmn(){
        print(UnityEngine.Random.Range(0,wildPokemon.Count));
        var wildP = wildPokemon[UnityEngine.Random.Range(0,wildPokemon.Count)];
        wildP.Init();
        return wildP;
    }
}
