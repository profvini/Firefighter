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

        if (lifeNpc > 0 && !saveBool)
        {
            lifeNpc -= Time.deltaTime * lessLife;
        }

        if (lifeNpc <= 0)
        {
            FindObjectOfType<PlayerMove>().isDead = true;
            FindObjectOfType<PlayerMove>().Death();
        }
    }

    public void objInterection()
    {
     
        npcObj.SetActive(false);

    }

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
