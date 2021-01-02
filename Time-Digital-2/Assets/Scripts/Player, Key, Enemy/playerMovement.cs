using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class playerMovement : MonoBehaviour
{
    public GameObject mainCam;
    public float safeSpotLightIntensity;
    public float movementSpeed;

    public AudioSource butao;

    //END OFF THE GAME
    [HideInInspector]
    public bool inTheEnd = false;

    [HideInInspector]
    public bool isMoving;
    [HideInInspector]
    public bool isSafe;
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public bool isStarting;
    [HideInInspector]
    public Vector3 lastCheckpointPos;
    [HideInInspector]
    public Quaternion lastCheckpointRot;
    [HideInInspector]
    public playerKeyHolder keys;
    [HideInInspector]
    public Rigidbody playerRb;
    [HideInInspector]
    public GameObject button;

    public static playerMovement current;

    //Velocidade de rotação do player
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private Vector3 moveDir;
    private CharacterController controller;
    private TriggerDetection tDetection;
    private CharacterController playerController;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerController.enabled = false;
        tDetection = GetComponent<TriggerDetection>();
        keys = GetComponent<playerKeyHolder>();
        playerRb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        isMoving = false;
        isSafe = false;
        isDead = false;
        isStarting = true;
        Physics.gravity *= 2;
        StartCoroutine(WaitForMusic());
        if (ES3.KeyExists("posicao") && ES3.KeyExists("energia"))
        {
            lastCheckpointPos = ES3.Load<Vector3>("posicao");
            lastCheckpointRot = ES3.Load<Quaternion>("rotacao");
            Manager.current.turnOff = ES3.Load<bool>("energia");
            transform.position = lastCheckpointPos;
            transform.rotation = lastCheckpointRot;
        }
        else
        {
            lastCheckpointPos = transform.position;
            lastCheckpointRot = transform.rotation;
        }
        playerController.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown("e") && button)
        {
            button.GetComponent<button>().buttonPressed = true;
            butao.Play();
        }
    }

    private void FixedUpdate()
    {
        //print("posicao fix antes " + transform.position);
        if (!isStarting)
        {
            if (!isDead)
                if (inTheEnd)
                    MovementUp();
                else
                    Movement();
            else
                transform.position = lastCheckpointPos;
        }
        //print("posicao fix dps " + transform.position);
    }

    private void Movement()
    {
        isMoving = false;
        //Pega inputs
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Guarda os inputs em um vetor direção
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        //Se o player esta se movendo
        if (direction.magnitude >= 0.1f)
        {
            isMoving = true;

            //Calcula o angulo que o player precisa rotacionar, baseado na direção dos inputs
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            if (tDetection.thirdPersonCam.activeSelf)
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Calcula direção baseada no angulo de rotação para ser uma direção relativa a camera
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
            moveDir = Vector3.zero;

        //Checa se esta no chao ou nao e manipula gravidade de acordo
        RaycastHit hit;
        float distance = 0.4f;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            Physics.gravity = new Vector3(0, -78.4f, 0);
            //Debug.DrawRay(transform.position, Vector3.down * distance, Color.green);
        }
        else
        {
            Physics.gravity = new Vector3(0, -19.6f, 0);
            //Debug.DrawRay(transform.position, Vector3.down * distance, Color.red);
        }

        controller.SimpleMove(moveDir.normalized * Time.fixedDeltaTime * movementSpeed * 10f);

        if (!tDetection.thirdPersonCam.activeSelf)
            transform.rotation = Quaternion.AngleAxis(mainCam.transform.rotation.eulerAngles.y * Time.fixedDeltaTime * 50, Vector3.up);
    }

    //USING FOR END OF THE GAME 
    private void MovementUp()
    {
        isMoving = false;
        //Pega input vertical
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
        controller.SimpleMove(direction.normalized * Time.fixedDeltaTime * movementSpeed / 2 * 10f);
    }

    public IEnumerator WaitForMusic()
    {
        yield return new WaitForSeconds(20.0f);
        AudioManager.sharedInstance.ChangeMusic(AudioManager.MusicType.Tema);
    }
}
