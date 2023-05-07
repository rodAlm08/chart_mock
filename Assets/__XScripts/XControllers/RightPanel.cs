using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RightPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelPrefab;
    private TextMeshProUGUI[] DistanceLabels, SoundLabels;
    //  [SerializeField] private RawImage yTickPrefab;
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        //       Debug.Log("Right Panel Width      ----------------------------           " + rect.rect.width);
        float WIDTH = rect.rect.width;
        float HEIGHT = rect.rect.height;
        float start = rect.rect.height * 0.25f;

        int numberOfValues = 10;
        float accum = HEIGHT * 0.7f / (numberOfValues);
        float offset = 0;// accum / 2;
        float MAX_AVG_DISTANCE = Bar_Chart_Controller.instance.DistanceMaxMin.x; // Bar_Chart_Controller.MAX_MIN_FM_AVG_DST.x;

        float MIN_SOUND = Bar_Chart_Controller.instance.SoundMaxMin.y;// Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH.y;
        float MAX_SOUND = Bar_Chart_Controller.instance.SoundMaxMin.x;// Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH.x;

        float avgDistanceXposition = transform.position.x - WIDTH / 2f + 0.25f * WIDTH;//9.25f * WIDTH;
        float audioxPosition = transform.position.x - WIDTH / 2f + 0.75f * WIDTH;//9.75f * WIDTH;

        DistanceLabels = new TextMeshProUGUI[numberOfValues + 1];
        SoundLabels = new TextMeshProUGUI[numberOfValues + 1];

        for (int i = 0; i < numberOfValues + 1; i++)
        {
            float startingOffSet = i == 0 ? 5f : 0f;
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            lb.text = ((i) * MAX_AVG_DISTANCE / numberOfValues).ToString("0.00");
            lb.transform.position = new Vector3(avgDistanceXposition, startingOffSet + start + (i * accum), 0);
            DistanceLabels[i] = lb;
        }

        for (int i = 0; i < numberOfValues + 1; i++)
        {
            float startingOffSet = i == 0 ? 5f : 0f;
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            lb.text = (MIN_SOUND + (i) * (MAX_SOUND - MIN_SOUND) / numberOfValues).ToString("0.00");
            lb.transform.position = new Vector3(audioxPosition, startingOffSet + start + (i * accum), 0);
            SoundLabels[i] = lb;

        }
        Bar_Chart_Controller.instance.RefreshLabelEvent += InitLabels;
    }
    private void OnDisable()
    {
        Bar_Chart_Controller.instance.RefreshLabelEvent -= InitLabels;
    }

    public void InitLabels()
    {
        int numberOfValues = 10;
        float MAX_AVG_DISTANCE = Bar_Chart_Controller.instance.DistanceMaxMin.x;// Bar_Chart_Controller.MAX_MIN_FM_AVG_DST.x;
        float MIN_SOUND = Bar_Chart_Controller.instance.SoundMaxMin.y;// Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH.y;
        float MAX_SOUND = Bar_Chart_Controller.instance.SoundMaxMin.x;// Bar_Chart_Controller.MAX_MIN_AUDIO_THRESH.x;
        for (int i = 0; i < DistanceLabels.Length; i++)
        {
            DistanceLabels[i].text = ((i) * MAX_AVG_DISTANCE / numberOfValues).ToString("0.00");
        }
        for (int i = 0; i < SoundLabels.Length; i++)
        {
            SoundLabels[i].text = (MIN_SOUND + (i) * (MAX_SOUND - MIN_SOUND) / numberOfValues).ToString("0.00");
        }
    }
}
