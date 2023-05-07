using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelPrefab;
    [SerializeField] private RawImage xTickPrefab;
    private TextMeshProUGUI[] Labels;
    private Toggle[] Selectables;
    private RawImage[] Ticks;
    void OnEnable()
    {
        Selectables = GetComponentsInChildren<Toggle>();
        for(int i = 0; i < Selectables.Length; i++)
        {
            int j = i;
            Selectables[i].onValueChanged.AddListener((selected) => { ToggleSwitched(selected, j); });
        }
        
        RectTransform rect = GetComponent<RectTransform>();
 
        float WIDTH = rect.rect.width * 0.8f;
        float HEIGHT = rect.rect.height;
        float start = rect.rect.width / 10;
        float accum = start;
        float maxDisplayable = 52f;

        float barWidth = WIDTH / (4 * maxDisplayable - 1) * (maxDisplayable / Bar_Chart_Controller.data.Length);
        float gap = barWidth;      
        float offset = accum / 2;
        Labels = new TextMeshProUGUI[Bar_Chart_Controller.data.Length];
        Ticks = new RawImage[Bar_Chart_Controller.data.Length];

        for (int i = 0; i < Bar_Chart_Controller.data.Length; i++)
        {
            float x = (3 * barWidth) * (i + 1) + gap * i + start;
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            RectTransform rr = lb.GetComponent<RectTransform>();           
            DateTime time = Bar_Chart_Controller.data[i].timestamp.value.getTimeStamp();
            int a = time.DayOfYear / 7;
            lb.text = a + "-" +Bar_Chart_Controller.data[i].timestamp.value.getTimeStamp().ToString("yyyy");
            lb.transform.position = new Vector3(x - (3.25f * barWidth), HEIGHT - rr.rect.height * 1.5f, 0);
            RawImage lr = Instantiate(xTickPrefab);
            lr.transform.SetParent(transform, false);
            lr.transform.position = new Vector3(x, HEIGHT - (lr.GetComponent<RectTransform>().rect.width / 2f) * lr.transform.localScale.x, 0);
            Labels[i] = lb;
            Ticks[i] = lr;
        }
        Bar_Chart_Controller.instance.RefreshLabelEvent += InitLabels;
    }
    private void OnDisable()
    {
        Bar_Chart_Controller.instance.RefreshLabelEvent -= InitLabels;
    }

    public void InitLabels()
    {
        for(int i = 0; i < Labels.Length; i++)
        {
            Destroy(Labels[i].gameObject);
            Destroy(Ticks[i].gameObject);
        }
        RectTransform rect = GetComponent<RectTransform>();

        float WIDTH = rect.rect.width * 0.8f;
        float HEIGHT = rect.rect.height;
        float start = rect.rect.width / 10;
        float accum = start;
        float maxDisplayable = 52f;

        float barWidth = WIDTH / (4 * maxDisplayable - 1) * (maxDisplayable / Bar_Chart_Controller.data.Length);
        float gap = barWidth;
        float offset = accum / 2;
        Labels = new TextMeshProUGUI[Bar_Chart_Controller.data.Length];
        Ticks = new RawImage[Bar_Chart_Controller.data.Length]; 
        for (int i = 0; i < Labels.Length; i++)
        {
            float x = (3 * barWidth) * (i + 1) + gap * i + start;
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            RectTransform rr = lb.GetComponent<RectTransform>();
            DateTime time = Bar_Chart_Controller.data[i].timestamp.value.getTimeStamp();
            int a = time.DayOfYear / 7;
            lb.text = a + "-" + Bar_Chart_Controller.data[i].timestamp.value.getTimeStamp().ToString("yyyy");
            lb.transform.position = new Vector3(x - (3.25f * barWidth), HEIGHT - rr.rect.height * 1.5f, 0);

            RawImage lr = Instantiate(xTickPrefab);
            lr.transform.SetParent(transform, false);
            lr.transform.position = new Vector3(x, HEIGHT - (lr.GetComponent<RectTransform>().rect.width / 2f) * lr.transform.localScale.x, 0);
            Labels[i] = lb;
            Ticks[i] = lr;
        }
    }

    private void ToggleSwitched(bool selected, int index) {
        if(Bar_Chart_Controller.instance.ToggleBar(selected, index))
        {
            Bar_Chart_Controller.instance.RedrawChart(null, null);
        }
        else
        {
            Bar_Chart_Controller.instance.ToggleBar(!selected, index);
            Selectables[index].SetIsOnWithoutNotify(!selected);
        }
    }

}