using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarController : MonoBehaviour
{
    public Transform healthbar;
   
    public void setSizeBar(float size)
    {
        healthbar.localScale = new Vector2(size, 1f);
    }
}
