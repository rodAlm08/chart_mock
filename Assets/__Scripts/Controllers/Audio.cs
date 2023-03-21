using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

//namespace Database.Models
//{
    /// <summary>
    /// Represents the information stored about each Audio test done by the user.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class Audio : Model
    {
        [JsonProperty("audioInfo")]
        public AudioInfo[] audioInfo;
        [JsonProperty("avgResponseTime")]
        public double avgResponseTime ;
        [JsonProperty("minSoundThreshold")]
        public double minSoundThreshold ;
        [JsonProperty("numTargetsSpawned1")]
        public double numTargetsSpawned1 ;
        [JsonProperty("numTargetsSpawned2")]
        public double numTargetsSpawned2 ;
        [JsonProperty("testOneDuration")]
        public double testOneDuration ;
        [JsonProperty("testTwoDuration")]
        public double testTwoDuration ;

        //public IReadOnlyList<AudioInfo> audioInfo
        //{
        //    get => _audioInfo.Data;
        //    set => _audioInfo = new MapArray<AudioInfo>(value as List<AudioInfo>);
        //}

        //public double? avgResponseTime
        //{
        //    get => _avgResponseTime.Data;
        //    set => _avgResponseTime.Data = value;
        //}

        //public double? minSoundThreshold
        //{
        //    get => _minSoundThreshold.Data;
        //    set => _minSoundThreshold.Data = value;
        //}

        //public double? numTargetsSpawned1
        //{
        //    get => _numTargetsSpawned1.Data;
        //    set => _numTargetsSpawned1.Data = value;
        //}

        //public double? numTargetsSpawned2
        //{
        //    get => _numTargetsSpawned2.Data;
        //    set => _numTargetsSpawned2.Data = value;
        //}

        //public double? testOneDuration
        //{
        //    get => _testOneDuration.Data;
        //    set => _testOneDuration.Data = value;
        //}

        //public double? testTwoDuration
        //{
        //    get => _testTwoDuration.Data;
        //    set => _testTwoDuration.Data = value;
        //}
    }

    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class AudioInfo : Model
    {
        [JsonProperty("distanceFromPlayer")]
        public double distanceFromPlayer ;
        [JsonProperty("responseTime1")]
        public double responseTime1 ;
        [JsonProperty("responseTime2")]
        public double responseTime2 ;
        [JsonProperty("soundVolume")]
        public double soundVolume ;

        //public double? distanceFromPlayer
        //{
        //    get => _distanceFromPlayer.Data;
        //    set => _distanceFromPlayer.Data = value;
        //}

        //public double? responseTime1
        //{
        //    get => _responseTime1.Data;
        //    set => _responseTime1.Data = value;
        //}

        //public double? responseTime2
        //{
        //    get => _responseTime2.Data;
        //    set => _responseTime2.Data = value;
        //}

        //public double? soundVolume
        //{
        //    get => _soundVolume.Data;
        //    set => _soundVolume.Data = value;
        //}
    }
//}
