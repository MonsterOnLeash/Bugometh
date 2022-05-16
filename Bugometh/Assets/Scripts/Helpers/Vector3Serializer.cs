using UnityEngine;
using System.Collections;
using System.Globalization;

static class Vector3Serializer
{

    public static string Serialize(Vector3 vector_obj)
    {
        return vector_obj.ToString();
    }

    public static Vector3 Deserialize(string vector_str)
    {

        if (vector_str.StartsWith("(") && vector_str.EndsWith(")"))
        {
            vector_str = vector_str.Substring(1, vector_str.Length - 2);
        } else
        {
            return Vector3.zero;
        }
        string[] sArray = vector_str.Split(',');
        if (sArray.Length != 3)
        {
            return Vector3.zero;
        }
        var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.NumberDecimalSeparator = ".";

        Vector3 result = new Vector3(
             float.Parse(sArray[0], ci),
             float.Parse(sArray[1], ci),
             float.Parse(sArray[2], ci));
        return result;
    }
}
