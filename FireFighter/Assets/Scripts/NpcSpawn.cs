using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawn : MonoBehaviour
{
    public GameObject[] npc_spawn_list;
    public int max_npc = 3;
    public int npc_count = 0;
    public int objSpawner = 0;


    // Start is called before the first frame update
    void Start()
    {
        npcStartPosition();
    }

    // Update is called once per frame
    void npcStartPosition()
    {

        while (npc_count < max_npc)
        {
            int random_i;
            
            random_i = Random.Range(0, 2);
            Debug.Log(random_i);

            if (random_i == 1)
            {
                if (npc_spawn_list[objSpawner].gameObject.activeSelf)
                {
                    npc_count++;
                }

                npc_count++;
                npc_spawn_list[objSpawner].gameObject.SetActive(true);              
            }

            objSpawner++;

            if(objSpawner >= 6)
            {
                objSpawner = 0;
            }

        }


    }
    void Update()
    {

    }
}
