using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
//using System.Diagnostics;
using System.IO;
using System.Text;
//using System.Diagnostics;
//using System.Diagnostics;
using UnityEngine;
using System;
using static UnityEditor.PlayerSettings;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Bar_Chart_Controller : MonoBehaviour
{
    private int[] temps = {54, 46, 34, 50, 24, 26, 88, 63, 73};
    private float max = 88;
    [SerializeField] private GameObject line;
    private LineRenderer lineRendererFMAcc;
    [SerializeField] private float WIDTH, HEIGHT; // Drawing canvas (Strictly speaking does not include spaces for drawing labels, legends, etc.)
    [SerializeField] private float barWidth = 1f, barDepth = 1f;
     private float gap;
    private string jjson = null;

    private TestsJSON fineMotorJson;
    
    [SerializeField] Bar bar;
    // Start is called before the first frame update
    void Awake()
    {
        using (StreamReader r = new StreamReader("finemotor1.json"))
        {
            jjson = r.ReadToEnd();
        }

        fineMotorJson = JsonUtility.FromJson<TestsJSON>(jjson);
        Debug.Log(jjson);
        foreach (TestsBoxer player in fineMotorJson.player)
        {
            if(player.audio != null)
            Debug.Log("Rodrigo ==+++++++++++++++++++== " + player.fineMotor.accuracy);
            //Debug.Log("Found employee: " + player.fineMotor.accuracy + " Average tracking time : " + player.fineMotor.avgTrackingTime + "\n" + player.timestamp.value.getTimeStamp());// + " " + employee.lastName);
        }
      //  Debug.Log("Maximum value :----------------------------------------: " + getMaximumAndMinimum(fineMotorJson));
    }

    private Vector2 getMaximumAndMinimum(TestsJSON playerJson)
    {
        TestsBoxer[] playerBoxer = playerJson.player;
        Array.Sort(playerBoxer, (a, b) =>
        {
            FineMotor fineA = a.fineMotor;
            FineMotor fineB = b.fineMotor;
            return (int)(fineB.avgTrackingTime - fineA.avgTrackingTime);
        });
        return new Vector2((float)playerBoxer[0].fineMotor.avgTrackingTime, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgTrackingTime);
    }

    private Dictionary<DateTime, TestsBoxer> getFineMotorTests(TestsJSON playerJson)
    {
        TestsBoxer[] boxers = playerJson.player;
        Array.Sort(boxers, (a, b) =>
        {
            TimeSecs tw1 = a.timestamp.value;
            TimeSecs tw2 = b.timestamp.value;
            return DateTime.Compare(tw1.getTimeStamp(), tw2.getTimeStamp());
        });

  //      Dictionary<DateTime, FineMotor> dict = new Dictionary<DateTime, FineMotor>();
        Dictionary<DateTime, TestsBoxer> dict = new Dictionary<DateTime, TestsBoxer>();
        for(int i = 0; i < boxers.Length; i++)
        {
       //     if(boxers[i].fineMotor != null)
         //   {
                dict.Add(boxers[i].timestamp.value.getTimeStamp(), boxers[i]);
       //     }
        }
        return dict;
    }
    void Start()
    {
        lineRendererFMAcc = line.AddComponent<LineRenderer>();
        
        Dictionary<DateTime, TestsBoxer> data = getFineMotorTests(fineMotorJson);


   //     lineRendererFMAcc.positionCount = data.Count;
        lineRendererFMAcc.widthMultiplier = 0.1f;
        lineRendererFMAcc.material.color = Color.red;
        lineRendererFMAcc.textureMode = LineTextureMode.RepeatPerSegment;
        lineRendererFMAcc.material.SetTextureScale("_MainTex", new Vector2(0.025f, 1f));

        Debug.Log("We are print " + data.Count + " data");
        Vector2 minMax = getMaximumAndMinimum(fineMotorJson);
        float maxValue = minMax.x;
        float minValue = minMax.y;

        Debug.Log("-------------------------   Max: " + maxValue + " Min: " + minValue);

        maxValue += (maxValue * 0.05f);
        minValue -= (maxValue * 0.05f);
        if(minValue < 0.0f) minValue = 0.0f;

     //   gap = (WIDTH - barWidth * temps.Length) / (temps.Length + 1);

        float startX = -WIDTH / 2f;
        float accum = startX;

        int maxDisplayable = 52;

        //        WIDTH = (maxDisplayable * barWidth) + ((maxDisplayable - 1) * (barWidth / 2f));


        float barWidth = (2 * WIDTH) / (3 * maxDisplayable - 1);
        gap = barWidth / 2f;
        barWidth /= 3f;
        int i = -1;
        List<Vector3> positions = new List<Vector3>();   
        foreach(KeyValuePair<DateTime, TestsBoxer> pair in data)
        {
            DateTime timestamp = pair.Key;

            if (pair.Value.fineMotor != null)
            {
                Debug.Log("FineMotor = " + pair.Value.fineMotor);
                double accuracy = pair.Value.fineMotor.accuracy;

                float lineHeight = (float)(accuracy / 100 * HEIGHT);
                if (accuracy > Mathf.Epsilon)
                {
                    //        lineRendererFMAcc.SetPosition(++i, new Vector3(accum, lineHeight, 0));

                    Vector3 pos = new Vector3(accum, lineHeight, 0);
                    positions.Add(pos);
                    Debug.Log("Count ---------------------------- " + ++i + " -------------- " + pos);
                    lineRendererFMAcc.positionCount = i + 1;
                }
                double avgTrackingTime = pair.Value.fineMotor.avgTrackingTime;
                float barHeight = (float)(HEIGHT * (avgTrackingTime - minValue) / (maxValue - minValue));
                Bar value = Instantiate(bar);
                value.GetComponent<Renderer>().material.color = Color.red;
                value.SetDimension(new Vector3(barWidth, barHeight, 0.25f));
                value.gameObject.transform.position = new Vector3(accum + value.gameObject.transform.localScale.x / 2,
                value.gameObject.transform.position.y + value.gameObject.transform.localScale.y / 2,
                                                                value.gameObject.transform.position.z);
            }
            accum += barWidth;
            if(pair.Value.visual != null && pair.Value.visual.responseTimes != null)
            {
                double avgResponseTime = pair.Value.visual.getAverageResponseTime();
                float barHeight = (float)(HEIGHT * (avgResponseTime - minValue) / (maxValue - minValue));
                Bar value = Instantiate(bar);
                value.GetComponent<Renderer>().material.color = Color.blue;
                value.SetDimension(new Vector3(barWidth, barHeight, 0.25f));
                value.gameObject.transform.position = new Vector3(accum + value.gameObject.transform.localScale.x / 2,
                value.gameObject.transform.position.y + value.gameObject.transform.localScale.y / 2,
                                                                value.gameObject.transform.position.z);
            }
            accum += barWidth;
            if (pair.Value.audio != null)
            {
                double avgResponseTime = pair.Value.audio.avgResponseTime / 1000;
                float barHeight = (float)(HEIGHT * (avgResponseTime - minValue) / (maxValue - minValue));
                Bar value = Instantiate(bar);
                value.GetComponent<Renderer>().material.color = Color.yellow;
                value.SetDimension(new Vector3(barWidth, barHeight, 0.25f));
                value.gameObject.transform.position = new Vector3(accum + value.gameObject.transform.localScale.x / 2,
                value.gameObject.transform.position.y + value.gameObject.transform.localScale.y / 2,
                                                                value.gameObject.transform.position.z);
            }
            accum += (barWidth + gap);
        }
        lineRendererFMAcc.SetPositions(positions.ToArray());
/*        Debug.Log("Point count; " + lineRendererFMAcc.positionCount + positions.ToCommaSeparatedString());
        lineRendererFMAcc.Simplify(0f);
        Debug.Log("Point count; " + lineRendererFMAcc.positionCount + positions.ToCommaSeparatedString());
*/    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
