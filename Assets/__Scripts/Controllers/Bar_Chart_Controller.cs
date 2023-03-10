using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.IO;
using System.Text;
//using System.Diagnostics;
//using System.Diagnostics;
using UnityEngine;


public class Bar_Chart_Controller : MonoBehaviour
{
    private int[] temps = {54, 46, 34, 50, 24, 26, 88, 63, 73};
    private float max = 88;

    [SerializeField] private float WIDTH, HEIGHT;
    [SerializeField] private float barWidth = 1f, barDepth = 1f;
     private float gap;
    private string jjson = null;
    
    [SerializeField] Bar bar;
    // Start is called before the first frame update
    void Awake()
    {
        using (StreamReader r = new StreamReader("finemotor.json"))
        {
            jjson = r.ReadToEnd();
            //      List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
        }

        Employees employeesInJson = JsonUtility.FromJson<Employees>(jjson);
 //       Debug.Log("How many values ? :: ---------------- " + employeesInJson.accuracy + " I am : " + employeesInJson.headshots);
        foreach (FineMotor employee in employeesInJson.player)
        {
            Debug.Log("Found employee: " + employee);// + " " + employee.lastName);
        }
    }
    void Start()
    {

        
            
        
        //calculate the gap between the bars
        gap = (WIDTH - barWidth * temps.Length) / (temps.Length + 1);

        float startX = -WIDTH / 2f;
        float accum = startX;


        Vector3[] positions = new Vector3[temps.Length];

        for(int i = 0; i < temps.Length; i++){
            float barHeight = temps[i] / max * HEIGHT;
     //       Debug.Log("inside for loop " + i);
            Bar pos = Instantiate(bar);
            pos.SetDimension(new Vector3(1f, barHeight, 1f));
            pos.gameObject.transform.position = new Vector3(accum + pos.gameObject.transform.localScale.x / 2, 
                                                            pos.gameObject.transform.position.y + pos.gameObject.transform.localScale.y / 2,
                                                            pos.gameObject.transform.position.z);
            accum += (barWidth + 2 * gap);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
