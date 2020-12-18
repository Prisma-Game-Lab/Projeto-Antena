using UnityEngine;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject thirdPersonCam;
    public GameObject firstPersonCam;
    public GameObject endCameraPoint;
    public Light lanterna;
    public Light luzCabecaEsconderijo;
    public float safeSpotLightIntensity;
    public float movementSpeed;

    public AudioSource morte;
    public AudioSource safeSpot;
    public AudioSource butao;
    public AudioSource musicFinal;
    public AudioSource musicTema;

    //END OFF THE GAME
    [HideInInspector]
    public bool inTheEnd = false;

    //SAVE GAME 
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
    private float lanternaInicial;
    private bool thirdPersonMode;
    private int time;
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
        thirdPersonCam.SetActive(thirdPersonMode);
        firstPersonCam.SetActive(!thirdPersonMode);
        Physics.gravity *= 2;
        triggerCount = 0;
        lanternaInicial = lanterna.intensity;

        StartCoroutine(WaitForMusic());
    }

    public IEnumerator WaitForMusic()
    {
        yield return new WaitForSeconds(10.0f);
        AudioManager.sharedInstance.ChangeMusic(musicTema);
    }


    private void Update()
    {
        if (Input.GetKeyDown("e") && button)
        {
            print("pressionado");
            button.GetComponent<button>().buttonPressed = true;
            butao.Play();
        }
    }

    private void FixedUpdate()
    {
        if (enableMovement)
        {
            if (!isDead)
                if (inTheEnd)
                    MovementUp();
                else
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

    //USING FOR ENDO OF THE GAME 
    private void MovementUp()
    {
        isMoving = false;
        //Pega input horizontal e vertical
        bool goingUp = Input.GetKey("w");
        float vertical = 0.0f;

        if (goingUp)
        {
            vertical = 1.0f;
        }

        //Guarda o input em um vetor direção
        Vector3 direction = new Vector3(0f, 0f, -vertical).normalized;
        //Se o player esta se movendo
        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;
        }
        else
            direction = Vector3.zero;

        controller.SimpleMove(direction.normalized * Time.fixedDeltaTime * movementSpeed/2 * 10f);
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
            luzCabecaEsconderijo.enabled = true;
            lanterna.intensity = safeSpotLightIntensity;
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
            Player playerToSave = new Player();
            playerToSave.checkpointsCount = 1;
            playerToSave.position = collision.gameObject.transform.position;
            SaveSystem.SaveGame(playerToSave);
        }
        else if (collision.gameObject.CompareTag("button"))
        {
            print("botao em area");
            button = collision.gameObject;

        }
        else if (collision.gameObject.CompareTag("End"))
        {
            endCameraPoint.gameObject.transform.position = this.gameObject.transform.position;
            this.gameObject.transform.rotation = endCameraPoint.gameObject.transform.rotation;
            collision.GetComponent<TheEnd>().reachedTheEnd = true;
            thirdPersonCam.GetComponent<CinemachineFreeLook>().Follow = endCameraPoint.gameObject.transform;
            thirdPersonCam.GetComponent<CinemachineFreeLook>().LookAt = gameObject.transform;
            inTheEnd = true;

            AudioManager.sharedInstance.ChangeMusic(musicFinal);
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
                luzCabecaEsconderijo.enabled = false;
                lanterna.intensity = lanternaInicial;
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
