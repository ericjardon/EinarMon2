using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour, Interactable
{

    [SerializeField] DialogLines dialog;     // lo especificamos desde el Editor de Unity
    [SerializeField] DialogLines defeatDialog;
    public event Action<bool> OnStartBattle;
    public int trainerId;
    public bool defeated = false;

    public void Interact() {
        // this class implements the abstract method Interact
        if (!defeated){
            GameObject.FindWithTag("GameController").GetComponent<GameManager>().SetRival(this);
            StartCoroutine(DialogManager.Instance.DisplayDialog(dialog, true));
            // will trigger fight
        }
        else {
            StartCoroutine(DialogManager.Instance.DisplayDialog(defeatDialog, false));
            // if defeated, he will not trigger fight
        }
    }

    public void isDefeated() {
        defeated = true;
    }

    public void setDefeated(bool defeated){
        this.defeated = defeated;
    }

}
