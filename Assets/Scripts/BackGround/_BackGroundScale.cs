using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BackGroundScale : MonoBehaviour
{   
    void Start()
    {
        float worldHeight = Camera.main.orthographicSize * 2f;
        float worldWitdh = worldHeight * Screen.width / Screen.height;
        transform.localScale = new Vector3(worldWitdh, worldHeight, 0f);
    }   
}
