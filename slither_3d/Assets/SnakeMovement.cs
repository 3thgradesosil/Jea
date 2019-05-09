using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    public List<Transform> BodyParts = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //InputRotation();
        MoveWithMouse();
        
    }
    private Vector3 pointInWorld;
    private Vector3 Mouse_Pos;
    private float radius = 3.0f;
    private Vector3 direction;

    void MoveWithMouse()
    {
        Ray ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000.0f);

        Mouse_Pos = new Vector3(hit.point.x, hit.point.y, 0);

        direction = Vector3.Slerp(direction, Mouse_Pos- transform.position, Time.deltaTime * 2);
        direction.z = 0;

        pointInWorld = direction.normalized * radius + transform.position;

        transform.LookAt(pointInWorld);
    }

    void InputRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            currentRotation += RotationSensetivity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            currentRotation -= RotationSensetivity * Time.deltaTime;
        }
    }

    public float speed = 3.5f;
    void FixedUpdate()
    {
        MoveForward();
        //Rotation();
        CameraFollow();
    }
    void MoveForward()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    public float currentRotation;
    public float RotationSensetivity = 300.0f;

    void Rotation()
    {
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y,currentRotation));
    }

    [Range(0.0f,1.0f)]
    public float smoothTime = 0.1f;
    void CameraFollow()
    {
        Transform camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.transform;
        Vector3 cameraVelocity = Vector3.zero;
        camera.position = Vector3.SmoothDamp(camera.position, 
           new Vector3(gameObject.transform.position.x,gameObject.transform.position.y, -10), ref cameraVelocity, smoothTime);
    }
    public Transform bodyObject;
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Food")
        {
            Destroy(other.gameObject);
            if(BodyParts.Count == 0)
            {
                Vector3 currentPos = transform.position;
                Transform newBodyPart = Instantiate(bodyObject, currentPos, Quaternion.identity) as  Transform;
                BodyParts.Add(newBodyPart);
            }
            else
            {
                Vector3 currentPos = BodyParts[BodyParts.Count - 1].position;
                Transform newBodyPart = Instantiate(bodyObject, currentPos, Quaternion.identity) as Transform;
                BodyParts.Add(newBodyPart);
            }
        }
    }
}
