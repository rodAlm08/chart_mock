using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private float width, height, depth;
    private GameObject bar;
    // Start is called before the first frame update
    void Start()
    {

        Vector3 scaleChange = new Vector3(width - transform.localScale.x, height - transform.localScale.y, depth - transform.localScale.z);
        transform.localScale += scaleChange;
/* 
        bar = GetComponent<GameObject>();
        transform.localScale.x = width;
        transform.localScale.y = height;
        transform.localScale.z = depth; */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
