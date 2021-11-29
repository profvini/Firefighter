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
        if (!OpenDoor)
        {
            doorInteraction++;
            switch (doorInteraction)
            {
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
                    AudioSource.PlayClipAtPoint(doorbreak, this.transform.position);

                    doorRg.constraints = RigidbodyConstraints.None;
                    doorRg.useGravity = true;

                    doorRg.AddForce(new Vector3(-movx, 0, -movz), ForceMode.Force);

                    //rodar animaçao ou fisica
                    break;
            }
        } else 
        {
            if (doorAnim != null)
            {
                doorAnim.SetTrigger("Open");
            }
        }
    }
}
