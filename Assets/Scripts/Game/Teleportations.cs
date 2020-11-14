using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportations : MonoBehaviour
{
    public GameObject gym;
    private Vector2 posGym;
    // Start is called before the first frame update
    void Start()
    {
        posGym = gym.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        col.gameObject.transform.position= new Vector2(posGym.x,posGym.y);
    }
}
