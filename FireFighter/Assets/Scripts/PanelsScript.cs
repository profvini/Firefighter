using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PanelsScript : NetworkBehaviour
{
    public GameObject menuJoin;
    public GameObject panels, panelPause, panelDeath, panelEnd;
    public GameObject saveBar, saveBartext;
    public GameObject imgCarrying, oxigImage, saveImage, saveImageBG;
    public GameObject crosshair;
    public Text txtSaveLifes;

    [SyncVar(hook = nameof(UISaveLifes))]
    private int saveLifes;

    private void Awake()
    {
        //panels = GameObject.Find("Panels");
        //panelPause = GameObject.Find("Panels/PanelPause");
        //panelDeath = GameObject.Find("Panels/DeathPanel");
        //panelEnd = GameObject.Find("Panels/PanelEnd");
        //crosshair = GameObject.Find("Panels/Crosshair");
        //savingImage = GameObject.Find("Panels/PanelGame/ImagSaving");
        //oxigImage = GameObject.Find("Panels/PanelGame/ImageOxig");
        //saveImage = GameObject.Find("Panels/PanelGame/SaveBar");
        //saveImageBG = GameObject.Find("Panels/PanelGame/SaveBarBG");
        //txtSaveLifes = GameObject.Find("Panels/txtSaveLifes").GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        saveLifes = 0;
        txtSaveLifes.text = "Lifes = " + saveLifes.ToString() + "/6";

        saveBar.SetActive(false);
        saveBartext.SetActive(false);
        saveImageBG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UISaveLifes(int _Old, int _New)
    {
        txtSaveLifes.text = "Lifes = " + saveLifes.ToString() + "/6";
    }

    public void addSaveLife()
    {
        saveLifes++;
        txtSaveLifes.text = "Lifes = " + saveLifes.ToString() + "/6";
    }
}
