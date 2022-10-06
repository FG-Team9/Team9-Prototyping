using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomPickup : MonoBehaviour
{ 
    public bool isMeleeMode = false; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isMeleeMode = true;
            Destroy(gameObject);
            Debug.Log("melee");
        }
    }
}
