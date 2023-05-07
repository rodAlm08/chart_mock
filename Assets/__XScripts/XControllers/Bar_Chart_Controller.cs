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
using System.Linq;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using TMPro;

public class Bar_Chart_Controller : MonoBehaviour
{
    //    public static Vector2 MAX_MIN_FM_AVG_TRK_TIME, MAX_MIN_FM_AVG_DST, MAX_MIN_AUDIO_THRESH, MAX_MIN_AUDIO_AVG_RES_TIME;
    //   public static TestsBoxer[] data;
    public static Dictionary<DateTime, TestsBoxer> DictData;

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
    [SerializeField] private float WIDTH, HEIGHT; // Drawing canvas (Strictly speaking does not include spaces for drawing labels, legends, etc.)
    [SerializeField] private float barWidth = 1f, barDepth = 1f;
    private float gap;
    private string jjson = null;
    public static TestsJSON fineMotorJson;
    public static TestsBoxer[] data;
    [SerializeField] Bar bar;
    public static Bar_Chart_Controller instance;
    public const int FM_AVG_TRK_TME = 0b0000, FM_ACC = 0b0001, FM_AVG_DST = 0b0010, AUDIO_TSH = 0b0101, AUDIO_RSP_TME = 0b0100, VISUAL_SHT_ACC = 0b1001, VISUAL_TGT_ACC = 0b1010, VISUAL_AVG_TRK_TME = 0b1000;
    public const int FINE_MOTOR = 0b00, AUDIO = 0b01, VISUAL = 0b10;
    public Vector2 TimeMaxMin, DistanceMaxMin, SoundMaxMin;

    public delegate void DestroyBar();
    public DestroyBar ClearScreenEvent;
    protected void OnScreenCleared()
    {
        ClearScreenEvent?.Invoke();
    }

