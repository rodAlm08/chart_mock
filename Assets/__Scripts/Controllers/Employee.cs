using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
[System.Serializable]
public class Employee
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    [JsonProperty("lastName")]
    public string firstName;
    public string lastName;
    public FineMotor fineMotor;
}