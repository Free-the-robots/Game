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

    private void OnEnable()
    {
        UserData.UserData userData = UserData.UserDataManager.Instance.userData;
        lifeMax = (int)(AssetDataManager.Instance.spaceshipScriptableData[userData.shipEquiped].lifeMax);
        armorMax = (int)(AssetDataManager.Instance.spaceshipScriptableData[userData.shipEquiped].armor);

        lifeW = lifeParent.rect.width;
        lifeH = lifeParent.rect.height;
        armorW = armorParent.rect.width;
        armorH = armorParent.rect.height;

        lifeParent.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(lifeW, lifeParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        lifeParent.GetChild(1).GetComponent<Text>().text = lifeMax.ToString();
        Debug.Log(lifeMax);
        armorParent.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(armorW, armorParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        armorParent.GetChild(1).GetComponent<Text>().text = armorMax.ToString();
    }

    public void updateLife(int value)
    {
        Debug.Log(value);
        lifeParent.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(value/(float)lifeMax*lifeW, lifeParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        lifeParent.GetChild(1).GetComponent<Text>().text = value.ToString();
    }

    public void updateArmor(int value)
    {
        armorParent.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(value/(float)armorMax*armorW, armorParent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
        armorParent.GetChild(1).GetComponent<Text>().text = value.ToString();
    }
}
