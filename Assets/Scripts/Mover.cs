using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mover : MonoBehaviour
{

    public Camera camera1;
    public Camera camera2;

    public bool boolCamera;

    public ParticleSystem lanza;
    public ParticleSystem lanza2;
    public ParticleSystem lanza3;

    [SerializeField]
    private float rotationSpeed = 10f;

    [SerializeField]
    private float maxMoveSpeed = 15f;

    [SerializeField]
    private float maxMoveSpeedBack = 5f;

    [SerializeField]
    float speed = 0f;

    [SerializeField]
    float acceleration = 0.6f;

    [SerializeField]
    private int playerIndex = 0;

    Rigidbody rb;

    public Image speedBar;

    //private Vector3 moveDirection = Vector3.zero;

    public Vector2 inputVector = Vector2.zero;

    public bool corriendo, retroceder;

    public bool  chocar;

    //public Animator animator;

    private static Temporizador temporizador;

    public bool pasarTempo;

    private bool meta=false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
      //  animator = GetComponent<Animator>();
       
    }




    private void Start()
    {
        Cursor.visible = false;
        temporizador = GetComponentInParent<Temporizador>();
        lanza.Stop();
        lanza2.Stop();
        lanza3.Stop();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "CheckPoint")
        {
            Debug.Log(this.gameObject.name + " PASO");
            meta = true;
            
           
        }


        if (other.name == "Final" && meta) {

            if (this.gameObject.name == "Jugador 1") {
                Debug.Log(this.gameObject.name + " MOLA MUCHO");
                SceneManager.LoadSceneAsync(5);
            }
            else if (this.gameObject.name == "Jugador 2") {
                Debug.Log(this.gameObject.name + " ERES GENIAL");
                SceneManager.LoadSceneAsync(6);
            }

            
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Muro")
        {
            chocar = true;
            Debug.Log("CHOCANDO ");
        }

        if (collision.collider.tag == "Suelo")
        {

            if ((transform.eulerAngles.x >= 80 && transform.eulerAngles.x <= 180) || (transform.eulerAngles.z >= 80 && transform.eulerAngles.z <= 180))
            {
                Debug.Log("SUELO");

                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }

            if ((transform.eulerAngles.x >= -80 && transform.eulerAngles.x <= -180) || (transform.eulerAngles.z >= -80 && transform.eulerAngles.z <= -180))
            {
                Debug.Log("SUELO");

                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }

        }
        

    }






    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }

    public void SetInputRun(bool IsRunnin)
    {
        if (IsRunnin == true)
        {
            corriendo = true;
        }
        else
        {
            corriendo = false;
        }


    }

    public void SetInputBack(bool IsRunnin)
    {
        if (IsRunnin == true)
        {
            retroceder = true;
        }
        else
        {
            retroceder = false;
        }


    }



    public void SetCamera(bool atras)
    {
        boolCamera = atras;
    }



    void Update()
    {
        pasarTempo = temporizador.tempo;
        if (temporizador.tempo)
        {

            if (boolCamera)
            {
                camera2.enabled = true;
                camera1.enabled = false;
            }
            else
            {
                camera2.enabled = false;
                camera1.enabled = true;
            }


            if (chocar)
            {
                speed = speed / 2.7f;
                chocar = false;
            }


            /*
            float rotationAmount = inputVector.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, rotationAmount);
            */

            if (speed != 0)
            {
                /*
                if (inputVector.x>0f &&corriendo )
                {
                    animator.SetBool("VuD", false);
                    animator.SetBool("GiD",true);
                }
                else if (inputVector.x <1f && corriendo) 
                {
                    animator.SetBool("GiD", false);
                    animator.SetBool("VuD", true);
                }
                else
                {
                    animator.SetBool("GiD", false);
                }
                */

                this.transform.Rotate(Vector3.up * speed * inputVector.x * Time.deltaTime * rotationSpeed);
            }

            if (corriendo == true  /*&& chocar==false|| InputManager.instance.playerControls.PlayerMovement.RumbleAction.WasPressedThisFrame()*/)
            {

                lanza.Play();
                lanza2.Play();
                lanza3.Play();

                if (speed < maxMoveSpeed)
                {
                    speed += acceleration * Time.deltaTime;

                }

                this.transform.Translate(Vector3.forward * speed * Time.deltaTime);

                //transform.position.x = transform.position.x + speed * Time.deltaTime;

                //this.transform.Translate(Vector3.forward * maxMoveSpeed * Time.deltaTime);
            }
            else
            {
                lanza.Stop();
                lanza2.Stop();
                lanza3.Stop();
                if (speed >= 0)
                {
                    speed -= 3 * acceleration * Time.deltaTime;
                    this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
                }
            }

            if (retroceder == true)
            {

                this.transform.Translate(-Vector3.forward * maxMoveSpeedBack * Time.deltaTime);
                this.transform.Rotate(Vector3.up * maxMoveSpeedBack * inputVector.x * Time.deltaTime * rotationSpeed);
            }

            try
            {
                speedBar.fillAmount = speed / maxMoveSpeed;
            }
            catch (NullReferenceException)
            {

            }

        }
        else {
            speedBar.fillAmount = 0;
        }


    }


}
