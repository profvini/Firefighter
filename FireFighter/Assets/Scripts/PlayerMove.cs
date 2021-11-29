using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundcheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    // private RaycastHit _hit;
    public float distanceOfRaycast;

    public bool isPause = false;
    public GameObject panelPause, panelDeath, panelEnd;
    

    RaycastHit _hit;

    public GameObject saveBar, saveBartext;
    public Image saveImage, saveImageBG, oxigImage, imgCarrying;
    public float saveFloat = 0f;

    public bool isCarrying = false;

    public float oxigFloat = 100, lostOxig = 0.3f;
    public bool canHit = true;
    public Image crosshair;
    public GameObject savePerson = null;

    public int saveLifes = 0;
    public Text txtSaveLifes;
    public bool isDead = false;

    private ParticleSystem _ps;

    // Start is called before the first frame update

    private void Awake()
    {
        panelPause = GameObject.Find("panelPause");
        panelDeath = GameObject.Find("panelDeath");
        panelEnd = GameObject.Find("panelEnd");
        
    }

    void Start()
    {     

        txtSaveLifes.text = "Lifes = " + saveLifes.ToString() + "/3";
        isDead = false;
        isCarrying = false;
        saveBar.SetActive(false);
        saveBartext.SetActive(false);
        panelPause.SetActive(false);
        panelDeath.SetActive(false);
        panelEnd.SetActive(false);
        saveImageBG.enabled = false;
        imgCarrying.enabled = false;
        _ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //hudAction();

        gamePause();

        actionPlayer();

        if (isCarrying)
        {
            speed = 3f;
            lostOxig = 1.0f;
        }
        else
        {
            speed = 6f;
            lostOxig = 0.5f;
        }

        if (oxigFloat <= 0)
        {
            isDead = true;
            Death();
        }

        //numero de saves para vencer o level
        if (saveLifes >= 3)
        {
            //Debug.Log("WIN!!!");
            panelEnd.SetActive(true);
            Time.timeScale = 0.00000001f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        isGrounded = Physics.CheckSphere(groundcheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("Pulo");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }



    //void hudAction()
    //{
    //    oxigFloat -= Time.deltaTime * lostOxig;
    //    oxigImage.fillAmount = oxigFloat / 100;
    //}

    void actionPlayer()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out _hit, distanceOfRaycast))
        {
            crosshairHit();

            if (_hit.transform.CompareTag("Obj"))
            {
                saveImageBG.enabled = true;
            }
            else
            {
                saveImageBG.enabled = false;
            }

            if (Input.GetButton("Fire1") && _hit.transform.CompareTag("Obj") && !isCarrying)
            {
                saveFloat += Time.deltaTime * 20f;
                saveBar.SetActive(true);
                saveBartext.SetActive(true);
                saveImage.fillAmount = saveFloat / 100;

                if (saveFloat >= 100)
                {
                    //som que pegou o npc
                    //imagem de carregando uma pessoa
                    //nao pode carregar outra !!!!!!!
                    isCarrying = true;
                    imgCarrying.enabled = true;
                    _hit.transform.gameObject.GetComponent<npcScript>().objInterection();
                    savePerson = _hit.transform.gameObject;
                }

            }

          

            if (Input.GetButtonDown("Fire1") && _hit.transform.CompareTag("Door") && canHit)
            {
                canHit = false;

                _hit.transform.gameObject.GetComponent<doorScript>().objInterection();
                Invoke("canHitAgain", 1f);
            }

            if (Input.GetButtonDown("Fire1") && _hit.transform.CompareTag("OpenDoor"))
            {
                _hit.transform.gameObject.GetComponent<doorScript>().objInterection();
            }


            if (Input.GetButton("Fire1") && _hit.transform.CompareTag("hotZone"))
            {
                saveFloat += Time.deltaTime * 20f;
                saveBar.SetActive(true);
                saveBartext.SetActive(true);
                saveImage.fillAmount = saveFloat / 100;

                if (saveFloat >= 100)
                {
                    Destroy(_hit.transform.gameObject);
                }
            }

            else
            {
                saveBar.SetActive(false);
                saveBartext.SetActive(false);

                saveFloat = 0f;
            }
        }
    }
    void crosshairHit()
    {
        if (_hit.collider.gameObject.CompareTag("Door") || _hit.transform.CompareTag("Obj") || _hit.transform.CompareTag("OpenDoor") || _hit.collider.gameObject.CompareTag("hotZone"))
        {
            crosshair.color = Color.green;
        }
        else
        {
            crosshair.color = Color.white;
        }
    }

    void canHitAgain()
    {
        canHit = true;
    }

    void gamePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;
        }

        switch (isPause)
        {
            case true:
                Time.timeScale = 0.00000001f;
                panelPause.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case false:
                Time.timeScale = 1;
                panelPause.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("safe"))
        {
            //som de salvando uma pessoa pode ser stay here.
            // I will be back
            isCarrying = false;
            imgCarrying.enabled = false;
            if (savePerson != null)
            {
                savePerson.transform.gameObject.GetComponent<npcScript>().objSave();
                saveLifes++;
                txtSaveLifes.text = "Lifes = " + saveLifes.ToString() + "/6";
                // other.transform.gameObject.GetComponent<npcScript>().objSave();
                savePerson = null;
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("hotZone"))
        {
            // play sound cant go in there! It is too hot!
        }
    }

    public void Death()
    {
        switch (isDead)
        {
            case true:
                Time.timeScale = 0.00000001f;
                panelDeath.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case false:
                Time.timeScale = 1;
                panelDeath.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }

    }
}
