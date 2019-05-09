using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    private int myOrder;
    private Transform head;
    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        for (int i = 0; i < head.GetComponent<SnakeMovement>().BodyParts.Count; i++)
        {
            if (gameObject == head.GetComponent<SnakeMovement>().BodyParts[i].gameObject)
            {
                myOrder = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Vector3 movementVelocity;
    [Range(0.0f,1.0f)]
    public float overTime = 0.15f;
    void FixedUpdate()
    {
     if(myOrder == 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, head.position, ref movementVelocity, overTime);
            transform.LookAt(head.transform.position);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, head.GetComponent<SnakeMovement>().BodyParts[myOrder - 1].position, ref movementVelocity,overTime);
            transform.LookAt(head.transform.position);
        }
    }
   
}
