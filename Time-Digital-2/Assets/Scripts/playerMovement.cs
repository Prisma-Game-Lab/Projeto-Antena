using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    

    private void Awake()
    {
        
    }

    private void Start()
    {
        //Trava e deixa o cursor invisivel
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement(){

        //Pega input horizontal e vertical
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Guarda o input em um vetor direção
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //Se o player esta se movendo
        if (direction.magnitude >= 0.1f)
        {
            //Calcula o angulo que o player precisa rotacionar, baseado na direção dos inputs
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            //Transição de rotação suave
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Calcula direção baseada no angulo de rotação para ser uma direção relativa a camera
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            //Movimenta o player somando moveDir na sua posição
            //transform.position += moveDir * Time.deltaTime * playerSpeed;
            if (rb.velocity.magnitude <= movementSpeed)
            {
                rb.AddForce(moveDir * Time.deltaTime * movementForce * 1000f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Colisao");
            sceneController.LoadScene(sceneController.currentScene.name);
        }
    }
}
