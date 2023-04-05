using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI labelPrefab;
    [SerializeField] private RawImage xTickPrefab;
    void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Debug.Log("REDRIGSO       ----------------------------           " + rect.rect.width);
        float WIDTH = rect.rect.width * 0.8f;
        float HEIGHT = rect.rect.height;
        float start = rect.rect.width / 10;
        int weeks = 52;
        float accum = WIDTH / weeks;
        float offset = accum / 2;

        for (int i = 0; i < weeks; i++)
        {
            TextMeshProUGUI lb = Instantiate(labelPrefab);
            lb.transform.SetParent(transform, false);
            RectTransform rr = lb.GetComponent<RectTransform>();
            lb.transform.position = new Vector3(start + (i * accum) - offset / 2f, HEIGHT - rr.rect.height * 1.5f, 0);
            RawImage lr = Instantiate(xTickPrefab);
            lr.transform.SetParent(transform, false);            
            lr.transform.position = new Vector3(offset / 2 + start + offset + (i * accum), HEIGHT - (lr.GetComponent<RectTransform>().rect.width / 2f) * lr.transform.localScale.x, 0);

        }
    }

}