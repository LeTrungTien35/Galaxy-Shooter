using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitMove : MonoBehaviour
{
    private float minX, maxX, minY, maxY;
    void Start()
    {
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        minX = -bounds.x;
        maxX = bounds.x;

        minY = -bounds.y;
        maxY = bounds.y;

    }

    void Update()
    {
        Vector3 temp = transform.position;

        if (temp.x < minX)
        {
            temp.x = minX;
        }
        else if (temp.x > maxX)
        {
            temp.x = maxX;
        }

        if (temp.y < minY)
        {
            temp.y = minY;
        }
        else if (temp.y > maxY)
        {
            temp.y = maxY;
        }

        transform.position = temp;
    }
}
