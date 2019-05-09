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
        spawnFood();

        
    }
    public void spawnFood()
    {
        StartCoroutine("CallEveryXSeconds", spawnFoodEveryXSeconds);
       
    }
    public float spawnFoodEveryXSeconds = 1;
    public GameObject FoodPrefab;
    IEnumerator CallEveryXSeconds(float x)
    {
        yield return new WaitForSecondsRealtime(x);
        StopCoroutine("CallEveryXSeconds");
        Vector3 randomFoodPos = new Vector3(Random.Range(
                                                Random.Range(transform.position.x - 10, transform.position.x - 5),
                                                Random.Range(transform.position.x + 5, transform.position.x + 10)
                                            ),
                                            Random.Range(
                                                Random.Range(transform.position.y - 10, transform.position.y - 5),
                                                Random.Range(transform.position.y + 5, transform.position.y + 10)), 0);

        GameObject newFood = Instantiate(FoodPrefab, randomFoodPos, Quaternion.identity) as GameObject;
        //GameObject foodParent = GameObject.Find("Foods");
        //newFood.transform.parent = foodParent.transform;
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
    private int Food_count;
    private int current_Food;
    public int[] growOnThisFood;
    private Vector3 currentSize = Vector3.one;
    public float growScale = 0.1f;
    public float bodyPartOverTimeFollow = 0.19f;
    bool SizeUp(int x)
    {
        try
        {
            if (x == growOnThisFood[current_Food])
            {
                current_Food++;
                return true;
            }
            else
            {
                return false;
            }
        }catch(System.Exception e)
        {
            return false;
        }
        //return false;
    }
    public Transform bodyObject;
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Food")
        {
            Destroy(other.gameObject);
            Food_count++;
            if (!SizeUp(Food_count))
            {
                if (BodyParts.Count == 0)
                {
                    Vector3 currentPos = transform.position;
                    Transform newBodyPart = Instantiate(bodyObject, currentPos, Quaternion.identity) as Transform;


                    newBodyPart.localScale = currentSize;
                    newBodyPart.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;

                    BodyParts.Add(newBodyPart);
                }
                else
                {
                    Vector3 currentPos = BodyParts[BodyParts.Count - 1].position;
                    Transform newBodyPart = Instantiate(bodyObject, currentPos, Quaternion.identity) as Transform;



                    newBodyPart.localScale = currentSize;
                    newBodyPart.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;

                    BodyParts.Add(newBodyPart);
                }
            }
            else
            {
                currentSize += Vector3.one * growScale;
                bodyPartOverTimeFollow += 0.04f;
                transform.localScale = currentSize; //headSize

                foreach(Transform bodyPart_i in BodyParts)
                {
                    bodyPart_i.localScale = currentSize;
                    bodyPart_i.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;
                }
            }
        }
    }
}
