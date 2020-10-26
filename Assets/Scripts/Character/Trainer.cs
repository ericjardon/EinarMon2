using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer : MonoBehaviour, Interactable
{
    public void Interact() {
        // this class implements the abstract method Interact
        Debug.Log("Interact with Trainer");
    }
}
