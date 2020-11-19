using UnityEngine;


public class playerMovement : MonoBehaviour
{
    public Transform cam;
    public float movementForce;
    public float movementSpeed;
    public Rigidbody rb;
    public SceneController sceneController;

    //Velocidade de rotação do player
    public float turnSmoothTime=0.1f;
    private float turnSmoothVelocity;
    
    [HideInInspector]
    public bool isMoving;

    public static playerMovement current;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //Trava e deixa o cursor invisivel
        //Cursor.lockState = CursorLockMode.Locked;+
        isMoving = false;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement(){
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
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            //Transição de rotação suave
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Calcula direção baseada no angulo de rotação para ser uma direção relativa a camera
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Adiciona força no player
            rb.AddForce(moveDir * Time.deltaTime * movementForce * 1000f);
            //Se velocidade atual maior que velocidade maxima
            if (rb.velocity.magnitude >= movementSpeed)
            {
                //Velocidade atual é igual a máxima
                rb.velocity = rb.velocity.normalized * movementSpeed;
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
}
