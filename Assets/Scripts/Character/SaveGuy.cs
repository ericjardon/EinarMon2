using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGuy : MonoBehaviour, Interactable
{
    [SerializeField] DialogLines dialog;

    public void Interact(){
        
        StartCoroutine(DialogManager.Instance.DisplayDialog(dialog, false));
        FindObjectOfType<GameLoader>().Save();
   }
    
}
