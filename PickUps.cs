using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PickUps1 : MonoBehaviour
{

    public string sceneName;

    PlayerMovement velocidad;
    StaminaPlayer Original;
    public GameObject Object1ToPickUp;
    public GameObject Object2ToPickUp;
    public GameObject Object3ToPickUp;

    public GameObject Object1ToActivate;
    public GameObject Object2ToActivate;
    public GameObject Object3ToActivate;

    public bool objeto1 = false;
    public bool objeto2 = false;
    public bool objeto3 = false;

    public bool objetoNave1 = false;
    public bool objetoNave2 = false;
    public bool objetoNave3 = false;

    public bool activando1 = false;
    public bool isPickingUp = false;
    public bool isAnimating = false;



    public string animationName = "cogerobjetos";

    private Animator anim;

    UIManagement ui;

    public string recoger = "Recoger";

    public InputAction activator;

    private void Start()
    {
        // Initialize objects' active state
        SetObjectsInitialState();
        velocidad = FindFirstObjectByType<PlayerMovement>();
        Original = FindFirstObjectByType<StaminaPlayer>();
        activator.Enable();
        anim = GetComponent<Animator>();
        ui = FindFirstObjectByType<UIManagement>();



        objetoNave1 = false;
        objetoNave2 = false;
        objetoNave3 = false;


    }

    private void Update()
    {
        if (activando1 && isAnimating && !isPickingUp)
        {
            Debug.Log("Activar triggered");

            if (objeto1)
            {
                ActivateObject(ref objeto1, Object1ToActivate);
                objetoNave1 = true;


            }
            else if (objeto2)
            {
                ActivateObject(ref objeto2, Object2ToActivate);
                objetoNave2 = true;

            }
            else if (objeto3)
            {
                ActivateObject(ref objeto3, Object3ToActivate);
                objetoNave3 = true;

            }
        }


        if (isPickingUp && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            isPickingUp = false;
            velocidad.speed = Original.originalSpeed;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            isAnimating = true;
        }
        else
        {
            isAnimating = false;
        }



    }

    private void FixedUpdate()
    {
        animacion();
    }

    private void OnTriggerStay(Collider other)
    {

        if (isAnimating && !isPickingUp)
        {
            if (other.CompareTag("Objeto1") && !objeto2 && !objeto3)
            {
                PickUpObject(ref objeto1, other.gameObject);
            }
            else if (other.CompareTag("Objeto2") && !objeto1 && !objeto3)
            {
                PickUpObject(ref objeto2, other.gameObject);
            }
            else if (other.CompareTag("Objeto3") && !objeto1 && !objeto2)
            {
                PickUpObject(ref objeto3, other.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Activation") && (objeto1 || objeto2 || objeto3))
        {
            Debug.Log("Activation area entered");
            activando1 = true;
        }


        if (other.CompareTag("Salir") && objetoNave1 && objetoNave2 && objetoNave3)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Activation"))
        {
            Debug.Log("Activation area exited");
            activando1 = false;
        }
    }

    private void PickUpObject(ref bool objeto, GameObject objectToPickUp)
    {
        objeto = true;
        objectToPickUp.SetActive(false);
        Debug.Log($"{objectToPickUp.name} picked up");



    }

    private void ActivateObject(ref bool objeto, GameObject objectToActivate)
    {
        objeto = false;
        objectToActivate.SetActive(true);
        Debug.Log("Activating object: " + objectToActivate.name);
    }

    public void animacion()
    {
        if (!isPickingUp)
        {
            float activatorValue = activator.ReadValue<float>();
            if (activatorValue > 0f)
            {
                anim.SetBool(recoger, true);

            }
            else if (activatorValue == 0f)
            {
                anim.SetBool(recoger, false);
                velocidad.speed = Original.originalSpeed;
            }
        }
    }

    private void SetObjectsInitialState()
    {
        Object1ToActivate.SetActive(false);
        Object1ToPickUp.SetActive(true);

        Object2ToActivate.SetActive(false);
        Object2ToPickUp.SetActive(true);

        Object3ToActivate.SetActive(false);
        Object3ToPickUp.SetActive(true);
    }


}