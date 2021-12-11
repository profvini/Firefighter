using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraController : NetworkBehaviour
{

    public Transform _camera;
    public Vector2 smoothV, mouseLook;
    public float sens, smoth;
    public Rigidbody _playerS;
    public PlayerMove _playerM;
    public PlayerMoveSemCam _playerMsC;

    void Start()
    {
        _playerM = transform.gameObject.GetComponent<PlayerMove>();
        _playerMsC = transform.gameObject.GetComponent<PlayerMoveSemCam>();
        sens = 2.5f;
        smoth = 1f;
    }


    void Update()
    {

        var md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sens * smoth, sens * smoth)); 
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoth);
        smoothV.y = Mathf.Lerp(smoothV.x, md.y, 1f / smoth);

        mouseLook += smoothV;

        if (mouseLook.y < -70)
        {
            mouseLook.y = -70f;
           
        }
           
        if (mouseLook.y >= 90)
        {
            mouseLook.y = 90f;
            
        }
            
        if (_playerMsC.isPause == false)
        {
            _playerS.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, _playerS.transform.up);

            if (mouseLook.y > -70 || mouseLook.y <= 90)
            {
                _camera.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.forward);
            }
        }
    }
}

