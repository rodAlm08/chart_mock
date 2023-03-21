using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

//namespace Database.Models
//{
    /// <summary>
    /// Represents the information stored about each Visual test done by the user.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class Visual : Model
    {
        [JsonProperty("numShotSuccess")]
        public int numShotSuccess;
        [JsonProperty("numShots")]
        public int numShots;
        [JsonProperty("numTargets")]
        public int numTargets;
        [JsonProperty("responseTimes")]
        public double[] responseTimes;
        [JsonProperty("shotAccuracy")]
        public double shotAccuracy;
        [JsonProperty("targetAccuracy")]
        public double targetAccuracy;
        [JsonProperty("testTime")]
        public double testTime;

        public double getAverageResponseTime()
    {
        double sum = 0;
        for(int i = 0; i < responseTimes.Length; i++)
        {
            sum += responseTimes[i];
        }
        return sum / responseTimes.Length;
    }
        /*
                public int? numShotSuccess
                {
                    get => _numShotSuccess.Data;
                    set => _numShotSuccess.Data = value;
                }

                public int? numShots
                {
                    get => _numShots.Data;
                    set => _numShots.Data = value;
                }

                public int? numTargets
                {
                    get => _numTargets.Data;
                    set => _numTargets.Data = value;
                }

                public IReadOnlyList<double> responseTimes
                {
                    get => _responseTimes.Data;
                    set => _responseTimes = new Array<double>(value as List<double>);
                }

                public double? shotAccuracy
                {
                    get => _shotAccuracy.Data;
                    set => _shotAccuracy.Data = value;
                }

                public double? targetAccuracy
                {
                    get => _targetAccuracy.Data;
                    set => _targetAccuracy.Data = value;
                }

                public double? testTime
                {
                    get => _testTime.Data;
                    set => _testTime.Data = value;
                }
            }
        */
    
}
