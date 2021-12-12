using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

//Colocar os paineis em um script separado já presente no jogo

public class PlayerMoveSemCam : NetworkBehaviour
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
    public GameObject panels, panelPause, panelDeath, panelEnd;
    

    RaycastHit _hit;

    //public GameObject saveBar, saveBartext, oxigImage, saveImage, saveImageBG;
    public Image imgCarrying; //oxigImage, saveImage, saveImageBG
    public float saveFloat = 0f;

    public bool isCarrying = false;

    public float oxigFloat = 100, lostOxig = 0.3f;
    public bool canHit = true;
    public GameObject crosshair;
    //public Image crosshair;
    public GameObject savePerson = null;

    public int saveLifes = 0;
    public Text txtSaveLifes;
    public bool isDead = false;

    private ParticleSystem _ps;

    public PanelsScript panel;

    // Start is called before the first frame update

    private void Awake()
    {
        panel = GameObject.Find("Panels").GetComponent<PanelsScript>();




    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0.756f, 0);
    }

    void Start()
    {
        Camera.main.GetComponent<MouseLook>().enabled = true;
        Camera.main.GetComponent<MouseLook>().playerBody = transform;
        _ps = GetComponent<ParticleSystem>();

        


        isDead = false;
        isCarrying = false;
        //imgCarrying.enabled = false;
        panel.panelPause.SetActive(false);
        panel.panelDeath.SetActive(false);
        panel.panelEnd.SetActive(false);
        panel.panels.GetComponent<Canvas>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            hudAction();

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
                panel.panelEnd.SetActive(true);
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
    }



    void hudAction()
    {
        oxigFloat -= Time.deltaTime * lostOxig;
        panel.oxigImage.GetComponent<Image>().fillAmount = oxigFloat / 100;
    }

    void actionPlayer()
    {
        if (isLocalPlayer)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out _hit, distanceOfRaycast))
            {
                crosshairHit();

                if (_hit.transform.CompareTag("Obj") || _hit.transform.CompareTag("hotZone") && !isPause)
                {
                    panel.saveImageBG.GetComponent<Image>().enabled = true;
                    panel.saveBartext.SetActive(true);
                }
                else
                {
                    panel.saveImageBG.GetComponent<Image>().enabled = false;
                    panel.saveBartext.SetActive(false);
                    panel.saveBar.GetComponent<Image>().fillAmount = 0;
                    saveFloat = 0f;
                }

                if (Input.GetButton("Fire1") && _hit.transform.CompareTag("Obj") && !isCarrying && !isPause && !isDead)
                {
                    saveFloat += Time.deltaTime * 20f;
                    panel.saveBar.SetActive(true);
                    panel.saveBartext.SetActive(true);
                    panel.saveBar.GetComponent<Image>().fillAmount = saveFloat / 100;

                    if (saveFloat >= 100)
                    {
                        panel.saveBar.GetComponent<Image>().fillAmount = 0;
                        panel.saveBar.SetActive(false);
                        panel.saveBartext.SetActive(false);
                        //som que pegou o npc
                        //imagem de carregando uma pessoa
                        //nao pode carregar outra !!!!!!!
                        isCarrying = true;
                        panel.imgCarrying.GetComponent<Image>().enabled = true;
                        _hit.transform.gameObject.GetComponent<npcScript>().objInterection();
                        savePerson = _hit.transform.gameObject;
                        saveFloat = 0;
                    }

                }

                if (Input.GetButtonUp("Fire1"))
                {
                    //panel.saveBar.SetActive(false);
                    //panel.saveBartext.SetActive(false);
                    panel.saveBar.GetComponent<Image>().fillAmount = 0;

                    saveFloat = 0f;
                }
              
                if (_hit.transform.CompareTag("Obj") || _hit.transform.CompareTag("hotZone"))
                {
                    panel.saveImageBG.SetActive(true);
                }

                else
                {
                    panel.saveImageBG.SetActive(false);
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

                if (Input.GetButton("Fire1") && _hit.transform.CompareTag("hotZone") && !isCarrying && !isPause && !isDead )
                {
                    saveFloat += Time.deltaTime * 20f;
                    panel.saveBar.SetActive(true);
                    panel.saveBartext.SetActive(true);
                    panel.saveBar.GetComponent<Image>().fillAmount = saveFloat / 100;

                    if (saveFloat >= 100)
                    {
                        panel.saveBar.GetComponent<Image>().fillAmount = 0;
                        panel.saveBar.SetActive(false);
                        panel.saveBartext.SetActive(false);
                        Destroy(_hit.transform.gameObject);
                        saveFloat = 0;
                    }
                }

                //else
                //{
                //    //saveBar.SetActive(false);
                //    //saveBartext.SetActive(false);

                //    saveFloat = 0f;
                //}
            }
        }
    }

    void crosshairHit()
    {
        if (_hit.collider.gameObject.CompareTag("Door") || _hit.transform.CompareTag("Obj") || _hit.transform.CompareTag("OpenDoor") || _hit.collider.gameObject.CompareTag("hotZone"))
        {
            panel.crosshair.GetComponent<Image>().color = Color.green;
        }
        else
        {
            panel.crosshair.GetComponent<Image>().color = Color.white;
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
                disableSavingBar();
                Time.timeScale = 0.00000001f;
                panel.panelPause.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case false:
                Time.timeScale = 1;
                panel.panelPause.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }
    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("safe") && isCarrying)
        {
            //som de salvando uma pessoa pode ser stay here.
            // I will be back
            isCarrying = false;
            panel.imgCarrying.SetActive(false);
            if (savePerson != null)
            {
                savePerson.transform.gameObject.GetComponent<npcScript>().objSave(saveLifes);
                saveLifes++;
                panel.addSaveLife();
                panel.imgCarrying.SetActive(false);
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
                panel.panelDeath.SetActive(true);
                disableSavingBar();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case false:
                Time.timeScale = 1;
                panel.panelDeath.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }

    }

    void disableSavingBar()
    {
        panel.saveBar.GetComponent<Image>().fillAmount = 0;
        panel.saveBar.SetActive(false);
        panel.saveBartext.SetActive(false);
        saveFloat = 0;
    }
}
