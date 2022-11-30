using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMessageListener : MonoBehaviour
{
    GameObject capsuleModifier;
    public SerialController serialController;

    public GameObject capsule;
    public Light lamp;
    private bool jump = false;
    private float rotate;
    private float sideRotate;
    private float speed;
    private float sideSpeed;
    private float lightCollision;
    private float lightJump;

    // Start is called before the first frame update
    void Start()
    {
        capsuleModifier = GameObject.Find("Player");
        serialController = GameObject.Find("Player").GetComponent<SerialController>();
    }

    // Update is called once per frame
    void Update()
    {
        capsule.transform.position += Vector3.forward * speed * Time.deltaTime * -5;
        capsule.transform.position += Vector3.left * sideSpeed * Time.deltaTime * -5;
        //if (rotate) capsule.transform.Rotate(0, 0, 10);
        //lamp.intensity = lightSensed;
    }

    void OnMessageArrived(string msg)
    {
        //  Debug.Log("Message arrived: " + msg);
        string[] msgSplit = msg.Split(',');
        jump = (float.Parse(msgSplit[5]) > 0) ? true : false;
        speed = (float.Parse(msgSplit[1]) - 512) / 500;
        sideSpeed = (float.Parse(msgSplit[2]) - 512) / 500;
        rotate = (float.Parse(msgSplit[6]) - 512) / 500;
        sideRotate = (float.Parse(msgSplit[7]) - 512) / 500;
        //lightCollision = float.Parse(msgSplit[3]) / 102;
        //lightJump = float.Parse(msgSplit[4]) / 102;

        if (jump)
        {
            capsuleModifier.gameObject.GetComponent<Rigidbody>().AddForce(0, 15, 0);
            serialController.SendSerialMessage("R");
        }
        else if (!jump)
        {
            serialController.SendSerialMessage("N");
        }

        if (sideRotate < -1)
        {
            capsule.transform.Rotate(Vector3.up * sideRotate * Time.deltaTime * 50);
            Debug.Log(sideRotate);
            serialController.SendSerialMessage("O");
        }
        else if (sideRotate > 0.3)
        {
            capsule.transform.Rotate(Vector3.down * sideRotate * Time.deltaTime * -100);
            Debug.Log(sideRotate);
            serialController.SendSerialMessage("O");
        }
        else
        {
            serialController.SendSerialMessage("F");
        }
    }

    //Invoked when a connect/disconnect event occurs. The parameter 'success'
    //will be 'true' upon connection, and 'false' upon disconnection or failure to connect
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }
    
    void OnTriggerEnter(Collider other)
    {
        serialController.SendSerialMessage("E");
    }

    void OnTriggerExit(Collider other)
    {
        serialController.SendSerialMessage("A");
    }
}
