using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static class MyRandom
{
    public static float p2Random
    {
        get { return 0.8f + 0.4f * Random.value; }
    }
    public static float p3Random
    {
        get { return 0.7f + 0.6f * Random.value; }
    }
    public static float p4Random
    {
        get { return 0.6f + 0.8f * Random.value; }
    }
    public static float p5Random
    {
        get { return 0.5f + 1f * Random.value; }
    }
}
[System.Serializable]
public class StatRange
{
    public int min;
    public int max;
    public float random { get { return min + (max - min) * Random.value; } }
}
[System.Serializable]
public class StatRangeFloat
{
    public StatRangeFloat(float _min, float _max)
    {
        min = _min;
        max = _max;
    }
    public float min;
    public float max;
    public float random { get { return min + (max - min) * Random.value; } }
    public float Evaluate(float ratio)
    {
        return min + (max - min) * UnityEngine.Mathf.Clamp01(ratio);
    }
}
public static class TransformUtility
{
    public static void SetLayer(this Transform trans, int layer)
    {
        trans.gameObject.layer = layer;
        foreach (Transform child in trans)
            child.SetLayer(layer);
    }
    public static Transform FindTransform(Transform parent, string name)
    {
        if (parent == null)
        {
            Debug.LogError("FindTransform: " + name + " is null");
            return null;
        }
        if (parent.name.Equals(name)) return parent;
        foreach (Transform child in parent)
        {
            Transform result = FindTransform(child, name);
            if (result != null) return result;
        }
        return null;
    }
    public static List<Transform> FindTransformList(Transform parent, string name)
    {
        List<Transform> transformList;
        if (parent == null)
        {
            Debug.LogError("FindTransform: " + name + " is null");
            return null;
        }
        transformList = new List<Transform>(parent.GetComponentsInChildren<Transform>());
        if (transformList.Count > 0)
        {
            return transformList.FindAll(t => t.name.Equals(name));
        }
        return null;
    }
    public static void GetAllChild(List<Transform> toList, Transform parent, int childLevelMax)
    {
        if (childLevelMax == 0)
            return;
        toList.Add(parent);
        foreach (Transform child in parent)
        {
            GetAllChild(toList, child, childLevelMax - 1);
        }
    }
    public static string ResolveTextSize(string input, int lineLength)
    {

        // Split string by char " "         
        string[] words = input.Split(" "[0]);

        // Prepare result
        string result = "";

        // Temp line string
        string line = "";

        // for each all words        
        foreach (string s in words)
        {
            // Append current word into line
            string temp = line + " " + s;

            // If line length is bigger than lineLength
            if (temp.Length > lineLength)
            {

                // Append current line into result
                result += line + "\n";
                // Remain word append into new line
                line = s;
            }
            // Append current word into current line
            else
            {
                line = temp;
            }
        }

        // Append last line into result        
        result += line;

        // Remove first " " char
        return result.Substring(1, result.Length - 1);
    }
	public static void Shuffle<T>(T[] texts) where T:class
	{
		// Knuth shuffle algorithm :: courtesy of Wikipedia :)
		for (int t = 0; t < texts.Length; t++)
		{
			T tmp = texts[t];
			int r = Random.Range(t, texts.Length);
			texts[t] = texts[r];
			texts[r] = tmp;
		}
	}
    public static string getGeneratedId()
    {
        string PEID;
        string charString;
        int i;
        string[] charArray;
        PEID = "";
        charString = "a b c d e f g h i j k l m n o p q r s t u v w x y z A B C D E F G H I J K L M N O P Q R S T U V W X Y Z 1 2 3 4 5 6 7 8 9 0";
        charArray = charString.Split(' ');

        for (i = 0; i < 4; i++)
        {
            PEID = PEID + charArray[UnityEngine.Random.Range(0, charArray.Length)];
        }
        PEID.Replace(" ", "");

        return PEID;
    }
    public static string colorToHex(Color32 color)
    {
        string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
        return hex;
    }

