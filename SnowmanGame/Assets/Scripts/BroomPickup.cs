using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomPickup : MonoBehaviour
{
    //[SerializeField] private AudioSource SM_broomPickup;
    public bool isMeleeMode = false; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isMeleeMode = true;
            Destroy(gameObject);
            Debug.Log("melee");
            
        }
       // if (isMeleeMode == true)
        //{
         //   SM_broomPickup.Play();
        //}
    }
}
