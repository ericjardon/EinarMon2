using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour, Interactable
{

    [SerializeField] DialogLines dialog;     // lo especificamos desde el Editor de Unity
    [SerializeField] DialogLines defeatDialog;
    public event Action<bool> OnStartBattle;
    private bool notBattled = true;

    public void Interact() {
        // this class implements the abstract method Interact
        // dialog box en el mapa: frase que empieza batalla
        if (notBattled){
            StartCoroutine(DialogManager.Instance.DisplayDialog(dialog, true));
            // will trigger fight
        }
        else {
            StartCoroutine(DialogManager.Instance.DisplayDialog(defeatDialog, false));
            // will not trigger fight
        }
    }

    public void isDefeated() {
        notBattled = false;
    }

}
