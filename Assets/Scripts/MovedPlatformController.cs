using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovedPlatformController : MonoBehaviour
{
    public float delta;
    public float speed;

    Vector3 startPos;
    Vector3 targetPos;

    bool isMoveToStartPos = true;

    private void Start()
    {
        startPos = new Vector3(-delta, transform.position.y, transform.position.z);
        targetPos = new Vector3(delta, transform.position.y, transform.position.z);
    }


    public void FixedUpdate()
    {
        if (transform.position.x <= startPos.x)
            isMoveToStartPos = true;

        if (transform.position.x >= targetPos.x)
            isMoveToStartPos = false;

        if (isMoveToStartPos)
            transform.position += new Vector3(Time.deltaTime * speed, 0, 0);
        else
            transform.position -= new Vector3(Time.deltaTime * speed, 0, 0);

    }
}
