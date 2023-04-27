using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

[Serializable]
public abstract class Model
{
/*    // Instructs Newtonsoft to minify JSON objects if the app isn't running in the editor
    private static Formatting formatting = Application.isEditor ? Formatting.Indented : Formatting.None;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, formatting, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = FirestoreContractResolver.Instance,
        });
    }*/
}
