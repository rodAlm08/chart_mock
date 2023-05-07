using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LeftPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelPrefab;
    TextMeshProUGUI[] AccLabels, TimeLabels;
    //  [SerializeField] private RawImage yTickPrefab;
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float WIDTH = rect.rect.width;
        float HEIGHT = rect.rect.height;
        float start = rect.rect.height * 0.25f;

        int numberOfValues = 10;
        float accum = HEIGHT * 0.7f / (numberOfValues);
        float offset = 0;// accum / 2;
        float MAX_TIME = Bar_Chart_Controller.instance.TimeMaxMin.x; // Bar_Chart_Controller.MAX_MIN_FM_AVG_TRK_TIME.x;
        float MAX_ACCURACY = 100;
        float accuracyXposition = 0.25f * WIDTH;
        float timexPosition = 0.75f * WIDTH;
        AccLabels = new TextMeshProUGUI[numberOfValues + 1];
        TimeLabels = new TextMeshProUGUI[numberOfValues + 1];

        for (int i = 0; i < numberOfValues + 1; i++)
        {
            float startingOffSet = i == 0 ? 5f:0f;
           
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            lb.text = ((i) * MAX_TIME / numberOfValues).ToString("0.00");
            RectTransform rr = lb.GetComponent<RectTransform>();
            lb.transform.position = new Vector3(timexPosition, startingOffSet + start + (i * accum),  0);
            TimeLabels[i] = lb;
        }
        for (int i = 0; i < numberOfValues + 1; i++)
        {
            float startingOffSet = i == 0 ? 5f : 0f;
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);   
            lb.text = ((i) * MAX_ACCURACY / numberOfValues).ToString();
            lb.transform.position = new Vector3(accuracyXposition, startingOffSet + start + (i * accum), 0);
            AccLabels[i] = lb;
        }
        Bar_Chart_Controller.instance.RefreshLabelEvent += InitLabels;
    }
    private void OnDisable()
    {
        Bar_Chart_Controller.instance.RefreshLabelEvent -= InitLabels;
    }

    public void InitLabels()
    {
        float MAX_TIME = Bar_Chart_Controller.instance.TimeMaxMin.x; //Bar_Chart_Controller.MAX_MIN_FM_AVG_TRK_TIME.x;
        float MAX_ACCURACY = 100;
        int numberOfValues = 10;
        for (int i = 0; i < TimeLabels.Length; i++)
        {
            TimeLabels[i].text = ((i) * MAX_TIME / numberOfValues).ToString("0.00");
        }
        for (int i = 0; i < AccLabels.Length; i++)
        {
            AccLabels[i].text = ((i) * MAX_ACCURACY / numberOfValues).ToString();
        }
    }
}
