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
//using static UnityEditor.PlayerSettings;
//using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Bar_Chart_Controller : MonoBehaviour
{
    private int[] temps = {54, 46, 34, 50, 24, 26, 88, 63, 73};
    private float max = 88;
    [SerializeField] private GameObject fmAccLine;
    [SerializeField] private GameObject fmAvgDistLine;
    [SerializeField] private GameObject audioThreshLine;
    [SerializeField] private GameObject visualShotAccLine;
    [SerializeField] private GameObject visualTargetAccLine;
    private LineRenderer lineRendererFMAcc;
    private LineRenderer lineRendererAvgDist;
    private LineRenderer lineRendererAudioThresh;
    private LineRenderer lineRendererVisualShotAcc;
    private LineRenderer lineRendererVisualTargetAcc;
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
    public const int FM_ACCURACY = 0, FM_AVERAGE_DISTANCE = 1, AUDIO_THRESHOLD = 2, VISUAL_SHOT_ACC = 3, VISUAL_TARGET_ACC = 4;
    private Vector2 getMaximumAndMinimum(TestsJSON playerJson, int attribute)
    {
        TestsBoxer[] playerBoxer = playerJson.player;
        Array.Sort(playerBoxer, (a, b) =>
        {
            FineMotor fineA = a.fineMotor;
            FineMotor fineB = b.fineMotor;
            if (attribute == FM_ACCURACY)
                return (int)(fineB.avgTrackingTime - fineA.avgTrackingTime);
            else if (attribute == FM_AVERAGE_DISTANCE)
            {
                Debug.Log("Testing ================== " + fineA.avgDistanceFromPlayer + " =============== " + fineB.avgDistanceFromPlayer);
                return (int)(fineB.avgDistanceFromPlayer - fineA.avgDistanceFromPlayer);
            }
            Audio audioA = a.audio;
            Audio audioB = b.audio;
            if (attribute == AUDIO_THRESHOLD)
            {
                Debug.Log("Audio wahala -------------------- " + audioA.minSoundThreshold + " --------------- " + audioB.minSoundThreshold);
                return (int)(audioB.minSoundThreshold - audioA.minSoundThreshold);
            }
            Visual visualA = a.visual;
            Visual visualB = b.visual;
            if (attribute == VISUAL_SHOT_ACC)
            {
                Debug.Log("Visual wahala ----------------- " + visualA.shotAccuracy + " ----------------- " + visualB.shotAccuracy);
                return (int)(visualB.shotAccuracy - visualA.shotAccuracy);
            } 
            else if (attribute == VISUAL_TARGET_ACC){
                Debug.Log("Visual wahala 1 ----------------- " + visualA.targetAccuracy + " ----------------- " + visualB.targetAccuracy);
                return (int)(visualB.targetAccuracy - visualA.targetAccuracy);
            }
            else return 0;
        });
        if (attribute == FM_ACCURACY)
        {
            return new Vector2((float)playerBoxer[0].fineMotor.avgTrackingTime, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgTrackingTime);
        }
        else if (attribute == FM_AVERAGE_DISTANCE)
        {
            return new Vector2((float)playerBoxer[0].fineMotor.avgDistanceFromPlayer, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgDistanceFromPlayer);
        }
        else if (attribute == AUDIO_THRESHOLD) {
            return new Vector2((float)playerBoxer[0].audio.minSoundThreshold, (float)playerBoxer[playerBoxer.Length - 1].audio.minSoundThreshold);
        }
        else if (attribute == VISUAL_SHOT_ACC)
        {
            return new Vector2((float)playerBoxer[0].visual.shotAccuracy, (float)playerBoxer[playerBoxer.Length - 1].visual.shotAccuracy);
        }
        else if (attribute == VISUAL_TARGET_ACC)
        {
            return new Vector2((float)playerBoxer[0].visual.targetAccuracy, (float)playerBoxer[playerBoxer.Length - 1].visual.targetAccuracy);
        }
        else return new Vector2();
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


        //     lineRendererFMAcc = GetComponent<LineRenderer>();
        //    lineRendererAvgDist = GetComponent<LineRenderer>();

        /*      lineRendererFMAcc = fmAccLine.AddComponent<LineRenderer>();
              lineRendererAvgDist = fmAvgDistLine.AddComponent<LineRenderer>();
              lineRendererAudioThresh = audioThreshLine.AddComponent<LineRenderer>();
              lineRendererVisualShotAcc = visualShotAccLine.AddComponent<LineRenderer>();
      */

        lineRendererFMAcc = fmAccLine.GetComponent<LineRenderer>();
        lineRendererAvgDist = fmAvgDistLine.GetComponent<LineRenderer>();
        lineRendererAudioThresh = audioThreshLine.GetComponent<LineRenderer>();
        lineRendererVisualShotAcc = visualShotAccLine.GetComponent<LineRenderer>();
        lineRendererVisualTargetAcc = visualTargetAccLine.GetComponent<LineRenderer>();


        Dictionary<DateTime, TestsBoxer> data = getFineMotorTests(fineMotorJson);


   //     lineRendererFMAcc.positionCount = data.Count;
    /*    lineRendererFMAcc.widthMultiplier = 0.1f;
        lineRendererFMAcc.material.color = Color.red;
        lineRendererFMAcc.textureMode = LineTextureMode.RepeatPerSegment;
        lineRendererFMAcc.material.SetTextureScale("_MainTex", new Vector2(0.025f, 1f));

        lineRendererAvgDist.widthMultiplier = 0.1f;
        lineRendererAvgDist.material.color = new Color(255, 32, 32);
        lineRendererAvgDist.textureMode = LineTextureMode.RepeatPerSegment;
        lineRendererAvgDist.material.SetTextureScale("_MainTex", new Vector2(0.025f, 1f));

        lineRendererAudioThresh.widthMultiplier = 0.1f;
        lineRendererAudioThresh.material.color = new Color(255, 255, 32);
        lineRendererAudioThresh.textureMode = LineTextureMode.RepeatPerSegment;
        lineRendererAudioThresh.material.SetTextureScale("_MainTex", new Vector2(0.025f, 1f));

        lineRendererVisualShotAcc.widthMultiplier = 0.1f;
        lineRendererVisualShotAcc.material.color = new Color(32, 255, 255);
        lineRendererVisualShotAcc.textureMode = LineTextureMode.RepeatPerSegment;
        lineRendererVisualShotAcc.material.SetTextureScale("_MainTex", new Vector2(0.025f, 1f));
*/



        Debug.Log("We are print " + data.Count + " data");
        Vector2 minMax = getMaximumAndMinimum(fineMotorJson, FM_ACCURACY);
        float maxValue = minMax.x;
        float minValue = minMax.y;

        Debug.Log("-------------------------   Max: " + maxValue + " Min: " + minValue);

        maxValue += (maxValue * 0.05f);
        minValue -= (maxValue * 0.05f);
        if(minValue < 0.0f) minValue = 0.0f;

     //   gap = (WIDTH - barWidth * temps.Length) / (temps.Length + 1);

        float startX = -WIDTH / 2f;
        float startY = 0;
        float accum = startX;

        int maxDisplayable = 52;

        //        WIDTH = (maxDisplayable * barWidth) + ((maxDisplayable - 1) * (barWidth / 2f));


        float barWidth = (2 * WIDTH) / (3 * maxDisplayable - 1);
        gap = barWidth / 2f;
        barWidth /= 3f;
        int i = -1;
        int j = -1;
        int k = -1;
        int l = -1;
        int m = -1;
        List<Vector3> accPositions = new List<Vector3>();
        List<Vector3> avgDistPositions = new List<Vector3>();
        List<Vector3> minSoundPositions = new List<Vector3>();
        List<Vector3> visualShotAccPos = new List<Vector3>();
        List<Vector3> visualTargetAccPos = new List<Vector3>();

        foreach(KeyValuePair<DateTime, TestsBoxer> pair in data)
        {
            DateTime timestamp = pair.Key;

            if (pair.Value.fineMotor != null)
            {
                Debug.Log("FineMotor = " + pair.Value.fineMotor);
                double accuracy = pair.Value.fineMotor.accuracy;
                double avgDistance = pair.Value.fineMotor.avgDistanceFromPlayer;
                

                Vector2 mnmx = getMaximumAndMinimum(fineMotorJson, FM_AVERAGE_DISTANCE);
                float mn = mnmx.y;
                float mx = mnmx.x;
                Debug.Log("AVG --++++++++++++++++++++++++++++ " + mn + "  ===================== " + mx);

                // float 
                if (accuracy > Mathf.Epsilon)
                {
                    //        lineRendererFMAcc.SetPosition(++i, new Vector3(accum, lineHeight, 0));
                    i++;
                    startY = 0;
                    float fmAccLineHeight = (float)(accuracy / 100 * HEIGHT);
                    Vector3 pos = new Vector3(startY + accum, fmAccLineHeight, 0);
                    accPositions.Add(pos);
                    Debug.Log("Count ---------------------------- " + i + " -------------- " + pos);
                    lineRendererFMAcc.positionCount = i + 1;
                }
                if(avgDistance > Mathf.Epsilon)
                {
                    startY = mn * -1;
                    float fmAvgDistLineHeight = startY + mn + HEIGHT * (float)(avgDistance - mn) / (mx - mn);

                    j++;
                    Vector3 avgD = new Vector3(accum, fmAvgDistLineHeight, 0);
                    avgDistPositions.Add(avgD);
                    Debug.Log("County ------------------------ " + j + " --------------- " + avgD);
                    Debug.Log("County " + avgDistance + " Height: " + fmAvgDistLineHeight);
                    lineRendererAvgDist.positionCount = j + 1;
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
            if(pair.Value.audio != null )
                
            {
                
                double minSoundThresh = pair.Value.audio.minSoundThreshold;
                //Debug.Log("Audio = " + minSoundThresh);
                minMax = getMaximumAndMinimum(fineMotorJson, AUDIO_THRESHOLD);
                float min = minMax.y;
                float max = minMax.x;
                startY = min * -1;
                Debug.Log("Audio min ================================================== " + minMax + "treshhold >>>>>>>>" + minSoundThresh);
                if (true)
                {
                    float audioThreshLineHeight = startY + min + HEIGHT * (float)(minSoundThresh - min) / (max - min);
                    k++;
                    Vector3 thresh = new Vector3(accum, audioThreshLineHeight, 0);
                    minSoundPositions.Add(thresh);
                    lineRendererAudioThresh.positionCount = k + 1;
                }
            }
            accum += barWidth;
            if(pair.Value.visual != null)
            {
                double visualShotAcc = pair.Value.visual.shotAccuracy;
                if (visualShotAcc > Mathf.Epsilon)
                {
                    startY = 0;
                    float visualShotAccHeight = (float)(visualShotAcc / 100 * HEIGHT);

                    l++;
                    Vector3 shotAcc = new Vector3(accum, visualShotAccHeight, 0);
                    visualShotAccPos.Add(shotAcc);
                    Debug.Log("County ------------------------ " + l + " --------------- " + shotAcc);
                    Debug.Log("County " + visualShotAcc + " Height: " + visualShotAccHeight);
                    lineRendererVisualShotAcc.positionCount = l + 1;
                }

                double visualTargetAcc = pair.Value.visual.targetAccuracy;
                if (visualTargetAcc > Mathf.Epsilon)
                {
                    startY = 0;
                    float visualTargetAccHeight = (float)(visualTargetAcc / 100 * HEIGHT);

                    m++;
                    Vector3 targetAcc = new Vector3(accum, visualTargetAccHeight, 0);
                    visualTargetAccPos.Add(targetAcc);
                    Debug.Log("Mounty ------------------------ " + m + " --------------- " + targetAcc);
                    Debug.Log("Mounty " + visualTargetAcc + " Height: " + visualTargetAccHeight);
                    lineRendererVisualTargetAcc.positionCount = m + 1;
                }

                if (pair.Value.visual.responseTimes != null) {
                    double avgResponseTime = pair.Value.visual.getAverageResponseTime();
                    float barHeight = (float)(HEIGHT * (avgResponseTime - minValue) / (maxValue - minValue));
                    Bar value = Instantiate(bar);
                    value.GetComponent<Renderer>().material.color = Color.blue;
                    value.SetDimension(new Vector3(barWidth, barHeight, 0.25f));
                    value.gameObject.transform.position = new Vector3(accum + value.gameObject.transform.localScale.x / 2,
                    value.gameObject.transform.position.y + value.gameObject.transform.localScale.y / 2,
                                                                    value.gameObject.transform.position.z);
                }

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
        lineRendererFMAcc.SetPositions(accPositions.ToArray());
        lineRendererAvgDist.SetPositions(avgDistPositions.ToArray());
        lineRendererAudioThresh.SetPositions(minSoundPositions.ToArray());
        lineRendererVisualShotAcc.SetPositions(visualShotAccPos.ToArray());
        lineRendererVisualTargetAcc.SetPositions(visualTargetAccPos.ToArray());
/*        Debug.Log("Point count; " + lineRendererFMAcc.positionCount + positions.ToCommaSeparatedString());
        lineRendererFMAcc.Simplify(0f);
        Debug.Log("Point count; " + lineRendererFMAcc.positionCount + positions.ToCommaSeparatedString());
*/    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
