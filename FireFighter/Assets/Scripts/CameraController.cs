using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform _camera;
    public Vector2 smoothV, mouseLook;
    public float sens, smoth;
    public Rigidbody _playerS;
    public PlayerMove _playerM;

    void Start()
    {
        _playerM = FindObjectOfType<PlayerMove>();
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
            Debug.Log("aqui embaixo");
        }
           
        if (mouseLook.y >= 90)
        {
            mouseLook.y = 90f;
            Debug.Log("aqui em cima");
        }
            
        if (_playerM.isPause == false)
        {
            _playerS.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, _playerS.transform.up);

            if (mouseLook.y > -70 || mouseLook.y <= 90)
            {
                _camera.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.forward);
            }
        }
    }
}

