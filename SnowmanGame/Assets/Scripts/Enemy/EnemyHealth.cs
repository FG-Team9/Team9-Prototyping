using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int scoreValue = 10;
    
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    int currentHealth;

    public int CurrentHealth { get { return currentHealth; } }

    void Awake ()
    {
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead) return;

        currentHealth -= amount;
            
        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;
        
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        
        capsuleCollider.isTrigger = true;

        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
        Debug.Log("is dead now thanks");
    }
}
