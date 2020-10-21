using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject health;

    public void SetHP(float hpNormalized){
        health.transform.localScale= new Vector3(hpNormalized, 1f);
    }

    public IEnumerator AnimateHPBar(float HP){
        float currtentHP= health.transform.localScale.x;
        float difference= currtentHP- HP;

        while (currtentHP - HP > Mathf.Epsilon){
            currtentHP -= difference * Time.deltaTime;
            health.transform.localScale= new Vector3(currtentHP, 1f);
            yield return null;
        }
        health.transform.localScale= new Vector3(HP, 1f);
    }
    
}
