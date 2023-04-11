using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeftPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelPrefab;
  //  [SerializeField] private RawImage yTickPrefab;
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Debug.Log("REDRIGSO       ----------------------------           " + rect.rect.width);
        float WIDTH = rect.rect.width;
        float HEIGHT = rect.rect.height;
        float start = rect.rect.height * 0.25f;
        int numberOfValues = 10;
        float accum = HEIGHT * 0.7f / (numberOfValues);
        float offset = 0;// accum / 2;
        float MAX_TIME = Bar_Chart_Controller.MAX_MIN_FM_AVG_TRK_TIME.x;
        float xPosition = 0.75f * WIDTH;
        for (int i = 0; i < numberOfValues + 1; i++)
        {
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            Debug.Log("Maximum Time : " + MAX_TIME);
            lb.text = ((i) * MAX_TIME / numberOfValues).ToString("0.00");
            RectTransform rr = lb.GetComponent<RectTransform>();
            lb.transform.position = new Vector3(xPosition, start + (i * accum),  0);
    //        RawImage lr = Instantiate(xTickPrefab);
   //         lr.transform.SetParent(transform, false);
  //         lr.transform.position = new Vector3(offset / 2 + start + offset + (i * accum), HEIGHT - (lr.GetComponent<RectTransform>().rect.width / 2f) * lr.transform.localScale.x, 0);

        }
    }
}
