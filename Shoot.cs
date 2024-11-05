using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shootClip;
    public bool enemyHit;

    private StaminaPlayer staminaPlayer;
    public Transform characterTransform;
    public float maxDistance = 100f;
    public float fireRate = 0.8f;
    public float groundHeight;

    public bool canShoot = true;
    public bool aiming = false;

    public InputAction shootAction;
    public InputAction aimAction;

    public string aimTrigger;

    public Animator anim;
    public LineRenderer lineRenderer;

    private UIManagement ui;

    void Start()
    {
        shootAction.Enable();
        aimAction.Enable();
        anim = GetComponent<Animator>();
        staminaPlayer = FindObjectOfType<StaminaPlayer>();
        ui = FindObjectOfType<UIManagement>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        if (audioSource != null && shootClip != null)
        {
            audioSource.clip = shootClip;
        }
    }

    void Update()
    {
        if (canShoot && shootAction.triggered && aiming && ui.currentRays > 0)
        {
            ui.currentRays--;
            ui.UpdateUIRays();
            Debug.Log("Shooting");
            StartCoroutine(ShootCoroutine());
        }

        float aimValue = aimAction.ReadValue<float>();
        if (aimValue == 1f && staminaPlayer.stamina > 0f)
        {
            anim.SetBool(aimTrigger, true);
            aiming = true;
        }
        else
        {
            aiming = false;
            anim.SetBool(aimTrigger, false);
            lineRenderer.enabled = false;
        }
    }

    IEnumerator ShootCoroutine()
    {
        canShoot = false;

        Vector3 raycastOrigin = characterTransform.position + Vector3.up * groundHeight;

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(raycastOrigin, characterTransform.forward, out hit, maxDistance);

        if (hitSomething)
        {
            Debug.Log("Hit: " + hit.collider.name);
            lineRenderer.SetPosition(0, raycastOrigin);
            lineRenderer.SetPosition(1, hit.point);
            lineRenderer.enabled = true;

            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit");
                enemyHit = true;
            }
        }
        else
        {
            Debug.Log("Missed.");
            lineRenderer.SetPosition(0, raycastOrigin);
            lineRenderer.SetPosition(1, raycastOrigin + characterTransform.forward * maxDistance);
            lineRenderer.enabled = true;
        }

        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip not assigned.");
        }

        yield return new WaitForSeconds(fireRate);

        lineRenderer.enabled = false;
        canShoot = true;
    }

    void OnDrawGizmos()
    {
        if (characterTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(characterTransform.position, characterTransform.forward * maxDistance);
        }
    }
}




































































/*{
    public Transform characterTransform; // Asigna el transform del personaje desde el Inspector
    public float maxDistance = 10f; // Distancia máxima del raycast
    public float fireRate = 1.5f; // Tasa de disparo en segundos

    public float alturaDelSuelo;

    private bool canShoot = true; // Variable para controlar si el personaje puede disparar
    public bool apuntando = false;

    public InputAction disparar;
    public InputAction apuntar;

    public string apunto;

    Animator anim;

    void Start()
    {
        disparar.Enable();
        apuntar.Enable();
        anim = GetComponent<Animator>();


    }
    void Update()
    {
        // Obtener la posición de origen del raycast con una altura específica
        Vector3 raycastOrigin = characterTransform.position + Vector3.up * alturaDelSuelo;

        if (canShoot && disparar.triggered && apuntando)
        {
            Debug.Log("disparo");
            StartCoroutine(Shoot(raycastOrigin)); // Pasa la posición de origen del raycast a la función Shoot
        }

        if (apuntar.ReadValue<float>() > 0f)
        {
            anim.SetBool(apunto, true);
            apuntando = true;
        }
        else
        {
            apuntando = false;
            anim.SetBool(apunto, false);
        }
    }

    IEnumerator Shoot(Vector3 origin)
    {
        canShoot = false;

        // Lanzar el raycast desde la posición ajustada
        RaycastHit hit;
        if (Physics.Raycast(origin, characterTransform.forward, out hit, maxDistance))
        {
            Debug.Log("Golpeado: " + hit.collider.name);
            // Realizar acciones adicionales cuando el raycast golpee algo
        }
        else
        {
            Debug.Log("No golpeado.");
            // Realizar acciones adicionales cuando el raycast no golpee nada
        }

        // Esperar antes de poder disparar nuevamente
        yield return new WaitForSeconds(fireRate);

        canShoot = true; // Activar la capacidad de disparo
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(characterTransform.position, characterTransform.forward * maxDistance);
    }


}*/
