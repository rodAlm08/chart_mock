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
        public string disappearTime;
        [JsonProperty("individualShotInfo")]
        public List<IndividualShotInfo> individualShotInfo = new List<IndividualShotInfo>();
        [JsonProperty("numShots")]
        public string numShots;
        [JsonProperty("numTargets")]
        public string numTargets;
        [JsonProperty("accuracy")]
        public string accuracy;
        [JsonProperty("headshots")]
        public string headshots;
        [JsonProperty("bodyShots")]
        public string bodyShots;
        [JsonProperty("avgTrackingTime")]
        public string avgTrackingTime;
        [JsonProperty("avgDistanceFromPlayer")]
        public string avgDistanceFromPlayer;

 /*       public string? disappearTime
        {
            get => disappearTime;
            set => disappearTime = value;
        }

        public IReadOnlyList<IndividualShotInfo> individualShotInfo
        {
            get => individualShotInfo;
            set => indiv    xidualShotInfo = value as List<IndividualShotInfo>;
        }

        public string? numShots
        {
            get => numShots;
            set => numShots = value;
        }

        public string? numTargets
        {
            get => numTargets;
            set => numTargets = value;
        }

        public string? accuracy
        {
            get => accuracy;
            set => accuracy = value;
        }

        public string? headshots
        {
            get => headshots;
            set => headshots = value;
        }

        public string? bodyShots
        {
            get => bodyshots;
            set => bodyshots = value;
        }

        public string? avgTrackingTime
        {
            get => avgTrackingTime;
            set => avgTrackingTime = value;
        }

        public string? avgDistanceFromPlayer
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
        public string collider;
        [JsonProperty("distanceFromPlayer")]
        public string distanceFromPlayer;
        [JsonProperty("isHeadshot")]
        public string isHeadshot;
        [JsonProperty("trackingTime")]
        public string trackingTime;
        [JsonProperty("posX")]
        public string posX;
        [JsonProperty("posY")]
        public string posY;
        [JsonProperty("posZ")]
        public string posZ;
        /*
                public string collider
                {
                    get => collider;
                    set => collider = value;
                }

                public string? distanceFromPlayer
                {
                    get => distanceFromPlayer;
                    set => distanceFromPlayer = value;
                }

                public string? isHeadshot
                {
                    get => isHeadshot;
                    set => isHeadshot = value;
                }

                public string? trackingTime
                {
                    get => trackingTime;
                    set => trackingTime = value;
                }

                public string? posX => posX;
                public string? posY => posY;
                public string? posZ => posZ;

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
