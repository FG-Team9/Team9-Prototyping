using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomScript : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    public int damagePerHit = 100;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Broom")
        {
            if(enemyHealth != null)
            {
                enemyHealth.TakeBroomDamage(damagePerHit);
            }
            Debug.Log("broom hit");
        }
    }
    
}
