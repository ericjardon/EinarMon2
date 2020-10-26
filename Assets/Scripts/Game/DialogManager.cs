using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPS;

    public event Action OnStartDialog;
    public event Action OnEndDialog;

    // Singleton paradigm: de esta forma muchos otros script pueden llamar al DialogManager.
    // Solo hay un dialogmanager para todo el juego
    public static DialogManager Instance {get; private set;}

    int currLine = 0;
    DialogLines theDialog;
    bool isTalking;

    public void Awake() {
        // se llama antes de Start. Asignamos a la variable estática esta única instancia.
        Instance = this;
    }

    public IEnumerator DisplayDialog(DialogLines dialog){
        // takes in a listof string to show sequencially in the dialog Box

        yield return new WaitForEndOfFrame();       
        // pausar y esperar 1 frame para no generar errores con HandleUpdate,
        // ya que el usuario está presionando la tecla Espacio en el mismo frame
        // al llamar a una corrutina, la función que la llama se covnierte en corrutina también.

        OnStartDialog?.Invoke();

        this.theDialog = dialog;

        dialogBox.SetActive(true);
        StartCoroutine(GenerateText(dialog.Lines[0]));
    }

    public IEnumerator GenerateText(string line){
        isTalking = true;
        dialogText.text="";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text+=letter;
            yield return new WaitForSeconds(1f/lettersPS);
        }

        isTalking = false;
    }

    public void HandleUpdate(){
        if (Input.GetKeyDown(KeyCode.Space) && !isTalking){
            ++currLine;

            if (currLine < theDialog.Lines.Count){
                // quedan líneas por mostrar
                StartCoroutine(GenerateText(theDialog.Lines[currLine]));
            }
            else {
                currLine = 0;       // reiniciar índice de línea de diálogo
                dialogBox.SetActive(false);     // Dejar de mostrar el dialogBox
                OnEndDialog?.Invoke();      // emitir evento OnEndDialog al GameManager
            }
        }
    }

}
