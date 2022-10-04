using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShitKidScript : MonoBehaviour
{

    private Animator myAnim;

    private int randomIdle = 0;

    private int randomReact = 0;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = this.GetComponentInChildren<Animator>();
        randomIdle = Random.Range(1, 6);
        randomReact = Random.Range(1, 3);
        
        myAnim.SetInteger("IdleInt", randomIdle);
        Debug.Log(myAnim);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee"))
        {
            if (true)
            {
                myAnim.SetTrigger("DieTrigger");
                Debug.Log("KNOCKED THE FUCK OUT");
                Debug.Log(randomReact);
            }
        } else if (other.CompareTag("Player"))
        {
            myAnim.SetTrigger("ShoveTrigger");
        }
        
        
    }
    
    /*
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Melee"))
        {
            myAnim.SetInteger("ReactInt", randomReact);
            Debug.Log("KNOCKED THE FUCK OUT");
        }
    }
    */
}
