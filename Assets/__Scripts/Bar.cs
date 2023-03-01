using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField]
    private float width, height, depth;
    private GameObject bar;
    // Start is called before the first frame update

    public void SetDimension(Vector3 dim){
        width = dim.x;
        height = dim.y;
        depth = dim.z;
        Vector3 scaleChange = new Vector3(width - transform.localScale.x, height - transform.localScale.y, depth - transform.localScale.z);
        transform.localScale += scaleChange;
    }
    void Start()
    {

        
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
