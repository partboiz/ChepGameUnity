using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    public const string KEY_DATA = "Unitdata";

    public void SaveTest(test Test)
    {
        string s = JsonUtility.ToJson(Test);
        PlayerPrefs.SetString(KEY_DATA,s);
    }
    private test Loadtest()
    {
        string s = PlayerPrefs.GetString(KEY_DATA);
        if (string.IsNullOrEmpty(s))
        {
            return new test();
        }
        return JsonUtility.FromJson<test>(s);
    }
    
}