    public static Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }
    public static string customNumberToString(long value)
    {
        string comma = string.Format("{0:n0}", value);
        string front = comma.Substring(0, Mathf.Min(comma.Length, 4));
        if (front.EndsWith(","))
            front = front.Substring(0, front.Length - 1);
        int commaCount = comma.Split(',').Length - 1;
        front = front.Replace(",", ".");
        if (commaCount < 5)
        {
            switch (commaCount)
            {
                case 0:
                    return front;
                case 1:
                    return front + "K";
                case 2:
                    return front + "M";
                case 3:
                    return front + "B";
                case 4:
                    return front + "T";
                default:
                    break;
            }
        }
        else
        {
            string[] charArray = "a b c d e f g h i j k l m n o p q r s t u v w x y z".Split(' ');
            if(commaCount >= charArray.Length + 5)
                return string.Format("{0:E2}", value); 
            string charPick = charArray[(commaCount - 5)%charArray.Length];
            return front + charPick + charPick;
        }
        return front;
        /*
        for (int i = 0; i < 20; i++)
        {
            ulong value = 123;
            print(TransformUtility.customNumberToString((long)(value * System.Math.Pow(10, i))));
        }*/
    }
    public static Vector3 GetRandomLocWithinRadius( Vector3 Origin, float DistanceMax, float DistanceMin)//, Vector3? forward = null
    {
        Vector3 V = Vector3.zero;
        Vector3 Vmin = Vector3.zero;
        float Angle = 0;
	    float newDistanceMin = 0;

	    newDistanceMin += (DistanceMax-DistanceMin)/2;
        Angle = Random.value * Mathf.PI * 2.0f;

        V.x = Mathf.Sin(Angle) * (DistanceMax - newDistanceMin) * Random.value;
        V.z = Mathf.Cos(Angle) * (DistanceMax - newDistanceMin) * Random.value;
        Vmin.x = Mathf.Sin(Angle) * (newDistanceMin);
        Vmin.z = Mathf.Cos(Angle) * (newDistanceMin);
        return Origin + (Vmin + V);
    }
    public static Vector3 Get3DCirclePoint(float angle, float radius, Vector3 center, Vector3 normal, Vector3? refPoint = null)
    {
        float xv = Mathf.Cos(angle);
        float yv = Mathf.Sin(angle);

        if(refPoint == null)
            refPoint = findV(normal).Value;
        Vector3 w = Vector3.Cross(refPoint.Value, normal);

        // Return center + r * (V * cos(a) + W * sin(a))
        Vector3 r1 = radius * (refPoint.Value * xv);
        Vector3 r2 = radius * (w * yv);

        return new Vector3(center.x + r1.x + r2.x,
                         center.y + r1.y + r2.y,
                         center.z + r1.z + r2.z);
    }
    static private Vector3? findV(Vector3 normal)
    {
        Vector3 vp = new Vector3(0f, 0f, 0f);
        if (normal.x != 0 || normal.y != 0)
        {
            vp = new Vector3(0f, 0f, 1f);
        }
        else if (normal.x != 0 || normal.z != 0)
        {
            vp = new Vector3(0f, 1f, 0f);
        }
        else if (normal.y != 0 || normal.z != 0)
        {
            vp = new Vector3(1f, 0f, 0f);
        }
        else
        {
            return null; // will cause an exception later.
        }

        Vector3 cp = Vector3.Cross(normal, vp);
        return cp.normalized;
    }
    public static T ParseEnum<T>(string value)
    {
        return (T)System.Enum.Parse(typeof(T), value, true);
    }
    public static void CopyTransform(Transform from, Transform to)
    {
        to.localPosition = from.localPosition;
        to.localRotation = from.localRotation;
        to.localScale = from.localScale;
    }
    public static T DeepClone<T>(this T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
    public static void AlignAndParentTo(Transform obj, Transform to)
    {
        obj.parent = to.transform;
        obj.localPosition = Vector3.zero;
        obj.localRotation = new Quaternion();
    }

    public static Quaternion QuaternionFromVector(Vector3 forward, Vector3 right, Vector3 up)
    {
        Matrix4x4 m = new Matrix4x4();
        m.SetColumn(0, right);
        m.SetColumn(1, up);
        m.SetColumn(2, forward);

        // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
        q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
        q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
        q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
        return q;
    }
    public static void Resize<T>(this List<T> list, int newCount)
    {
        if (list == null)
            list = new List<T>();
        if (newCount <= 0)
        {
            list.Clear();
        }
        else
        {
            while (list.Count > newCount) list.RemoveAt(list.Count - 1);
            while (list.Count < newCount) list.Add(default(T));
        }
    }
    public static string AddSpacesToSentence(string text)
    {
        if (string.IsNullOrEmpty(text))
            return "";
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }
}