using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar_Chart_Controller : MonoBehaviour
{
    private int[] temps = {54, 46, 34, 50, 24, 26, 88, 63, 73};
    [SerializeField] private float WIDTH, HEIGHT;
    [SerializeField] private float barWidth = 1f, barDepth = 1f;
    [SerializeField] private float gap = 0.5f;
    
    [SerializeField] Bar bar;
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] positions = new Vector3[temps.Length];

        for(int i = 0; i < temps.Length; i++){
            Debug.Log("inside for loop " + i);
            Bar pos = Instantiate(bar);
            pos.SetDimension(new Vector3(1f, temps[i], 1f));
            pos.gameObject.transform.position = new Vector3(pos.gameObject.transform.position.x + i * 2, 
            pos.gameObject.transform.position.y + pos.gameObject.transform.localScale.y / 2,
            pos.gameObject.transform.position.z);
        }
        Debug.Log("Congratulations we just instantiated a fucking bar");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
