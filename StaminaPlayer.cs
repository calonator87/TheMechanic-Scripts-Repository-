using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class StaminaPlayer : MonoBehaviour
{
    public Image staminaBar;
    public float stamina, maxStamina;
    public float staminaCost;
    public float rechargeRate;

    private bool isRunning;

    public InputAction run;

    private Coroutine rechargeCoroutine;

    Shoot aiming;
    PlayerMovement playerMovement;
    public float originalSpeed;

    public string runningAnimation;
    Animator anim;

    void Start()
    {
        run.Enable();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        aiming = FindFirstObjectByType<Shoot>();
        anim = GetComponent<Animator>();

        originalSpeed = playerMovement.speed;
    }

    void Update()
    {
        ApplyStaminaWhileRunning();
        ApplyStaminaWhileAiming();
    }

    public void ApplyStaminaWhileAiming()
    {
        if (aiming.aiming)
        {
            stamina -= staminaCost * Time.deltaTime;
            if (stamina <= 0)
            {
                stamina = 0;
            }

            if (rechargeCoroutine != null) StopCoroutine(rechargeCoroutine);
            rechargeCoroutine = StartCoroutine(RechargeStamina());
            staminaBar.fillAmount = stamina / maxStamina;
        }
    }

    public void ApplyStaminaWhileRunning()
    {
        if (run.ReadValue<float>() == 1)
        {
            isRunning = true;
            if (rechargeCoroutine != null) StopCoroutine(rechargeCoroutine);
            rechargeCoroutine = StartCoroutine(RechargeStamina());
        }
        else if (run.ReadValue<float>() == 0)
        {
            isRunning = false;
        }

        if (isRunning)
        {
            anim.SetBool(runningAnimation, true);
            playerMovement.speed = playerMovement.maxSpeed;
            stamina -= staminaCost * Time.deltaTime;
            if (stamina <= 0)
            {
                playerMovement.speed = originalSpeed;
                stamina = 0;
                anim.SetBool(runningAnimation, false);
            }

            staminaBar.fillAmount = stamina / maxStamina;
        }

        if (!isRunning)
        {
            anim.SetBool(runningAnimation, false);
            playerMovement.speed = originalSpeed;
        }
    }

    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);
        while (stamina < maxStamina)
        {
            stamina += rechargeRate / 10f;
            if (stamina > maxStamina) stamina = maxStamina;
            staminaBar.fillAmount = stamina / maxStamina;
            yield return new WaitForSeconds(0.1f);
        }
    }
}