    public delegate void RefreshLabel();
    public RefreshLabel RefreshLabelEvent;
    protected void OnLabelsChanged()
    {
        RefreshLabelEvent?.Invoke();
    }





    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        LoadData();
    }


    void OnEnable()
    {
        DictData = GetTestData(fineMotorJson);
        LoadMaxMin();
        DrawCharts();
        /*       

               Vector2 minMax = getMaximumAndMinimum(Bar_Chart_Controller.data, FM_AVG_TRACK_TIME);

               lineRendererFMAcc = fmAccLine.GetComponent<LineRenderer>();
               lineRendererAvgDist = fmAvgDistLine.GetComponent<LineRenderer>();
               lineRendererAudioThresh = audioThreshLine.GetComponent<LineRenderer>();
               lineRendererVisualShotAcc = visualShotAccLine.GetComponent<LineRenderer>();
               lineRendererVisualTargetAcc = visualTargetAccLine.GetComponent<LineRenderer>();


               float maxValue = minMax.x;
               float minValue = minMax.y;

               float startX = -WIDTH / 2f;
               float startY = 0;
               float accum = startX;

               float maxDisplayable = 52f;

               float barWidth = WIDTH / (4 * maxDisplayable - 1) * (maxDisplayable / DictData.Count);
               gap = barWidth;

               float lineThickness = 2.5f * (maxDisplayable / DictData.Count);

               Debug.Log("Width: " + WIDTH + " Nmax: " + maxDisplayable + " barWidth: " + barWidth + " Space: " + gap + " Number to display: " + DictData.Count);
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

               foreach (KeyValuePair<DateTime, TestsBoxer> pair in DictData)
               {
                   DateTime timestamp = pair.Key;

                   if (pair.Value.fineMotor != null)
                   {
                       double accuracy = pair.Value.fineMotor.accuracy;
                       double avgDistance = pair.Value.fineMotor.avgDistanceFromPlayer;
                       Vector2 mnmx = getMaximumAndMinimum(Bar_Chart_Controller.data, FM_AVERAGE_DISTANCE);
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
                       minMax = getMaximumAndMinimum(Bar_Chart_Controller.data, AUDIO_THRESHOLD);
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
       */
    }

    private void LoadData()
    {
        using (StreamReader r = new StreamReader("finemotor1.json"))
        {
            jjson = r.ReadToEnd();
        }
        fineMotorJson = JsonUtility.FromJson<TestsJSON>(jjson);
    }

    private void LoadMaxMin()
    {
        Vector2 fmTime = GetMaximumMinimum((TestsBoxer[])Bar_Chart_Controller.data.Clone(), FM_AVG_TRK_TME);
        Debug.Log("%%FMTIME: " + fmTime);
        Vector2 audioTime = GetMaximumMinimum((TestsBoxer[])Bar_Chart_Controller.data.Clone(), AUDIO_RSP_TME);
        Debug.Log("%%AudioTime: :" + audioTime);
        Vector2 visualTime = GetMaximumMinimum((TestsBoxer[])Bar_Chart_Controller.data.Clone(), VISUAL_AVG_TRK_TME);
        Debug.Log("%%Visual time: " + visualTime);
        TimeMaxMin = new Vector2(Mathf.Max(fmTime.x, audioTime.x / 1000, visualTime.x), Mathf.Min(fmTime.y, audioTime.y / 1000, visualTime.y));
        Debug.Log("%%TimeMaxMin: " + TimeMaxMin);
        DistanceMaxMin = GetMaximumMinimum((TestsBoxer[])Bar_Chart_Controller.data.Clone(), FM_AVG_DST);
        Debug.Log("%%Distance MaxMin: " + DistanceMaxMin);
        SoundMaxMin = GetMaximumMinimum((TestsBoxer[])Bar_Chart_Controller.data.Clone(), AUDIO_TSH);
        Debug.Log("%%SoundMaxMin: " + SoundMaxMin);
    }
    private int NumberOfBars = 3;
    private int Selection = 0b111;

    public bool ToggleBar(bool selected, int index)
    {
        if (index < 3)
        {
            int Joiner = (int)Math.Pow(2, 2 - index);
            if (selected) {
                Selection |= Joiner;
                //Selection >> (3 - index - 1);
                NumberOfBars++; 
            }
            else
            {
                Joiner = (int)(Math.Pow(2, (2 - index)) - 1);
                Selection &= Joiner;
                NumberOfBars--;
            }
            Debug.Log("NumberOfBars: " + NumberOfBars);
            return NumberOfBars > 0;
        }
        return true;
    }
    public void DrawCharts()
    {
        bool FMBar = (Selection >> 2) == 1;
        bool AudioBar = ((Selection & 3) >> 1) == 1;
        bool VisualBar = (Selection & 1) == 1;
        Debug.Log("WHat are you ??????????????? " + data + " Size " + data.Length + " Compared: " + fineMotorJson.player.Length + " And " + DictData.Count);
        Vector2 minMax = TimeMaxMin;// getMaximumAndMinimum((TestsBoxer[])Bar_Chart_Controller.data.Clone(), FM_AVG_TRK_TME);
        int numberOfBar = NumberOfBars;
        float maxValue = minMax.x;
        float minValue = minMax.y;
        float startX = -WIDTH / 2f;
        float startY = 0;
        float accum = startX;
        float maxDisplayable = 52f;
        //  float barWidth = WIDTH / (4 * maxDisplayable - 1) * (maxDisplayable / DictData.Count);

        float barWidth = WIDTH / (maxDisplayable * (numberOfBar + 1) - 1) * (maxDisplayable / DictData.Count);

        gap = barWidth;
        float lineThickness = 2.5f * (maxDisplayable / DictData.Count);
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
        lineRendererFMAcc = fmAccLine.GetComponent<LineRenderer>();
        lineRendererAvgDist = fmAvgDistLine.GetComponent<LineRenderer>();
        lineRendererAudioThresh = audioThreshLine.GetComponent<LineRenderer>();
        lineRendererVisualShotAcc = visualShotAccLine.GetComponent<LineRenderer>();
        lineRendererVisualTargetAcc = visualTargetAccLine.GetComponent<LineRenderer>();
        foreach (KeyValuePair<DateTime, TestsBoxer> pair in DictData)
        {
            int BarIndex = 0;
            DateTime timestamp = pair.Key;

            if (pair.Value.fineMotor != null)
            {
                double accuracy = pair.Value.fineMotor.accuracy;
                double avgDistance = pair.Value.fineMotor.avgDistanceFromPlayer;
                Vector2 mnmx = DistanceMaxMin;// getMaximumAndMinimum((TestsBoxer[])Bar_Chart_Controller.data.Clone(), FM_AVG_DST);
                float mn = mnmx.y;
                float mx = mnmx.x;
                if (accuracy > Mathf.Epsilon)
                {
                    i++;
                    startY = 0; // Drawing will alway begin at point 0 (datum) for accuracy 0-100%
                    float fmAccLineHeight = (float)(accuracy / 100 * HEIGHT);
                    Vector3 pos = new Vector3(startY + accum, fmAccLineHeight, 0);
                    accPositions.Add(pos);
                    lineRendererFMAcc.positionCount = i + 1;
                }
                if (avgDistance > Mathf.Epsilon)
                {
                    startY = mn * -1; // Drawing will begin at the lowest value
                    float fmAvgDistLineHeight = startY + mn + HEIGHT * (float)(avgDistance - mn) / (mx - mn);
                    j++;
                    Vector3 avgD = new Vector3(accum, fmAvgDistLineHeight, 0);
                    avgDistPositions.Add(avgD);
                    lineRendererAvgDist.positionCount = j + 1;
                }
                if (FMBar)
                {
                    double avgTrackingTime = pair.Value.fineMotor.avgTrackingTime;
                    float barHeight = (float)(HEIGHT * (avgTrackingTime - minValue) / (maxValue - minValue));
                    Bar value = Instantiate(bar);
                    ClearScreenEvent += value.DestroyBar;
                    value.GetComponent<Renderer>().material.color = Color.red;
                    value.SetDimension(new Vector3(barWidth, barHeight, 0.25f));
                    value.gameObject.transform.position = new Vector3((accum + BarIndex * barWidth) + value.gameObject.transform.localScale.x / 2,
                    value.gameObject.transform.position.y + value.gameObject.transform.localScale.y / 2,
                                                                    value.gameObject.transform.position.z);
                    BarIndex++;
                }
            }
            else if(FMBar) { BarIndex++; }

            //               accum += barWidth;
            if (pair.Value.audio != null)
            {
                double minSoundThresh = pair.Value.audio.minSoundThreshold;
                minMax = SoundMaxMin;// getMaximumAndMinimum((TestsBoxer[])Bar_Chart_Controller.data.Clone(), AUDIO_TSH);
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
                if (AudioBar)
                {
                    double avgResponseTime = pair.Value.audio.avgResponseTime / 1000;
                    float barHeight = (float)(HEIGHT * (avgResponseTime - minValue) / (maxValue - minValue));
                    Bar value = Instantiate(bar);
                    ClearScreenEvent += value.DestroyBar;
                    value.GetComponent<Renderer>().material.color = Color.yellow;
                    value.SetDimension(new Vector3(barWidth, barHeight, 0.25f));
                    value.gameObject.transform.position = new Vector3((accum + BarIndex * barWidth) + value.gameObject.transform.localScale.x / 2,
                    value.gameObject.transform.position.y + value.gameObject.transform.localScale.y / 2,
                                                                    value.gameObject.transform.position.z);
                    BarIndex++;
                }
            }
            else if(AudioBar) { BarIndex++; }

            //              accum += barWidth;
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

                if (VisualBar && pair.Value.visual.responseTimes != null)
                {
                    double avgResponseTime = pair.Value.visual.getAverageResponseTime();
                    float barHeight = (float)(HEIGHT * (avgResponseTime - minValue) / (maxValue - minValue));
                    Bar value = Instantiate(bar);
                    ClearScreenEvent += value.DestroyBar;
                    value.GetComponent<Renderer>().material.color = Color.blue;
                    value.SetDimension(new Vector3(barWidth, barHeight, 0.25f));
                    value.gameObject.transform.position = new Vector3((accum + BarIndex * barWidth) + value.gameObject.transform.localScale.x / 2,
                    value.gameObject.transform.position.y + value.gameObject.transform.localScale.y / 2,
                                                                    value.gameObject.transform.position.z);
                    BarIndex++;
                }
                else if(VisualBar)
                {
                    BarIndex++;
                }

            }
            else { BarIndex++; }
            //                    accum += (barWidth + gap);
            accum += (BarIndex * barWidth + gap);
            //         accum += ((BarIndex + 1) * barWidth + gap);
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
    private DateTime StartDate, EndDate;
    public void RedrawChart(DateTime? StartDate, DateTime? EndDate)
    {
        if (!StartDate.HasValue)
        {
            StartDate = this.StartDate;
            EndDate = this.EndDate;
        }
        else
        {
            this.StartDate = StartDate.Value;
            this.EndDate = EndDate.Value;
        }
        LoadData();
        DictData = GetTestData(fineMotorJson, StartDate.Value, EndDate.Value);
        LoadMaxMin();
        OnScreenCleared();
        DrawCharts();
        OnLabelsChanged();
    }
    private Vector2 GetMaximumMinimum(TestsBoxer[] boxer, int Attribute)
    {
        Array.Sort(boxer, (a, b) =>
        {
            if (Attribute >> 2 == FINE_MOTOR)
            {
                FineMotor fineA = a.fineMotor;
                FineMotor fineB = b.fineMotor;
                if (Attribute == FM_AVG_TRK_TME)
                {
                    return (int)Math.Sign(fineB.avgTrackingTime - fineA.avgTrackingTime);
                }
                else if (Attribute == FM_AVG_DST)
                {
                    return (int)Math.Sign(fineB.avgDistanceFromPlayer - fineA.avgDistanceFromPlayer);
                }
                else if (Attribute == FM_ACC)
                {
                    return (int)Math.Sign(fineB.accuracy - fineA.accuracy);
                }
            }
            else if (Attribute >> 2 == AUDIO)
            {
                Audio audioA = a.audio;
                Audio audioB = b.audio;
                if (Attribute == AUDIO_TSH)
                {
                    return (int)Math.Sign(audioB.minSoundThreshold - audioA.minSoundThreshold);
                }
                else if (Attribute == AUDIO_RSP_TME)
                {
                    return (int)Math.Sign(audioB.avgResponseTime - audioA.avgResponseTime);
                }
            }
            else if (Attribute >> 2 == VISUAL)
            {
                Visual visualA = a.visual;
                Visual visualB = b.visual;
                if (Attribute == VISUAL_TGT_ACC)
                {
                    return (int)Math.Sign(visualB.targetAccuracy - visualA.targetAccuracy);
                }
                else if (Attribute == VISUAL_SHT_ACC)
                {
                    return (int)Math.Sign(visualB.shotAccuracy - visualA.shotAccuracy);
                }
                else if (Attribute == VISUAL_AVG_TRK_TME)
                {
                    return (int)Math.Sign(visualB.getAverageResponseTime() - visualA.getAverageResponseTime());
                }
            }
            return 0;
        }
        );
        switch (Attribute)
        {
            case FM_ACC:
                return new Vector2((float)boxer[0].fineMotor.accuracy, (float)boxer[boxer.Length - 1].fineMotor.accuracy);
            case FM_AVG_DST:
                return new Vector2((float)boxer[0].fineMotor.avgDistanceFromPlayer, (float)boxer[boxer.Length - 1].fineMotor.avgDistanceFromPlayer); ;
            case FM_AVG_TRK_TME:
                foreach (var box in boxer)
                {
                    Debug.Log("Spitting: " + box.fineMotor.avgTrackingTime);
                }
                return new Vector2((float)boxer[0].fineMotor.avgTrackingTime, (float)boxer[boxer.Length - 1].fineMotor.avgTrackingTime); ;
            case AUDIO_RSP_TME:
                return new Vector2((float)boxer[0].audio.avgResponseTime, (float)boxer[boxer.Length - 1].audio.avgResponseTime);
            case AUDIO_TSH:
                return new Vector2((float)boxer[0].audio.minSoundThreshold, (float)boxer[boxer.Length - 1].audio.minSoundThreshold);
            case VISUAL_AVG_TRK_TME:
                return new Vector2((float)boxer[0].visual.getAverageResponseTime(), (float)boxer[boxer.Length - 1].visual.getAverageResponseTime());
            case VISUAL_SHT_ACC:
                return new Vector2((float)boxer[0].visual.shotAccuracy, (float)boxer[boxer.Length - 1].visual.shotAccuracy);
            case VISUAL_TGT_ACC:
                return new Vector2((float)boxer[0].visual.targetAccuracy, (float)boxer[boxer.Length - 1].visual.targetAccuracy);
        }
        return Vector2.zero;
    }


    private Vector2 getMaximumAndMinimum(TestsBoxer[] playerBoxer, int attribute)
    {
        Array.Sort(playerBoxer, (a, b) =>
        {

            FineMotor fineA = a.fineMotor;
            FineMotor fineB = b.fineMotor;
            if (attribute == FM_AVG_TRK_TME)
            {
                return (int)(fineB.avgTrackingTime - fineA.avgTrackingTime);
            }
            else if (attribute == FM_AVG_DST)
            {
                return (int)(fineB.avgDistanceFromPlayer - fineA.avgDistanceFromPlayer);
            }
            Audio audioA = a.audio;
            Audio audioB = b.audio;
            if (attribute == AUDIO_TSH)
            {
                return (int)(audioB.minSoundThreshold - audioA.minSoundThreshold);
            }
            Visual visualA = a.visual;
            Visual visualB = b.visual;
            if (attribute == VISUAL_SHT_ACC)
            {
                return (int)(visualB.shotAccuracy - visualA.shotAccuracy);
            }
            else if (attribute == VISUAL_TGT_ACC)
            {
                return (int)(visualB.targetAccuracy - visualA.targetAccuracy);
            }
            else return 0;
        });
        if (attribute == FM_AVG_TRK_TME)
        {

            //          Bar_Chart_Controller.MAX_MIN_FM_AVG_TRK_TIME = new Vector2((float)playerBoxer[0].fineMotor.avgTrackingTime, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgTrackingTime);
            //          Debug.Log("FINE MOTOR MINMAX: " + MAX_MIN_FM_AVG_TRK_TIME);
            return new Vector2((float)playerBoxer[0].fineMotor.avgTrackingTime, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgTrackingTime);
        }
        else if (attribute == FM_AVG_DST)
        {
            //          Bar_Chart_Controller.MAX_MIN_FM_AVG_DST = new Vector2((float)playerBoxer[0].fineMotor.avgDistanceFromPlayer, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgDistanceFromPlayer);
            return new Vector2((float)playerBoxer[0].fineMotor.avgDistanceFromPlayer, (float)playerBoxer[playerBoxer.Length - 1].fineMotor.avgDistanceFromPlayer);
        }
        else if (attribute == AUDIO_TSH)
        {
            //          Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH = new Vector2((float)playerBoxer[0].audio.minSoundThreshold, (float)playerBoxer[playerBoxer.Length - 1].audio.minSoundThreshold);
            return new Vector2((float)playerBoxer[0].audio.minSoundThreshold, (float)playerBoxer[playerBoxer.Length - 1].audio.minSoundThreshold);
        }
        else if (attribute == VISUAL_SHT_ACC)
        {
            return new Vector2((float)playerBoxer[0].visual.shotAccuracy, (float)playerBoxer[playerBoxer.Length - 1].visual.shotAccuracy);
        }
        else if (attribute == VISUAL_TGT_ACC)
        {
            return new Vector2((float)playerBoxer[0].visual.targetAccuracy, (float)playerBoxer[playerBoxer.Length - 1].visual.targetAccuracy);
        }
        else return new Vector2();
    }
    private Dictionary<DateTime, TestsBoxer> GetTestData(TestsJSON playerJson)
    {
        TestsBoxer[] boxers = playerJson.player;
        Array.Sort(boxers, (a, b) =>
        {
            TimeSecs tw1 = a.timestamp.value;
            TimeSecs tw2 = b.timestamp.value;
            return DateTime.Compare(tw1.getTimeStamp(), tw2.getTimeStamp());
        });
        Dictionary<DateTime, TestsBoxer> dict = new Dictionary<DateTime, TestsBoxer>();
        for (int i = 0; i < boxers.Length; i++)
        {
            dict.Add(boxers[i].timestamp.value.getTimeStamp(), boxers[i]);
        }
        Bar_Chart_Controller.data = dict.Values.ToArray();

        return dict;
    }

    private Dictionary<DateTime, TestsBoxer> GetTestData(TestsJSON playerJson, DateTime StartDate, DateTime EndDate)
    {
        TestsBoxer[] boxers = playerJson.player;
        Array.Sort(boxers, (a, b) =>
        {
            TimeSecs tw1 = a.timestamp.value;
            TimeSecs tw2 = b.timestamp.value;
            return DateTime.Compare(tw1.getTimeStamp(), tw2.getTimeStamp());
        });
        Dictionary<DateTime, TestsBoxer> dict = new Dictionary<DateTime, TestsBoxer>();
        for (int i = 0; i < boxers.Length; i++)
        {
            if (boxers[i].timestamp.value.getTimeStamp() >= StartDate && boxers[i].timestamp.value.getTimeStamp() <= EndDate)
            {
                dict.Add(boxers[i].timestamp.value.getTimeStamp(), boxers[i]);
            }
        }
        Bar_Chart_Controller.data = dict.Values.ToArray();

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

}
