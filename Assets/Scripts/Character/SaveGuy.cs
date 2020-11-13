using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGuy : MonoBehaviour, Interactable
{
    [SerializeField] DialogLines dialog;

    public void Interact(){
        
        StartCoroutine(DialogManager.Instance.DisplayDialog(dialog, false));
        Debug.Log("Hablaste con SaveGay");
        FindObjectOfType<GameLoader>().Save();
   }
    
}
