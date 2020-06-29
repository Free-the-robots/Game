using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDataUpdate : MonoBehaviour
{
    private int lifeMax;
    private int armorMax;

    private float lifeW;
    private float lifeH;
    private float armorW;
    private float armorH;

    public RectTransform lifeParent;
    public RectTransform armorParent;

    public Color baseColor = new Color(0.3009968f, 0.7169812f, 0.7088346f);
    public Color midColor = new Color(0.8f, 0.4f, 0f);
    public Color criticalColor = new Color(1f, 0f, 0f);

    private void OnEnable()
    {
        UserData.UserData userData = UserData.UserDataManager.Instance.userData;
        lifeMax = (int)(AssetDataManager.Instance.spaceshipScriptableData[userData.shipEquiped].lifeMax);
        armorMax = (int)(AssetDataManager.Instance.spaceshipScriptableData[userData.shipEquiped].armor);

        lifeW = lifeParent.rect.width;
        lifeH = lifeParent.rect.height;
        armorW = armorParent.rect.width;
        armorH = armorParent.rect.height;

        if(lifeMax > 0)
            lifeParent.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(lifeW, lifeParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        lifeParent.GetChild(1).GetComponent<Text>().text = lifeMax.ToString();
        if(armorMax > 0)
            armorParent.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(armorW, armorParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        armorParent.GetChild(1).GetComponent<Text>().text = armorMax.ToString();
    }

    public void updateLife(int value)
    {
        lifeParent.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(value/(float)lifeMax*lifeW, lifeParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        lifeParent.GetChild(1).GetComponent<Text>().text = value.ToString();

        if(value / (float)lifeMax < 0.25f)
            lifeParent.GetChild(0).GetComponent<Image>().color = criticalColor;
        else if (value / (float)lifeMax < 0.5f)
            lifeParent.GetChild(0).GetComponent<Image>().color = midColor;
        else
            lifeParent.GetChild(0).GetComponent<Image>().color = baseColor;
    }

    public void updateArmor(int value)
    {
        armorParent.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(value/(float)armorMax*armorW, armorParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        armorParent.GetChild(1).GetComponent<Text>().text = value.ToString();
    }
}
