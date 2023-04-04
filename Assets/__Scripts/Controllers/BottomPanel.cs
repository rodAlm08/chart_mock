using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI labelPrefab;
    [SerializeField] private LineRenderer xTickPrefab;
    void Start()
    {

        //    TextMeshProUGUI label = Instantiate(labelPrefab);
        //   label.transform.SetParent(transform, false);
        RectTransform rect = GetComponent<RectTransform>();
        Debug.Log("REDRIGSO       ----------------------------           " + rect.rect.width);
        float WIDTH = rect.rect.width * 0.8f;
        float HEIGHT = rect.rect.height;
        float start = WIDTH / 10;
        int weeks = 52;
        float accum = WIDTH / weeks;
        float offset = accum / 2f;
        //    label.transform.position = new Vector3(start - offset, 0, 0);

        for (int i = 0; i < weeks; i++)
        {
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            RectTransform rr = lb.GetComponent<RectTransform>();
            lb.transform.position = new Vector3(start + offset + (i * accum) + rr.rect.width / 4, HEIGHT - rr.rect.height * 1.5f, 0);

            LineRenderer lr = Instantiate(xTickPrefab);
            lr.transform.SetParent(transform, false);
            lr.transform.position = transform.position;
            lr.positionCount = 2;
            Vector3 pos1 = new Vector3(rect.rect.x + rect.rect.width / 10 + offset + (i * accum), HEIGHT, 0);
            Vector3 pos2 = new Vector3(rect.rect.x + rect.rect.width / 10 + offset + (i * accum), HEIGHT + 10, 0);
            lr.SetPositions(new Vector3[] { pos1, pos2 });
        }
    }

}
