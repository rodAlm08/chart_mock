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

        float MIN_SOUND = Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH.y;
        float MAX_SOUND = Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH.x;

        float avgDistanceXposition = transform.position.x - WIDTH / 2f + 0.25f * WIDTH;//9.25f * WIDTH;
        float audioxPosition = transform.position.x - WIDTH / 2f + 0.75f * WIDTH;//9.75f * WIDTH;


        for (int i = 0; i < numberOfValues + 1; i++)
        {
            float startingOffSet = i == 0 ? 5f : 0f;
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            Debug.Log("Maximum AVG Distance : " + MAX_AVG_DISTANCE + " Value for i " + i + " = " );
            lb.text = ((i) * MAX_AVG_DISTANCE / numberOfValues).ToString("0.00");
            lb.transform.position = new Vector3(avgDistanceXposition, startingOffSet + start + (i * accum),  0);

        }

        for (int i = 0; i < numberOfValues + 1; i++)
        {
            float startingOffSet = i == 0 ? 5f : 0f;
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            Debug.Log("audioooooooooo " + ((i) * MAX_SOUND / numberOfValues).ToString());

            lb.text = (MIN_SOUND + (i) * (MAX_SOUND - MIN_SOUND) / numberOfValues).ToString("0.00");
            lb.transform.position = new Vector3(audioxPosition, startingOffSet + start + (i * accum), 0);


        }
    }
}
