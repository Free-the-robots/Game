using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogData
{
    public string PathImageDisplayed;
    public string NameEntitySpeaking;
    public string TextDisplayed;
    public string ColorNameEntity;
    public string ColorText;
}

[Serializable]
public class DialogDataList
{
    public List<DialogData> ListDialogData;
}
