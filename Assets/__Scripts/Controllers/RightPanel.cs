using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RightPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelPrefab;
  //  [SerializeField] private RawImage yTickPrefab;
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Debug.Log("Right Panel Width      ----------------------------           " + rect.rect.width);
        float WIDTH = rect.rect.width;
        float HEIGHT = rect.rect.height;
        float start = rect.rect.height * 0.25f;

        int numberOfValues = 10;
        float accum = HEIGHT * 0.7f / (numberOfValues);
        float offset = 0;// accum / 2;
        float MAX_AVG_DISTANCE = Bar_Chart_Controller.MAX_MIN_FM_AVG_DST.x;
        float MAX_SOUND = Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH.x;

        float avgDistanceXposition = 9.25f * WIDTH;
        float audioxPosition = 9.75f * WIDTH;


        for (int i = 0; i < numberOfValues + 1; i++)
        {
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            Debug.Log("Maximum AVG Distance : " + MAX_AVG_DISTANCE + WIDTH);
            lb.text = ((i) * MAX_AVG_DISTANCE / numberOfValues).ToString("0.00");
            lb.transform.position = new Vector3(avgDistanceXposition, start + (i * accum),  0);

        }

        for (int i = 0; i < numberOfValues + 1; i++)
        {
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            Debug.Log("audioooooooooo " + ((i) * MAX_SOUND / numberOfValues).ToString());

            lb.text = ((i) * MAX_SOUND / numberOfValues).ToString();
            lb.transform.position = new Vector3(audioxPosition, start + (i * accum), 0);


        }
    }
}