using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    [SerializeField] Text dialogT;
    [SerializeField] int letterPS;
    [SerializeField] Color colorH;
    [SerializeField] GameObject actionS;
    [SerializeField] GameObject moveS;
    [SerializeField] GameObject moveD;

    [SerializeField] List<Text> actionTxt;
    [SerializeField] List<Text> moveTxt;
    [SerializeField] Text PowerPointsTxt;
    [SerializeField] Text typeTxt;



    public void SetDialog(string dialog){
        dialogT.text=dialog;
    }

    public IEnumerator TD(string dialog){
        dialogT.text="";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogT.text+=letter;
            yield return new WaitForSeconds(1f/letterPS);
        }
    }

    public void ShowDT(bool enabled){
        dialogT.enabled=enabled;
    }

    public void ShowAS(bool enabled){
        actionS.SetActive(enabled);
    }

    public void ShowMS(bool enabled){
        moveS.SetActive(enabled);
        moveD.SetActive(enabled);
    }
    public void UpdateMS(int selectedMove, Move details){
        for(int i=0; i<moveTxt.Count; ++i){
            if(i==selectedMove)
                moveTxt[i].color= colorH;
            else
                moveTxt[i].color= Color.black;
        }
        PowerPointsTxt.text=$"PP {details.PP}/{details.Base.GetPP()}";
        typeTxt.text= details.Base.GetType().ToString();
    }
    public void UpdateAS(int selecedAction){
        for(int i=0; i<actionTxt.Count; ++i){
            if(i==selecedAction)
                actionTxt[i].color= colorH;
            else
                actionTxt[i].color= Color.black;
        }
    }

    public void SetMoveNames(List<Move> moves){
        for(int i=0; i<moveTxt.Count; ++i){
            if(i<moves.Count){
                moveTxt[i].text= moves[i].Base.GetMoveName();
            }
            else{
                moveTxt[i].text= "" ;
            }
                
        }
    }
}
