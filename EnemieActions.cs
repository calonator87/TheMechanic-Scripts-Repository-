using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : MonoBehaviour
{
    private Shoot beingShot;
    public GameObject player;
    private EnemyDetection enemyEyes;
    public AudioSource audioSource;
    public AudioClip brutusHit;

    public bool isInTrigger = false;
    public bool coroutineRunning = false;

    public enum PossibleStates { PatrolState, AttackState }
    public PossibleStates state;

    public Transform[] waypoints;
    public int nextWaypoint = 0;

    public NavMeshAgent navigator;

    public float raycastCollisionSpeed = 6f;
    public float raycastNoCollisionSpeed = 2.5f;
    public float originalSpeed;

    public Animator animator;
    public string attackTrigger = "Attack";

    void Start()
    {
        navigator = GetComponent<NavMeshAgent>();
        navigator.destination = transform.position;
        animator = GetComponent<Animator>();
        enemyEyes = FindObjectOfType<EnemyDetection>();
        beingShot = FindObjectOfType<Shoot>();

        originalSpeed = navigator.speed;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        if (audioSource != null && brutusHit != null)
        {
            audioSource.clip = brutusHit;
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip not assigned in Inspector.");
        }
    }

    void Update()
    {
        DecideStateChange();

        switch (state)
        {
            case PossibleStates.PatrolState:
                PatrolState();
                break;
            case PossibleStates.AttackState:
                AttackState();
                break;
            default:
                break;
        }

        if (beingShot.enemyHit && !coroutineRunning)
        {
            StartCoroutine(ResetSpeed());
        }
    }

    void DecideStateChange()
    {
        state = enemyEyes.playerDetected ? PossibleStates.AttackState : PossibleStates.PatrolState;
    }

    void PatrolState()
    {
        if (!isInTrigger && !beingShot.enemyHit)
        {
            navigator.speed = raycastNoCollisionSpeed;
        }
        else
        {
            navigator.speed = 0f;
        }

        if (navigator.remainingDistance < 0.1f)
        {
            navigator.destination = waypoints[nextWaypoint].position;
            nextWaypoint = (nextWaypoint + 1) % waypoints.Length;
        }
    }

    void AttackState()
    {
        if (!isInTrigger && !beingShot.enemyHit)
        {
            navigator.speed = raycastCollisionSpeed;
        }
        else
        {
            navigator.speed = 0f;
        }

        navigator.destination = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            animator.SetBool(attackTrigger, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            animator.SetBool(attackTrigger, false);
        }
    }

    IEnumerator ResetSpeed()
    {
        coroutineRunning = true;
        Debug.Log("Speed reduced to 0");
        navigator.speed = 0f;

        if (animator != null)
        {
            animator.speed = 0f;
        }

        Renderer renderer = transform.Find("Body_Retopo")?.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.blue;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        yield return new WaitForSeconds(3);

        if (state == PossibleStates.AttackState)
        {
            navigator.speed = raycastCollisionSpeed;
            Debug.Log("Speed restored to " + raycastCollisionSpeed);
        }
        else
        {
            navigator.speed = originalSpeed;
            Debug.Log("Speed restored to " + originalSpeed);
        }

        beingShot.enemyHit = false;

        if (animator != null)
        {
            animator.speed = 1f;
        }

        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }

        coroutineRunning = false;
    }
}