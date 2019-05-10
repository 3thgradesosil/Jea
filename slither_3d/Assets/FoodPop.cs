using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPop : MonoBehaviour
{
    public Vector3 startSize = Vector3.zero;
    public Vector3 currentSize;

    void Awake()
    {
        transform.localScale = startSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentSize = Vector3.Slerp(currentSize, new Vector3(0.2f, 0.2f, 0.2f), Time.deltaTime * 5);

        transform.localScale = currentSize;
    }
}
