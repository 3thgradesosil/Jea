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
        spawnBomb();
        Running();
        Scaling();

    }
    public void spawnBomb()
    {
        StartCoroutine("CallBombEveryXSeconds", 5);

    }
    IEnumerator CallBombEveryXSeconds(float x)
    {
        yield return new WaitForSeconds(x);
        float radiiusOfSpawn = 3;
        StopCoroutine("CallBombEveryXSeconds");
        Vector3 randomBombPos = new Vector3(Random.Range(
                                                Random.Range(transform.position.x - 10, transform.position.x - 5),
                                                Random.Range(transform.position.x + 5, transform.position.x + 10)
                                            ),
                                            Random.Range(
                                                Random.Range(transform.position.y - 10, transform.position.y - 5),
                                                Random.Range(transform.position.y + 5, transform.position.y + 10)), 0);
        Vector3 directionB = randomBombPos - transform.position;
        Vector3 finalBombPos = transform.position + (directionB.normalized * radiiusOfSpawn);
        GameObject newBomb = Instantiate(BombPrefab, finalBombPos, Quaternion.identity) as GameObject;
        GameObject BombParent = GameObject.Find("Bombs");
        newBomb.transform.parent = BombParent.transform;

    }
    public void spawnFood()
    {
        StartCoroutine("CallEveryXSeconds", spawnFoodEveryXSeconds);

    }


    public float spawnFoodEveryXSeconds = 1;
    public GameObject FoodPrefab;
    public GameObject BombPrefab;

    IEnumerator CallEveryXSeconds(float x)
    {
        yield return new WaitForSeconds(x);

        float radiiusOfSpawn = 3;
        StopCoroutine("CallEveryXSeconds");
        Vector3 randomFoodPos = new Vector3(Random.Range(
                                                Random.Range(transform.position.x - 10, transform.position.x - 5),
                                                Random.Range(transform.position.x + 5, transform.position.x + 10)
                                            ),
                                            Random.Range(
                                                Random.Range(transform.position.y - 10, transform.position.y - 5),
                                                Random.Range(transform.position.y + 5, transform.position.y + 10)), 0);

        

        Vector3 directionF = randomFoodPos - transform.position;
        Vector3 finalFoodPos = transform.position + (directionF.normalized * radiiusOfSpawn);
       

       
       


      

        GameObject newFood = Instantiate(FoodPrefab, finalFoodPos, Quaternion.identity) as GameObject;
        GameObject foodParent = GameObject.Find("Foods");
        newFood.transform.parent = foodParent.transform;
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

        direction = Vector3.Slerp(direction, Mouse_Pos - transform.position, Time.deltaTime * 2);
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
        ApplyShitOnBody();
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
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, currentRotation));
    }

    [Range(0.0f, 1.0f)]
    public float smoothTime = 0.1f;


    void CameraFollow()
    {
        Transform camera = GameObject.FindGameObjectWithTag("MainCamera").gameObject.transform;
        Vector3 cameraVelocity = Vector3.zero;
        camera.position = Vector3.SmoothDamp(camera.position,
           new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10), ref cameraVelocity, smoothTime);
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
                return false ;
            }
            else
            {
                return false;
            }
        }
        catch (System.Exception e)
        {
            return false;
        }
        //return false;
    }


    public Transform bodyObject;
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Food")
        {
            Destroy(other.gameObject);
            //Food_count++;
            if (!SizeUp(Food_count))
            {
                Food_count++;
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



                    //newBodyPart.localScale = currentSize;
                    //newBodyPart.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;

                    BodyParts.Add(newBodyPart);
                }
            }
            /*else
            {
                currentSize += Vector3.one * growScale;
                currentSize += Vector3.one * growScale;
                //bodyPartOverTimeFollow += 0.04f;
                bodyPartFollowTime_n += growScale / 3;
                bodyPartFollowTime_r += growScale / 4;
                transform.localScale = currentSize; //headSize
               
            }*/
        }
        else if (other.transform.tag == "Bomb")
        {
            Destroy(other.gameObject);
            int lastIndex = BodyParts.Count - 1;
            Transform lastBodyPart = BodyParts[lastIndex].transform;

            Instantiate(FoodPrefab, lastBodyPart.position, Quaternion.identity);

            BodyParts.RemoveAt(lastIndex);
            Destroy(lastBodyPart.gameObject);
        }
    }



    private bool running;
    public float runningSpeed = 8.5f;
    public float normalSpeed = 3.5f;
    public float bodyPartFollowTime_n = 0.19f;
    public float bodyPartFollowTime_r = 0.1f;


    void Running()
    {
        if(BodyParts.Count > 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                speed = runningSpeed;
                running = true;
                bodyPartOverTimeFollow = bodyPartFollowTime_r;
            }
            if (Input.GetMouseButtonUp(0))
            {
                speed = normalSpeed;
                running = false;
                bodyPartOverTimeFollow = bodyPartFollowTime_n;
            }
        }
        else
        {
            speed = normalSpeed;
            running = false;
            bodyPartOverTimeFollow = bodyPartFollowTime_n;
        }

        if(running == true)
        {
            StartCoroutine("LoseBodyParts");
        }
        else
        {
            bodyPartOverTimeFollow = bodyPartFollowTime_n;
        }
    }


    IEnumerator LoseBodyParts()
    {
        yield return new WaitForSeconds(1.5f);

        int lastIndex = BodyParts.Count - 1;
        Transform lastBodyPart = BodyParts[lastIndex].transform;

        Instantiate(FoodPrefab, lastBodyPart.position, Quaternion.identity);
        
        BodyParts.RemoveAt(lastIndex);
        Destroy(lastBodyPart.gameObject);
        Food_count--;
        StopCoroutine("LoseBodyParts");
    }


    void ApplyShitOnBody()
    {
        transform.localScale = currentSize;
        foreach (Transform bodyPart_i in BodyParts)
        {
            bodyPart_i.localScale = currentSize;
            bodyPart_i.GetComponent<SnakeBody>().overTime = bodyPartOverTimeFollow;
        }
    }

    public List<bool> scaleTrack;
    private int howBigAmI;
    public float followTimeSensitivity = 0.2f;
    public float scaleSensitivity = 0.1f;

    void Scaling()
    {
        scaleTrack = new List<bool>(new bool[growOnThisFood.Length]);
        howBigAmI = 0;

        for(int i =0; i < growOnThisFood.Length; i++)
        {
            if(Food_count >= growOnThisFood[i])
            {
                scaleTrack[i] = !scaleTrack[i];
                howBigAmI++;
            }
        }
        currentSize = new Vector3(1 + (howBigAmI * scaleSensitivity)
                                 , 1 + (howBigAmI * scaleSensitivity)
                                 , 1 + (howBigAmI * scaleSensitivity));
        bodyPartFollowTime_n = (howBigAmI / 100.0f) + followTimeSensitivity;
        bodyPartFollowTime_r = bodyPartFollowTime_n / 2;
    }
}

