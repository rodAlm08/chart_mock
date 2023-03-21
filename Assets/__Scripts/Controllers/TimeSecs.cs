using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class TimeSecs
{
    public long _seconds;
    public long _nanoseconds;

    public DateTime getTimeStamp()
    {
        DateTimeOffset dateTimeOffSet = DateTimeOffset.FromUnixTimeMilliseconds((_seconds + (_nanoseconds / 1000000000)) * 1000);
        DateTime dateTime = dateTimeOffSet.DateTime;
        return dateTime;
    }
}
