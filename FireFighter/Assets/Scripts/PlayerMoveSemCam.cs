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

        panel.menuJoin.SetActive(false);
    }

    void Start()
    {
        Camera.main.GetComponent<MouseLook>().enabled = true;
        Camera.main.GetComponent<MouseLook>().playerBody = transform;
        _ps = GetComponent<ParticleSystem>();




        isDead = false;
        isCarrying = false;
        
        //Desativa os painéis por padrão
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

            //Caso o player esteja carregando um NPC, sua velocidade cai pela metade e seu oxigênio começa a baixar mais rápido
            if (isCarrying)
            {
                speed = 3f;
                lostOxig = 1.0f;
            }
            //Caso contrário, velocidade maior e menos oxigênio perdido por segundo
            else
            {
                speed = 6f;
                lostOxig = 0.5f;
            }

            //Caso oxigênio chegue a 0, game over, chama a fuñção que termina com o jogo
            if (oxigFloat <= 0)
            {
                isDead = true;
                Death();
            }

            //numero de saves para vencer o level
            if (saveLifes >= 5)
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

                //Quando o LMB é apertado, e a tag corresponde, e estas boools são falsas, o player enche a variável, que preenche uma barra da UI, que quando chega a 100
                //permite carregar um NPC
                if (Input.GetButton("Fire1") && _hit.transform.CompareTag("Obj") && !isCarrying && !isPause && !isDead)
                {
                    //Ativa os painés que representam e enche a barrinha
                    saveFloat += Time.deltaTime * 20f;
                    panel.saveBar.SetActive(true);
                    panel.saveBartext.SetActive(true);
                    panel.saveBar.GetComponent<Image>().fillAmount = saveFloat / 100;

                    if (saveFloat >= 100)
                    {
                        //Quando a variável chega a 100, a barrinha esvazia e o painél é desativado
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

                //Funcionamento básico do LMB
                if (Input.GetButtonUp("Fire1"))
                {
                    //panel.saveBar.SetActive(false);
                    //panel.saveBartext.SetActive(false);
                    panel.saveBar.GetComponent<Image>().fillAmount = 0;

                    saveFloat = 0f;
                }

                //Checa se alguma das tags batem quando clica, e ativa os painés respectivos com a tag, sendo obj pro NPC e hotZone pro fogo
                if (_hit.transform.CompareTag("Obj") || _hit.transform.CompareTag("hotZone"))
                {
                    panel.saveImageBG.SetActive(true);
                }

                else
                {
                    panel.saveImageBG.SetActive(false);
                }

                //Se o clique com LMB tiver a tag Door, e o a bool canhit for verdadeira, inverte a bool, chama o script da porta, e reseta a bool
                if (Input.GetButtonDown("Fire1") && _hit.transform.CompareTag("Door") && canHit)
                {
                    canHit = false;

                    _hit.transform.gameObject.GetComponent<doorScript>().objInterection();
                    Invoke("canHitAgain", 1f);
                }

                //Se a tag for porta aberta, também chama o script da porta.
                if (Input.GetButtonDown("Fire1") && _hit.transform.CompareTag("OpenDoor"))
                {
                    _hit.transform.gameObject.GetComponent<doorScript>().objInterection();
                }

                //Funcionamento do fogo, feito de forma parecida com o salvar dos NPCs, o principío é o mesmo, o diferencial é que quando chega a 100, ele desativa o gameobject com a tag hotZone
                if (Input.GetButton("Fire1") && _hit.transform.CompareTag("hotZone") && !isCarrying && !isPause && !isDead)
                {
                    saveFloat += Time.deltaTime * 20f;
                    panel.saveBar.SetActive(true);
                    panel.saveBartext.SetActive(true);
                    panel.saveBar.GetComponent<Image>().fillAmount = saveFloat / 100;

                    if (saveFloat >= 100)
                    {
                        //Reseta a varíavel pra 0 e desativa os painéis.
                        panel.saveBar.GetComponent<Image>().fillAmount = 0;
                        panel.saveBar.SetActive(false);
                        panel.saveBartext.SetActive(false);
                        //Desativa o objeto onde se estava mirando, neste caso o Fogo
                        _hit.transform.gameObject.SetActive(false);
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

    //Função que diz que quando o cursor passa em cima de alguma dessas tags ele troca de cor para indicar um interagível
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

    //Reseta a booleana
    void canHitAgain()
    {
        canHit = true;
    }

    //Função que ativa o pause do game, que basicamente congelar o jogo, destrancar o cursor do centro da tela e trazer um painél branco na tela para indicar o puase
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

    //Função para quando o player entra na safezone carregando um NPC.
    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("safe") && isCarrying)
        {
            //som de salvando uma pessoa pode ser stay here.
            // I will be back
            isCarrying = false;
            panel.imgCarrying.SetActive(false);
            //Se está carregando alguém
            if (savePerson != null)
            {
                //Spawna a pessoa sentada na safezone e adiciona 1 ao contador de total de vidas salvas
                savePerson.transform.gameObject.GetComponent<npcScript>().objSave(saveLifes);
                saveLifes++;
                panel.addSaveLife();
                //Desativa a imagem que indica que o player está carregando alguém
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
            //Caso o oxigênio acabe, o tempo basicamente congela, o painél de morte ativa, o cursor destrava do centro da tela e o jogo para.
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

    //Reseta e desativa a barrinha que preenche;
    void disableSavingBar()
    {
        panel.saveBar.GetComponent<Image>().fillAmount = 0;
        panel.saveBar.SetActive(false);
        panel.saveBartext.SetActive(false);
        saveFloat = 0;
    }
}
