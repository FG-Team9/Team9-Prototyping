using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePondScript : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    private Rigidbody playerRB;
    public float iceMass = .25f;
    public float iceDrag = .1f;
    public float iceAngularDrag = 0f;

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
            playerRB.mass = iceMass;
            playerRB.drag = iceDrag;
            playerRB.angularDrag = iceAngularDrag;
            Debug.Log("Im on the ice");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerRB.mass = 10f;
            playerRB.drag = 1f;
            playerRB.angularDrag = 3f;
        }
    }
}
