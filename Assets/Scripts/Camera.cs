using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private float xMax;

    [SerializeField]
    private float xMin;

    [SerializeField]
    private float yMax;

    [SerializeField]
    private float yMin;

    private Transform playerTransform;

    //private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        //cameraTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3( Mathf.Clamp(playerTransform.position.x, xMin, xMax), Mathf.Clamp(playerTransform.position.y, yMin, yMax), -10);
        //transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
    }
}
