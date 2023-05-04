using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TopPanel : MonoBehaviour
{
    public enum Selection
    {
        DAY, MONTH, YEAR
    }
    private GameObject DateSelection1, DateSelection2;
    private TMP_Dropdown OrderByDropDown, FromDay, FromMonth, FromYear, ToDay, ToMonth, ToYear;
    private TextMeshProUGUI FromText, ToText, OrderByText;
    private DateTime StartDate, EndDate;
    // Start is called before the first frame update
    void Start()
    {
        StartDate = Bar_Chart_Controller.data[0].timestamp.value.getTimeStamp();
        EndDate = Bar_Chart_Controller.data[Bar_Chart_Controller.data.Length - 1].timestamp.value.getTimeStamp();
        Debug.Log("StartDate: " + StartDate.ToString() + " - EndDate: " + EndDate.ToString());
        RectTransform[] objs = GetComponentsInChildren<RectTransform>();
        foreach(var obj in objs){
            if(obj.name == "DateSelection (1)")
            {
                DateSelection1 = obj.gameObject;
                TMP_Dropdown[] comps = DateSelection1.GetComponentsInChildren<TMP_Dropdown>();
                foreach (var comp in comps)
                {
                    if(comp.name == "DayDropDown")
                    {
                        FromDay = comp;
                        FromDay.ClearOptions();
                        FromDay.AddOptions(GetDays(StartDate.Month, StartDate.Year));
                        FromDay.SetValueWithoutNotify(StartDate.Day - 1);
                        FromDay.onValueChanged.AddListener((index) =>  FromDateSelectionChanged(Selection.DAY, index));
                    } else if(comp.name == "MonthDropDown")
                    {
                        FromMonth = comp;
                        FromMonth.ClearOptions();
                        FromMonth.AddOptions(GetMonths());
                        FromMonth.SetValueWithoutNotify(StartDate.Month - 1);
                        FromMonth.onValueChanged.AddListener((index) => FromDateSelectionChanged(Selection.MONTH, index));
                    } else if(comp.name == "YearDropDown")
                    {
                        FromYear = comp;
                        FromYear.ClearOptions();
                        FromYear.AddOptions(GetYear(StartDate.Year, EndDate.Year));
                        FromYear.onValueChanged.AddListener((index) => FromDateSelectionChanged(Selection.YEAR, index));
                    }
                }
            } else if (obj.name == "DateSelection (2)")
            {
                DateSelection2 = obj.gameObject;
                TMP_Dropdown[] comps = DateSelection2.GetComponentsInChildren<TMP_Dropdown>();
                foreach (var comp in comps)
                {
                    if (comp.name == "DayDropDown")
                    {
                        ToDay = comp;
                        ToDay.ClearOptions();
                        ToDay.AddOptions(GetDays(EndDate.Month, EndDate.Year));
                        ToDay.SetValueWithoutNotify(EndDate.Day - 1);
                        ToDay.onValueChanged.AddListener((index) => ToDateSelectionChanged(Selection.DAY, index));
                    }
                    else if (comp.name == "MonthDropDown")
                    {
                        ToMonth = comp;
                        ToMonth.ClearOptions();
                        ToMonth.AddOptions(GetMonths());
                        ToMonth.SetValueWithoutNotify(EndDate.Month - 1);
                        ToMonth.onValueChanged.AddListener((index) => ToDateSelectionChanged(Selection.MONTH, index));
                    }
                    else if (comp.name == "YearDropDown")
                    {
                        ToYear = comp;
                        ToYear.ClearOptions();
                        ToYear.AddOptions(GetYear(StartDate.Year, EndDate.Year));
                        ToYear.onValueChanged.AddListener((index) => ToDateSelectionChanged(Selection.YEAR, index));
                    }
                }
            } else if(obj.name == "OrderByDropDown")
            {
                OrderByDropDown = obj.GetComponent<TMP_Dropdown>();
        
                OrderByDropDown.ClearOptions();
                OrderByDropDown.AddOptions(new List<string> { "Test", "Day", "Week", "Month", "Year" });
                OrderByDropDown.onValueChanged.AddListener((a) => OrderByValueChanged(a));
            }
        }
    }

    private void UpdateDatePicker(TMP_Dropdown dropdown, DateTime date, Selection selection)
    {
        dropdown.ClearOptions();
        
        if (selection == Selection.DAY)
        {
            dropdown.AddOptions(GetDays(date.Month, date.Year));
            dropdown.SetValueWithoutNotify(date.Day - 1);
        }
        else if (selection == Selection.MONTH)
        {
            dropdown.AddOptions(GetMonths());
            dropdown.SetValueWithoutNotify(date.Month - 1);
        } else if(selection == Selection.YEAR)
        {
            dropdown.SetValueWithoutNotify(StartDate.Year - date.Year);  
        }
    }
    private void FromDateSelectionChanged(Selection select, int index)
    {
        if(select == Selection.DAY)
        {
            int day = index + 1;
            StartDate = new DateTime(StartDate.Year, StartDate.Month, day);
            UpdateDatePicker(FromDay, StartDate, select);
        } else if(select == Selection.MONTH)
        {
            int month = index + 1;
            int day = StartDate.Day;
            while(day > GetNumberOfDays(month, StartDate.Year))
            {
                day--;
            }
            StartDate = new DateTime(StartDate.Year, month, day);
            UpdateDatePicker(FromDay, StartDate, Selection.DAY);
   //         UpdateDatePicker(FromMonth, StartDate, Selection.MONTH);
        } else if (select == Selection.YEAR)
        {
            int year = StartDate.Year + index;
            StartDate = new DateTime(year, StartDate.Month, StartDate.Year);
            UpdateDatePicker(FromYear, StartDate, select);
        }
        Bar_Chart_Controller.instance.RedrawChat(StartDate, EndDate);
    }
    private void ToDateSelectionChanged(Selection select, int index)
    {
        if (select == Selection.DAY)
        {
            int day = index + 1;
            EndDate = new DateTime(EndDate.Year, EndDate.Month, day);
            UpdateDatePicker(ToDay, EndDate, select);
        }
        else if (select == Selection.MONTH)
        {
            int month = index + 1;
            int day = EndDate.Day;
            while(day > GetNumberOfDays(month, EndDate.Year))
            {
                day--;
            }
            EndDate = new DateTime(EndDate.Year, month, day);
            UpdateDatePicker(ToDay, EndDate, Selection.DAY);
    //        UpdateDatePicker(ToMonth, EndDate, Selection.MONTH);
        }
        else if (select == Selection.YEAR)
        {
            int year = EndDate.Year + index;
            EndDate = new DateTime(year, EndDate.Month, EndDate.Year);
            UpdateDatePicker(ToYear, EndDate, Selection.YEAR);
        }
        Bar_Chart_Controller.instance.RedrawChat(StartDate, EndDate);
    }

    private void OrderByValueChanged(int index)
    {
  //      Debug.Log("Top Panel Order by value changed to " + index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetNumberOfDays(int month, int year)
    {
        return (month == 9 || month == 4 || month == 6 || month == 11) ? 30 : (month != 2 ? 31 : (DateTime.IsLeapYear(year) ? 29 : 28));
    }
    public List<string> GetDays(int month, int year)
    {
        int count = (month == 9 || month == 4 || month == 6 || month == 11) ? 30 : (month != 2 ? 31 : (DateTime.IsLeapYear(year) ? 29 : 28));
        string[] days = new string[count];
        List<string> list = new List<string>();
        for (int i = 0; i < count; i++)
        {
            days[i] = (i + 1).ToString();
            list.Add(days[i]);
    //        Debug.Log("Days: " + days[i]);
        }
        return list;
    }

    public List<string> GetMonths()
    {
        List<string> list = new List<string>();
        for(int i = 0; i < 12; i++)
        {
            list.Add((i + 1).ToString());
  //          Debug.Log("Days: " + (i + 1).ToString());
        }
        return list;
    }

    public List<string> GetYear(int startYear, int endYear) 
    {
        List<string> list = new List<string>();
        for(int i = 0; i <= (endYear - startYear); i++)
        {
            list.Add((startYear + i).ToString());
        }
        return list;
    }
}
