using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObj : MonoBehaviour
{
    // objetos para cair... criar variaveis
    public bool testBool = false;


    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            //Ativar Rigibody dos objetos para eles cairem ou ativar a gravitade 
            // som de objeto caindo ou se soltando
            // ativar som de objeto caindo
            // ativar fogo nos objetos comando active ou enable nao lembro
            // teste 
            testBool = true;
        }
    }

}
