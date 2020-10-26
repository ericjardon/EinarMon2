using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour, Interactable
{

    [SerializeField] DialogLines dialog;     // lo especificamos desde el Editor de Unity

    public void Interact() {
        // this class implements the abstract method Interact
        // dialog box en el mapa: frase que empieza batalla
        StartCoroutine(DialogManager.Instance.DisplayDialog(dialog));

        // comienza batalla
    }
}
