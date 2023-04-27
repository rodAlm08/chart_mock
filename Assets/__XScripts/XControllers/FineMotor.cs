using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

//namespace Database.Models
//{
    /// <summary>
    /// Represents the information stored about each Fine Motor test done by the user.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class FineMotor : Model
    {
        [JsonProperty("disappearTime")]
        public double disappearTime;
    [JsonProperty("individualShotInfo")]
    public IndividualShotInfo[] individualShotInfo;
        [JsonProperty("numShots")]
        public double numShots;
        [JsonProperty("numTargets")]
        public double numTargets;
        [JsonProperty("accuracy")]
        public double accuracy;
        [JsonProperty("headshots")]
        public double headshots;
        [JsonProperty("bodyShots")]
        public double bodyShots;
        [JsonProperty("avgTrackingTime")]
        public double avgTrackingTime;
        [JsonProperty("avgDistanceFromPlayer")]
        public double avgDistanceFromPlayer;
/*
        public double? disappearTime
        {
            get => disappearTime;
            set => disappearTime = value;
        }

        public IReadOnlyList<IndividualShotInfo> individualShotInfo
        {
            get => individualShotInfo;
            set => indiv    xidualShotInfo = value as List<IndividualShotInfo>;
        }

        public double? numShots
        {
            get => numShots;
            set => numShots = value;
        }

        public double? numTargets
        {
            get => numTargets;
            set => numTargets = value;
        }

        public double? accuracy
        {
            get => accuracy;
            set => accuracy = value;
        }

        public double? headshots
        {
            get => headshots;
            set => headshots = value;
        }

        public double? bodyShots
        {
            get => bodyshots;
            set => bodyshots = value;
        }

        public double? avgTrackingTime
        {
            get => avgTrackingTime;
            set => avgTrackingTime = value;
        }

        public double? avgDistanceFromPlayer
        {
            get => avgDistanceFromPlayer;
            set => avgDistanceFromPlayer = value;
        }*/
    }

    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class IndividualShotInfo : Model
    {
        [JsonProperty("collider")]
        public double collider;
        [JsonProperty("distanceFromPlayer")]
        public double distanceFromPlayer;
        [JsonProperty("isHeadshot")]
        public double isHeadshot;
        [JsonProperty("trackingTime")]
        public double trackingTime;
        [JsonProperty("posX")]
        public double posX;
        [JsonProperty("posY")]
        public double posY;
        [JsonProperty("posZ")]
        public double posZ;
        /*
                public double collider
                {
                    get => collider;
                    set => collider = value;
                }

                public double? distanceFromPlayer
                {
                    get => distanceFromPlayer;
                    set => distanceFromPlayer = value;
                }

                public double? isHeadshot
                {
                    get => isHeadshot;
                    set => isHeadshot = value;
                }

                public double? trackingTime
                {
                    get => trackingTime;
                    set => trackingTime = value;
                }

                public double? posX => posX;
                public double? posY => posY;
                public double? posZ => posZ;

                public Vector3 position
                {
                    get => new Vector3((float)posX.Data.Value, (float)posY.Data.Value, (float)posZ.Data.Value);
                    set
                    {
                        posX.Data = value.x;
                        posY.Data = value.y;
                        posZ.Data = value.z;
                    }
                }*/
 //x   }
}
