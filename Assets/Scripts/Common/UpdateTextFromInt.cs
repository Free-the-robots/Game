using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextFromInt : MonoBehaviour
{
    public void addValueText(int value)
    {
        GetComponent<Text>().text = (System.Convert.ToInt32(GetComponent<Text>().text) + value).ToString();
    }
    public void updateText(int value)
    {
        GetComponent<Text>().text = value.ToString();
    }
}
