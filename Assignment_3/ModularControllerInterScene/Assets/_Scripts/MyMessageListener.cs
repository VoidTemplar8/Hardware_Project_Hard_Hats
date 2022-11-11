using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMessageListener : MonoBehaviour
{
    GameObject capsuleModifier;

    public GameObject capsule;
    public Light lamp;
    private bool rotate;
    private float speed;
    private float lightSensed;

    // Start is called before the first frame update
    void Start()
    {
        capsuleModifier = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        capsule.transform.position += Vector3.forward * speed;
        if (rotate) capsule.transform.Rotate(0, 0, 10);
        lamp.intensity = lightSensed;
    }

    void OnMessageArrived(string msg)
    {
        //  Debug.Log("Message arrived: " + msg);
        string[] msgSplit = msg.Split(',');
        rotate = (float.Parse(msgSplit[0]) > 0) ? true : false;
        speed = (float.Parse(msgSplit[1]) - 512) / 500;
        lightSensed = float.Parse(msgSplit[2]) / 102;

        if (msg == "1")
        {
            capsuleModifier.gameObject.GetComponent<Rigidbody>().AddForce(0, 100, 0);
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
}
