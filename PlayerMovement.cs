using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Shoot aimAnimations;
    private PickUps1 pickupAnimations;
    public float speed;
    public float maxSpeed;

    public string walkAnimation;

    private Transform mainCameraTransform;

    Animator anim;

    void Start()
    {
        mainCameraTransform = Camera.main.transform;
        anim = GetComponent<Animator>();
        aimAnimations = FindFirstObjectByType<Shoot>();
        pickupAnimations = FindFirstObjectByType<PickUps1>();
    }

    public void Update()
    {
        if (aimAnimations.aiming)
        {
            speed = 0f;
        }

        if (pickupAnimations.isAnimating)
        {
            speed = 0f;
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = CalculateMovement(moveHorizontal, moveVertical);

        MovePlayerInterpolated(movement);

        if (movement != Vector3.zero)
        {
            RotateTowardsMovementDirection(movement);
            anim.SetBool(walkAnimation, true);
        }
        else
        {
            anim.SetBool(walkAnimation, false);
        }
    }

    Vector3 CalculateMovement(float horizontal, float vertical)
    {
        Vector3 cameraForward = Vector3.Scale(mainCameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = cameraForward * vertical + mainCameraTransform.right * horizontal;

        moveDirection.y = 0f;
        moveDirection.Normalize();

        return moveDirection;
    }

    void MovePlayerInterpolated(Vector3 movement)
    {
        Vector3 targetPosition = transform.position + (movement * speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.5f);
    }

    void RotateTowardsMovementDirection(Vector3 movementDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

        if (!IsRotating())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    bool IsRotating()
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward);
        float angle = Quaternion.Angle(currentRotation, targetRotation);
        return angle > 1f;
    }
}