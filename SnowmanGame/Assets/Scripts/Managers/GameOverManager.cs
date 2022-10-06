using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    private bool isDead = false;

	Canvas canvas;
	
	void Start()
	{
		canvas = GetComponent<Canvas>();
	}

	void Update()
	{
		if (playerHealth.CurrentHealth <= 0 && !isDead)
		{
			GameOver();
			isDead = true;
		}
	}
	
    void GameOver()
    {
	    canvas.enabled = !canvas.enabled;
	    Time.timeScale = 0;
    }
}
