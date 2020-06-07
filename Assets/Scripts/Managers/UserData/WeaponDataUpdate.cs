using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserData;

public class WeaponDataUpdate : MonoBehaviour
{
    private WeaponData actualWeapon;
    private Projectiles.ProjectileData actualWeaponData;
    private Transform actualWeaponObject;

    public Text weaponName;
    public Text Level;
    public FillUIParent xP;

    public Transform weaponContent;
    public GameObject ScrollButton;

    public ToggleGroup togglesGroup;
    public ToggleGroup weaponToggleGroup;
    public RectTransform weaponRawImage;

    public WeaponStatsUpdate weaponStats;
    public WeaponStatsUpdate weaponEvoStats;

    public Transform ToCamera;
    public Transform ToCameraOrigin;
    private float toOffset;
    public Camera secondCamera;

    public Transform weapon3D;

    // Start is called before the first frame update
    void OnEnable()
    {
        secondCamera.GetComponent<CameraOrthoPerspLerp>().enable();
        secondCamera.GetComponent<LerpToWhenEnabled>().lerpRotate = true;
        toOffset = secondCamera.GetComponent<LerpToWhenEnabled>().toOffset.z;
        secondCamera.GetComponent<LerpToWhenEnabled>().setToTransform(ToCamera);
        secondCamera.GetComponent<LerpToWhenEnabled>().setOffsetZTo(0f);
        secondCamera.GetComponent<LerpToWhenEnabled>().enable();
        weapon3D.gameObject.SetActive(true);

        //Get Data from user data
        UserData.UserData userData = UserDataManager.Instance.userData;
        AssetDataManager assetData = AssetDataManager.Instance;
        actualWeapon = userData.weapons.FirstOrDefault();
        if(actualWeapon != null)
            actualWeaponData = assetData.weaponData[actualWeapon.id];

        //Update stats and info
        UpdateStats();

        //Active gameobjects
        weapon3D.gameObject.SetActive(true);
        ActivateWeapon(userData.weapons.IndexOf(actualWeapon));

        ToggleGroup toggleGroup = weaponContent.GetComponent<ToggleGroup>();

        for (int i = 0; i < userData.weapons.Count; ++i)
        {
            GameObject button = GameObject.Instantiate(ScrollButton);
            Toggle toggle = button.GetComponent<Toggle>();
            button.transform.SetParent(weaponContent);
            int tmpI = i;
            toggle.onValueChanged.AddListener((bool val) => UpdateWeapon(val, tmpI));
            toggle.group = toggleGroup;
            button.GetComponentInChildren<Text>().text = " " + i;
        }
        if(actualWeapon != null)
            weaponContent.GetComponentsInChildren<Toggle>()[actualWeapon.id].isOn = true;


        ToggleGroup toggleGroupWeapon = weaponContent.GetComponent<ToggleGroup>();

        for (int i = 0; i < userData.weapons.Count; ++i)
        {
            GameObject button = GameObject.Instantiate(ScrollButton);
            Toggle toggle = button.GetComponent<Toggle>();
            button.transform.SetParent(weaponContent);
            int tmpI = userData.weapons[i].id;
            toggle.onValueChanged.AddListener((bool val) => UpdateWeapon(val, tmpI));
            toggle.group = toggleGroup;
            button.GetComponentInChildren<Text>().text = " " + i;
        }
    }

    public void UpdateWeapon(bool toggle, int index)
    {
        //if (toggle)
        //{
        //    UserData.UserData userData = UserDataManager.Instance.userData;
        //    AssetDataManager assetData = AssetDataManager.Instance;
        //    if (actualWeaponData.id != index)
        //    {
        //        actualWeapon = userData.weapons.Find(i => i.id == index);
        //        actualWeaponData = assetData.weaponData.Find(obj => obj.id == index);

        //        UpdateStats();

        //        ActivateWeapon(index);
        //    }
        //    else
        //    {
        //        if (userData.shipEquiped != index && actualWeapon != null && actualWeapon.unlocked)
        //        {
        //            Debug.Log("Equip " + index);
        //            userData.shipEquiped = index;
        //            UserDataManager.Instance.SaveData();
        //        }
        //        if (actualWeapon == null || !actualWeapon.unlocked)
        //            Debug.Log("Not unlocked " + index);
        //    }
        //}
        //else
        //{
        //    DisableWeapon(index);
        //}
    }

    public void DisableWeapon(int id)
    {
        weapon3D.GetChild(id).gameObject.SetActive(false);
        actualWeaponObject = null;
    }

    public void ActivateWeapon(int id)
    {
        actualWeaponObject = weapon3D.GetChild(id);
        actualWeaponObject.gameObject.SetActive(true);
    }

    public void UpdateStats()
    {
        weaponStats.UpdateStat(actualWeaponData.damage, actualWeaponData.frequency, actualWeaponData.lifeTime);
    }

    public void Disable()
    {
        secondCamera.GetComponent<CameraOrthoPerspLerp>().enable();
        secondCamera.GetComponent<LerpToWhenEnabled>().enable();
        GetComponent<CanvasGroupFade>().enable();
    }

    private void OnDisable()
    {
        secondCamera.GetComponent<LerpToWhenEnabled>().setOffsetZTo(toOffset);
        secondCamera.GetComponent<LerpToWhenEnabled>().setToTransform(ToCameraOrigin);
        secondCamera.GetComponent<LerpToWhenEnabled>().lerpRotate = false;
        DisableWeapon(actualWeaponData.id);
        foreach (Transform transform in weaponContent)
        {
            GameObject.Destroy(transform.gameObject);
        }
        weapon3D.gameObject.SetActive(false);
    }

    public void UpdateWeaponPage(bool toggle)
    {
        if (toggle)
        {
            weapon3D.GetComponent<RotateWhenEnabled>().enabled = false;

            foreach (Toggle toggleWeapon in weaponToggleGroup.GetComponentsInChildren<Toggle>())
            {
                int i = toggleWeapon.transform.GetSiblingIndex();
                //toggleWeapon.interactable = i < actualTurrets.Count && i >= (actualTurrets.Count - actualWeaponData.modifiableTurretCount);
            }
        }
        else
        {
            weapon3D.GetComponent<RotateWhenEnabled>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
