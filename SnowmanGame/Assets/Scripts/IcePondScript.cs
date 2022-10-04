using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePondScript : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    private Rigidbody playerRB;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRB = playerObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRB.mass = .25f;
            playerRB.drag = 0.1f;
            playerRB.angularDrag = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRB.mass = 1f;
            playerRB.drag = 3f;
            playerRB.angularDrag = 3f;
        }
    }
}
