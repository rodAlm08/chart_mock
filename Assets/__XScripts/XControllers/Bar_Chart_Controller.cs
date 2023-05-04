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
    public static Vector2 MAX_MIN_FM_AVG_TRK_TIME, MAX_MIN_FM_AVG_DST, MAX_MIN_AUDIO_THRESH, MAX_MIN_AUDIO_AVG_RES_TIME;
    public static TestsBoxer[] data;

    private int[] temps = { 54, 46, 34, 50, 24, 26, 88, 63, 73 };
    private float max = 88;
    [SerializeField] private GameObject fmAccLine;
    [SerializeField] private GameObject fmAvgDistLine;
    [SerializeField] private GameObject audioThreshLine;
    [SerializeField] private GameObject visualShotAccLine;
    [SerializeField] private GameObject visualTargetAccLine;
    [SerializeField] private LineRenderer gridLinePrefab;
    private LineRenderer lineRendererFMAcc;
    private LineRenderer lineRendererAvgDist;
    private LineRenderer lineRendererAudioThresh;
    private LineRenderer lineRendererVisualShotAcc;
    private LineRenderer lineRendererVisualTargetAcc;
    //   private LineRenderer lineRendererGrid;
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
            if (player.audio != null) { }
            //       Debug.Log("Rodrigo ==+++++++++++++++++++== " + player.fineMotor.accuracy);
            //Debug.Log("Found employee: " + player.fineMotor.accuracy + " Average tracking time : " + player.fineMotor.avgTrackingTime + "\n" + player.timestamp.value.getTimeStamp());// + " " + employee.lastName);
        }
        //  Debug.Log("Maximum value :----------------------------------------: " + getMaximumAndMinimum(fineMotorJson));
    }
    public const int FM_AVG_TRACK_TIME = 0, FM_AVERAGE_DISTANCE = 1, AUDIO_THRESHOLD = 2, VISUAL_SHOT_ACC = 3, VISUAL_TARGET_ACC = 4;
    private Vector2 getMaximumAndMinimum(TestsJSON playerJson, int attribute)
    {
        TestsBoxer[] playerBoxer = playerJson.player;

        Array.Sort(playerBoxer, (a, b) =>
        {
            FineMotor fineA = a.fineMotor;
            FineMotor fineB = b.fineMotor;
            if (attribute == FM_AVG_TRACK_TIME)
                return (int)(fineB.avgTrackingTime - fineA.avgTrackingTime);
            //    return (int)(fineB.accuracy - fineA.accuracy);
            else if (attribute == FM_AVERAGE_DISTANCE)
            {
                //       Debug.Log("Testing ================== " + fineA.avgDistanceFromPlayer + " =============== " + fineB.avgDistanceFromPlayer);
                return (int)(fineB.avgDistanceFromPlayer - fineA.avgDistanceFromPlayer);
            }
            Audio audioA = a.audio;
            Audio audioB = b.audio;
            if (attribute == AUDIO_THRESHOLD)
            {
                //       Debug.Log("Audio wahala -------------------- " + audioA.minSoundThreshold + " --------------- " + audioB.minSoundThreshold);
                return (int)(audioB.minSoundThreshold - audioA.minSoundThreshold);
            }
            Visual visualA = a.visual;
            Visual visualB = b.visual;
            if (attribute == VISUAL_SHOT_ACC)
            {
                //       Debug.Log("Visual wahala ----------------- " + visualA.shotAccuracy + " ----------------- " + visualB.shotAccuracy);
                return (int)(visualB.shotAccuracy - visualA.shotAccuracy);
            }
            else if (attribute == VISUAL_TARGET_ACC)
            {
                //       Debug.Log("Visual wahala 1 ----------------- " + visualA.targetAccuracy + " ----------------- " + visualB.targetAccuracy);
                return (int)(visualB.targetAccuracy - visualA.targetAccuracy);
            }
            else return 0;
        });
        if (attribute == FM_AVG_TRACK_TIME)
        {

            Bar_Chart_Controller.MAX_MIN_FM_AVG_TRK_TIME = new Vector2((float)playerBoxer[0].fineMotor.avgTrackingTime, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgTrackingTime);
            //       Debug.Log("This is what i have: " + Bar_Chart_Controller.MAX_MIN_FM_AVG_TRK_TIME);
            return new Vector2((float)playerBoxer[0].fineMotor.avgTrackingTime, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgTrackingTime);
        }
        else if (attribute == FM_AVERAGE_DISTANCE)
        {
            Bar_Chart_Controller.MAX_MIN_FM_AVG_DST = new Vector2((float)playerBoxer[0].fineMotor.avgDistanceFromPlayer, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgDistanceFromPlayer);
            //      Debug.Log("This is what i have: " + Bar_Chart_Controller.MAX_MIN_FM_AVG_DST);

            return new Vector2((float)playerBoxer[0].fineMotor.avgDistanceFromPlayer, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgDistanceFromPlayer);
        }
        else if (attribute == AUDIO_THRESHOLD)
        {
            Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH = new Vector2((float)playerBoxer[0].audio.minSoundThreshold, (float)playerBoxer[playerBoxer.Length - 1].audio.minSoundThreshold);
            //      Debug.Log("This is what i have for AUDIO: " + Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH);

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

        Bar_Chart_Controller.data = new TestsBoxer[boxers.Length];

        //      Dictionary<DateTime, FineMotor> dict = new Dictionary<DateTime, FineMotor>();
        Dictionary<DateTime, TestsBoxer> dict = new Dictionary<DateTime, TestsBoxer>();
        for (int i = 0; i < boxers.Length; i++)
        {
            //     if(boxers[i].fineMotor != null)
            //   {
            //Debug.Log("DIctionary Sorteddddddd i = "+i + "  " + data[i].timestamp.value.getTimeStamp() + "  "+data[i].timestamp.value.getTimeStamp().DayOfYear / 7  );
            //        Debug.Log("Printing Timestamp --- " + boxers[i].timestamp.value.getTimeStamp());
            dict.Add(boxers[i].timestamp.value.getTimeStamp(), boxers[i]);
            Bar_Chart_Controller.data[i] = boxers[i];
            //     }
        }

        return dict;
    }


    private void drawGrid(int numX, int numY)
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Grid");
        float startY = 0;// -HEIGHT / 2f;
        float startX = -WIDTH / 2f;
        float depth = 20f;
        for (int i = 0; i < numX + 1; i++)
        {
            LineRenderer gridLine = Instantiate(gridLinePrefab, obj.transform);

            gridLine.positionCount = 4;

            gridLine.SetPosition(0, new Vector3(startX, startY + (i * HEIGHT / numX), 0f));
            gridLine.SetPosition(1, new Vector3(startX, startY + (i * HEIGHT / numX), depth));
            gridLine.SetPosition(2, new Vector3(startX + WIDTH, startY + (i * HEIGHT / numX), depth));
            gridLine.SetPosition(3, new Vector3(startX + WIDTH, startY + (i * HEIGHT / numX), 0f));

            //     gridLine.positionCount = 4 + 1;

        }
        return;
        for (int i = 0; i < numY + 1; i++)
        {
            LineRenderer gridLine = Instantiate(gridLinePrefab, obj.transform);
            gridLine.positionCount = 4;
            gridLine.SetPosition(0, new Vector3(startX + (i * WIDTH / numY), startY, 0f));
            gridLine.SetPosition(1, new Vector3(startX + (i * WIDTH / numY), startY, depth));
            gridLine.SetPosition(2, new Vector3(startX + (i * WIDTH / numY), startY + HEIGHT, depth));
            gridLine.SetPosition(3, new Vector3(startX + (i * WIDTH / numY), startY + HEIGHT, 0f));
        }
    }


    void OnEnable()
    {

        lineRendererFMAcc = fmAccLine.GetComponent<LineRenderer>();
        lineRendererAvgDist = fmAvgDistLine.GetComponent<LineRenderer>();
        lineRendererAudioThresh = audioThreshLine.GetComponent<LineRenderer>();
        lineRendererVisualShotAcc = visualShotAccLine.GetComponent<LineRenderer>();
        lineRendererVisualTargetAcc = visualTargetAccLine.GetComponent<LineRenderer>();


        Dictionary<DateTime, TestsBoxer> data = getFineMotorTests(fineMotorJson);



        Vector2 minMax = getMaximumAndMinimum(fineMotorJson, FM_AVG_TRACK_TIME);
        float maxValue = minMax.x;
        float minValue = minMax.y;

        
        float startX = -WIDTH / 2f;
        float startY = 0;
        float accum = startX;

        float maxDisplayable = 52f;

        float barWidth = WIDTH / (4 * maxDisplayable - 1) * (maxDisplayable / data.Count);
        gap = barWidth;

        float lineThickness = 2.5f * (maxDisplayable / data.Count);

        Debug.Log("Width: " + WIDTH + " Nmax: " + maxDisplayable + " barWidth: " + barWidth + " Space: " + gap + " Number to display: " + data.Count);
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

        foreach (KeyValuePair<DateTime, TestsBoxer> pair in data)
        {
            DateTime timestamp = pair.Key;

            if (pair.Value.fineMotor != null)
            {
                double accuracy = pair.Value.fineMotor.accuracy;
                double avgDistance = pair.Value.fineMotor.avgDistanceFromPlayer;
                Vector2 mnmx = getMaximumAndMinimum(fineMotorJson, FM_AVERAGE_DISTANCE);
                float mn = mnmx.y;
                float mx = mnmx.x;
                if (accuracy > Mathf.Epsilon)
                {
                    i++;
                    startY = 0;
                    float fmAccLineHeight = (float)(accuracy / 100 * HEIGHT);
                    Vector3 pos = new Vector3(startY + accum, fmAccLineHeight, 0);
                    accPositions.Add(pos);
                    lineRendererFMAcc.positionCount = i + 1;
                }
                if (avgDistance > Mathf.Epsilon)
                {
                    startY = mn * -1;
                    float fmAvgDistLineHeight = startY + mn + HEIGHT * (float)(avgDistance - mn) / (mx - mn);
                    j++;
                    Vector3 avgD = new Vector3(accum, fmAvgDistLineHeight, 0);
                    avgDistPositions.Add(avgD);
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
            accum += barWidth;
            if (pair.Value.audio != null)
            {
                double minSoundThresh = pair.Value.audio.minSoundThreshold;
                minMax = getMaximumAndMinimum(fineMotorJson, AUDIO_THRESHOLD);
                float min = minMax.y;
                float max = minMax.x;
                startY = min * -1;
                if (true)
                {
                    float audioThreshLineHeight = startY + min + HEIGHT * (float)(minSoundThresh - min) / (max - min);
                    k++;
                    Vector3 thresh = new Vector3(accum, audioThreshLineHeight, 0);
                    minSoundPositions.Add(thresh);
                    lineRendererAudioThresh.positionCount = k + 1;
                }
                double avgResponseTime = pair.Value.audio.avgResponseTime / 1000;
                float barHeight = (float)(HEIGHT * (avgResponseTime - minValue) / (maxValue - minValue));
                Bar value = Instantiate(bar);
                value.GetComponent<Renderer>().material.color = Color.yellow;
                value.SetDimension(new Vector3(barWidth, barHeight, 0.25f));
                value.gameObject.transform.position = new Vector3(accum + value.gameObject.transform.localScale.x / 2,
                value.gameObject.transform.position.y + value.gameObject.transform.localScale.y / 2,
                                                                value.gameObject.transform.position.z);
            }
            accum += barWidth;
            if (pair.Value.visual != null)
            {
                double visualShotAcc = pair.Value.visual.shotAccuracy;
                if (visualShotAcc > Mathf.Epsilon)
                {
                    startY = 0;
                    float visualShotAccHeight = (float)(visualShotAcc / 100 * HEIGHT);

                    l++;
                    Vector3 shotAcc = new Vector3(accum, visualShotAccHeight, 0);
                    visualShotAccPos.Add(shotAcc);
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
                    lineRendererVisualTargetAcc.positionCount = m + 1;
                }

                if (pair.Value.visual.responseTimes != null)
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

            }
 //           accum += barWidth;
            accum += (barWidth + gap);
        }
        lineRendererFMAcc.SetWidth(lineThickness, lineThickness);
        lineRendererAvgDist.SetWidth(lineThickness, lineThickness);
        lineRendererAudioThresh.SetWidth(lineThickness, lineThickness);
        lineRendererVisualShotAcc.SetWidth(lineThickness, lineThickness);
        lineRendererVisualTargetAcc.SetWidth(lineThickness, lineThickness);

        lineRendererFMAcc.SetPositions(accPositions.ToArray());
        lineRendererAvgDist.SetPositions(avgDistPositions.ToArray());
        lineRendererAudioThresh.SetPositions(minSoundPositions.ToArray());
        lineRendererVisualShotAcc.SetPositions(visualShotAccPos.ToArray());
        lineRendererVisualTargetAcc.SetPositions(visualTargetAccPos.ToArray());

    }

    // Update is called once per frame
    void Update()
    {

    }
}
