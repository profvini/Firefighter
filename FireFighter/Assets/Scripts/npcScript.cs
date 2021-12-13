using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class npcScript : NetworkBehaviour
{
    public GameObject npcObj;
    public Transform saveTransform;
    public Animator npcAnim;
    public float lifeNpc = 100f , lessLife = 0.5f;

    public bool saveBool = false;



    public void Update()
    {
        //NPC perde vida com o tempo
        if (lifeNpc > 0 && !saveBool)
        {
            lifeNpc -= Time.deltaTime * lessLife;
        }
        //Se a vida chega a 0, o NPC morre
        if (lifeNpc <= 0)
        {
            FindObjectOfType<PlayerMove>().isDead = true;
            FindObjectOfType<PlayerMove>().Death();
        }
    }

    //Desativa o NPC no local original quando ele passa a ser carregado pelo player
    public void objInterection()
    {  
        npcObj.SetActive(false);
    }

    //Muda a posição do NPC que irá spawnar na safezone
    public void objSave(int saveLifes)
    {
        saveBool = true;
        npcObj.SetActive(true);
        npcAnim.SetTrigger("Saved");
        npcObj.transform.position = new Vector3(saveTransform.position.x,saveTransform.position.y,saveTransform.position.z - (saveLifes * 1.34f));
        npcObj.transform.eulerAngles = new Vector3(0, 90, 0);
        npcObj.tag = "Saved";
    }
}
