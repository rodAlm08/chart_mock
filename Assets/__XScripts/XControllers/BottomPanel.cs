using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelPrefab;
    [SerializeField] private RawImage xTickPrefab;
    void OnEnable()
    {
        RectTransform rect = GetComponent<RectTransform>();
 
        float WIDTH = rect.rect.width * 0.8f;
        float HEIGHT = rect.rect.height;
        float start = rect.rect.width / 10;
  //      int weeks = 52;

        float accum = start;
        float maxDisplayable = 52f;

        float barWidth = WIDTH / (4 * maxDisplayable - 1) * (maxDisplayable / Bar_Chart_Controller.data.Length);
        float gap = barWidth;
  
        



  //      float accum = WIDTH / weeks;

        
        float offset = accum / 2;

        for (int i = 0; i < Bar_Chart_Controller.data.Length; i++)
        {
            float x = (3 * barWidth) * (i + 1) + gap * i + start;
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            RectTransform rr = lb.GetComponent<RectTransform>();
            
            DateTime time = Bar_Chart_Controller.data[i].timestamp.value.getTimeStamp();


            int a = time.DayOfYear / 7;


            lb.text = a + "-" +Bar_Chart_Controller.data[i].timestamp.value.getTimeStamp().ToString("yyyy");

            Debug.Log("Sorted Values From BottomPanel: " + Bar_Chart_Controller.data[i].timestamp.value.getTimeStamp());

            //           lb.transform.position = new Vector3(start + (i * accum) - offset / 2f, HEIGHT - rr.rect.height * 1.5f, 0);
            lb.transform.position = new Vector3(x - (3.25f * barWidth), HEIGHT - rr.rect.height * 1.5f, 0);

            RawImage lr = Instantiate(xTickPrefab);
            lr.transform.SetParent(transform, false);
            lr.transform.position = new Vector3(x, HEIGHT - (lr.GetComponent<RectTransform>().rect.width / 2f) * lr.transform.localScale.x, 0);

        }
    }

}