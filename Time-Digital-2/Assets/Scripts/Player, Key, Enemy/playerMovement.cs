using UnityEngine;


public class playerMovement : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject thirdPersonCam;
    public GameObject firstPersonCam;
    public Light safeSpotLight;
    public float flashLight_Intensity;
    public float movementSpeed;

    public AudioSource morte;
    public AudioSource safeSpot;

    public bool enableMovement = false; 

    [HideInInspector]
    public bool isMoving;
    [HideInInspector]
    public bool isSafe;
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public Vector3 lastCheckpointPos;
    [HideInInspector]
    public Quaternion lastCheckpointRot;
    [HideInInspector]
    public playerKeyHolder keys;
    [HideInInspector]
    public GameObject button;
    [HideInInspector]
    public Rigidbody playerRb;

    public static playerMovement current;

    //Velocidade de rotação do player
    private float turnSmoothTime=0.1f;
    private float turnSmoothVelocity;
    private bool thirdPersonMode;
    private int triggerCount;
    private CharacterController controller;
    private Vector3 moveDir;



    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        keys = GetComponent<playerKeyHolder>();
        playerRb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        lastCheckpointPos = transform.position;
        lastCheckpointRot = transform.rotation;
        //Trava e deixa o cursor invisivel
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        isMoving = false;
        isSafe = false;
        thirdPersonMode = true;
        isDead = false;
        Physics.gravity *= 2;
        thirdPersonCam.SetActive(thirdPersonMode);
        firstPersonCam.SetActive(!thirdPersonMode);
        triggerCount = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown("e") && button)
        {
            print("pressionado");
            button.GetComponent<button>().buttonPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (enableMovement)
        {
            if (!isDead)
                Movement();
            else
                transform.position = lastCheckpointPos;
        }
      
    }

    private void Movement()
    {
        isMoving = false;
        //Pega input horizontal e vertical
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Guarda o input em um vetor direção
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        //Se o player esta se movendo
        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;

            //Calcula o angulo que o player precisa rotacionar, baseado na direção dos inputs
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (thirdPersonCam.activeSelf)
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Calcula direção baseada no angulo de rotação para ser uma direção relativa a camera
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }else
            moveDir = Vector3.zero;
        controller.SimpleMove(moveDir.normalized * Time.fixedDeltaTime * movementSpeed * 10f);

        if (!thirdPersonCam.activeSelf)
            transform.rotation = Quaternion.AngleAxis(mainCam.transform.rotation.eulerAngles.y * Time.fixedDeltaTime * 50, Vector3.up);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            Debug.Log("Entrou esconderijo");
            thirdPersonCam.SetActive(!thirdPersonMode);
            firstPersonCam.SetActive(thirdPersonMode);
            isSafe = true;

            AudioManager.sharedInstance.PlayRequest(safeSpot, AudioManager.SoundType.SafeSpot);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            thirdPersonCam.SetActive(thirdPersonMode);
            firstPersonCam.SetActive(!thirdPersonMode);
            isSafe = false;

            AudioManager.sharedInstance.StopRequest(AudioManager.SoundType.SafeSpot);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            isSafe = true;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        //Entrou num esconderijo
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            triggerCount++;
            Debug.Log("Entrou esconderijo");
            safeSpotLight.enabled = true;
            safeSpotLight.intensity = flashLight_Intensity;
            thirdPersonCam.SetActive(!thirdPersonMode);
            firstPersonCam.SetActive(thirdPersonMode);
            isSafe = true;

            AudioManager.sharedInstance.PlayRequest(safeSpot, AudioManager.SoundType.SafeSpot);
        }
        //Foi atacado e morreu
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            //Morte
            AudioManager.sharedInstance.PlayRequest(morte, AudioManager.SoundType.Morte);
            isDead = true;
            //gameObject.GetComponent<ScannerGenerator>().canUseSonar = true;
        }
        //Encontrou um checkpoint
        else if (collision.gameObject.CompareTag("CheckPoint"))
        {
            lastCheckpointPos = collision.gameObject.transform.position;
            lastCheckpointRot = transform.rotation;
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            Player.SavePlayer();
        }
        else if (collision.gameObject.CompareTag("button"))
        {
            print("botao em area");
            button = collision.gameObject;

        }
    }
    private void OnTriggerExit(Collider collision)
    {
        //Saiu do esconderijo
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            triggerCount--;
            if (triggerCount <= 0)
            {
                safeSpotLight.enabled = false;
                thirdPersonCam.SetActive(thirdPersonMode);
                firstPersonCam.SetActive(!thirdPersonMode);
                isSafe = false;
                AudioManager.sharedInstance.StopRequest(AudioManager.SoundType.SafeSpot);
            }
        }
        else if (collision.gameObject.CompareTag("button"))
        {
            button = null;
        }
    }
}
