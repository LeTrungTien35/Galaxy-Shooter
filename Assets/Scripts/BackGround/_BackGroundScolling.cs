using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BackGroundScolling : MonoBehaviour
{
    [SerializeField]
    private float tocDoCuon = 0.1f;
    private Renderer _renderer;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        float y = Mathf.Repeat(Time.time * tocDoCuon, 1);
        Vector2 Offset = new Vector2(0, y);
        _renderer.sharedMaterial.SetTextureOffset("_MainTex", Offset);
    }

      
}
