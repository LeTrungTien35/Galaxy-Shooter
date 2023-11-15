using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMove : MonoBehaviour
{
    private float deltaX, deltaY;
    private Rigidbody2D rd;
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        MoveTouch();
    }

    void MoveTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    deltaY = touchPos.y - transform.position.y;
                    break;
                case TouchPhase.Moved:
                    rd.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                    break;
                case TouchPhase.Ended:
                    rd.velocity = Vector2.zero;
                    break;
            }
        }
    }
}
