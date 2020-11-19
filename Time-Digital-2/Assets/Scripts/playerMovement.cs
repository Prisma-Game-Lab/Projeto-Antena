using UnityEngine;


public class playerMovement : MonoBehaviour
{
    public GameObject mainCam;
    public GameObject thirdPersonCam;
    public GameObject firstPersonCam;
    public SceneController sceneController;

    public float movementForce;
    public float movementSpeed;

    [HideInInspector]
    public bool isMoving;
    [HideInInspector]
    public bool isSafe;

    public static playerMovement current;

    //Velocidade de rotação do player
    private float turnSmoothTime=0.1f;
    private float turnSmoothVelocity;
    private Rigidbody playerRb;
    private bool thirdPersonMode;

    
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //Trava e deixa o cursor invisivel
        //Cursor.lockState = CursorLockMode.Locked;
        isMoving = false;
        isSafe = false;
        thirdPersonMode = true;

        thirdPersonCam.SetActive(thirdPersonMode);
        firstPersonCam.SetActive(!thirdPersonMode);

    }

    private void FixedUpdate()
    {
        Movement();
        if (!thirdPersonCam.activeSelf)
            transform.rotation = Quaternion.AngleAxis(mainCam.transform.rotation.eulerAngles.y * Time.fixedDeltaTime * 50, Vector3.up);
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

            //Transição de rotação suave
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            if(thirdPersonCam.activeSelf)
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Calcula direção baseada no angulo de rotação para ser uma direção relativa a camera
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Adiciona força no player
            playerRb.AddForce(moveDir * Time.deltaTime * movementForce * 1000f);
            //Se velocidade atual maior que velocidade maxima
            if (playerRb.velocity.magnitude >= movementSpeed)
            {
                //Velocidade atual é igual a máxima
                playerRb.velocity = playerRb.velocity.normalized * movementSpeed;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Colisao");
            sceneController.LoadScene(sceneController.currentScene.name);
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            Debug.Log("Entrou esconderijo");
            thirdPersonCam.SetActive(!thirdPersonMode);
            firstPersonCam.SetActive(thirdPersonMode);
            isSafe = true;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            Debug.Log("Saiu esconderijo");
            thirdPersonCam.SetActive(thirdPersonMode);
            firstPersonCam.SetActive(!thirdPersonMode);
            isSafe = false;
        }
    }
}
