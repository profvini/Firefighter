using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{
    public PlayerMove _player;
    public AudioClip doorHit, doorbreak;
    public int doorInteraction = 0;
    public Rigidbody doorRg;
    public float movx, movz;
    public bool OpenDoor = false;
    public Animator doorAnim;

    public void Start()
    {
        _player = FindObjectOfType<PlayerMove>();
              
    }

    public void objInterection()
    {
        //Se a tag não for OpenDoor, roda a parte das portas trancadas
        if (!OpenDoor)
        {
            doorInteraction++;
            switch (doorInteraction)
            {
                //Casos 1 a 3 fazem tocar o som de hit na porta, caso 4 é o som que quebra a porta.
                case 1:
                    AudioSource.PlayClipAtPoint(doorHit, this.transform.position);
                    break;
                case 2:
                    AudioSource.PlayClipAtPoint(doorHit, this.transform.position);
                    break;
                case 3:
                    AudioSource.PlayClipAtPoint(doorHit, this.transform.position);
                    break;
                case 4:
                    //Toca o som de quebrar a porta
                    AudioSource.PlayClipAtPoint(doorbreak, this.transform.position);

                    //Retira os constraints do rigidbody e ativa sua gravidade
                    doorRg.constraints = RigidbodyConstraints.None;
                    doorRg.useGravity = true;

                    //Faz a porta cair de acordo com a variável que pode ser modificada no inspector
                    doorRg.AddForce(new Vector3(-movx, 0, -movz), ForceMode.Force);

                    //rodar animaçao ou fisica
                    break;
            }
        } 
        //Caso contrário, a porta só abre
        else 
        {
            if (doorAnim != null)
            {
                doorAnim.SetTrigger("Open");
            }
        }
    }
}
