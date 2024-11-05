using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Damage : MonoBehaviour
{
    public Image healthBar;
    public int pedroHealth = 100;
    public PlayerHealth health;
    UIManagement uiHealth;
    EnemyActions enemy;
    private bool isDamaged = false;

    private void Start()
    {
        uiHealth = FindObjectOfType<UIManagement>();
        enemy = FindFirstObjectByType<EnemyActions>();
    }

    private void Update()
    {

    }

    public void TakeDamage()
    {
        uiHealth.SetHealth(0);
        if (pedroHealth <= 0)
        {
            SceneManager.LoadScene("GAMEOVERDEF");
        }
    }

    // Simulation of taking damage (for example, it can be called from another script or event)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isDamaged && enemy.isInTrigger)
        {
            isDamaged = true;
            StartCoroutine(TakeDamageInAnimation());

            pedroHealth = 0;
        }
        else
        {
            isDamaged = false;
        }
    }

    private IEnumerator TakeDamageInAnimation()
    {
        // Get the total duration of the animation
        AnimatorStateInfo currentState = enemy.animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = currentState.length / 3;

        // Wait until the end of the animation
        yield return new WaitForSeconds(animationDuration);

        // Check if the damage flag is still active
        if (isDamaged)
        {
            // Call the function to take damage
            TakeDamage();
            Debug.Log("DAMAGE RECEIVED");
        }
    }
}