using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserData;

public class WeaponDataUpdate : MonoBehaviour
{
    private WeaponData actualWeapon;
    private EvoWeaponData actualEvoWeapon;
    private Projectiles.ProjectileData actualWeaponData;
    private Projectiles.ParticleEvolutive actualEvoWeaponData;
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
    public Camera secondCamera;

    public Transform weapon3D;

    // Start is called before the first frame update
    void OnEnable()
    {
        secondCamera.GetComponent<CameraOrthoPerspLerp>().enable();
        secondCamera.GetComponent<LerpToWhenEnabled>().lerpRotate = true;
        secondCamera.GetComponent<LerpToWhenEnabled>().setToTransform(ToCamera);
        secondCamera.GetComponent<LerpToWhenEnabled>().enable();
        weapon3D.gameObject.SetActive(true);

        //Get Data from user data
        UserData.UserData userData = UserDataManager.Instance.userData;
        AssetDataManager assetData = AssetDataManager.Instance;
        actualWeapon = userData.weapons.FirstOrDefault();
        if (actualWeapon != null)
            actualWeaponData = assetData.weaponData[actualWeapon.id];

        actualEvoWeapon = userData.evoweapons.FirstOrDefault();
        if (actualEvoWeapon == null)
            togglesGroup.GetComponentsInChildren<Toggle>()[1].interactable = false;

        //Update stats and info
        UpdateStats();

        //Active gameobjects
        weapon3D.gameObject.SetActive(true);
        if (actualWeapon != null)
            ActivateWeapon(actualWeapon.id);

        ToggleGroup toggleGroup = weaponContent.GetComponent<ToggleGroup>();

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
        if(actualWeapon != null)
            weaponContent.GetComponentsInChildren<Toggle>()[actualWeapon.id].isOn = true;


        ToggleGroup toggleGroupWeapon = weaponContent.GetComponent<ToggleGroup>();

        for (int i = 0; i < userData.evoweapons.Count; ++i)
        {
            GameObject button = GameObject.Instantiate(ScrollButton);
            Toggle toggle = button.GetComponent<Toggle>();
            button.transform.SetParent(weaponContent);
            int tmpI = i;
            toggle.onValueChanged.AddListener((bool val) => UpdateEvoWeapon(val, tmpI));
            toggle.group = toggleGroup;
            button.GetComponentInChildren<Text>().text = " " + i;
        }
    }

    public void UpdateWeapon(bool toggle, int index)
    {
        if (toggle)
        {
            UserData.UserData userData = UserDataManager.Instance.userData;
            AssetDataManager assetData = AssetDataManager.Instance;

            actualWeapon = userData.weapons[index];
            if (actualWeapon != null)
                actualWeaponData = assetData.weaponData[actualWeapon.id];

            UpdateStats();

            ActivateWeapon(index);
        }
        else
        {
            DisableWeapon();
        }
    }

    public void UpdateEvoWeapon(bool toggle, int index)
    {
        //if (toggle)
        //{
        //    UserData.UserData userData = UserDataManager.Instance.userData;
        //    AssetDataManager assetData = AssetDataManager.Instance;

        //    actualEvoWeapon = userData.evoweapons[index];

        //    UpdateEvoStats();

        //    ActivateWeapon(index);
        //}
        //else
        //{
        //    DisableWeapon();
        //}
    }

    public void DisableWeapon()
    {
        if(actualWeaponObject != null)
        {
            GameObject.Destroy(weapon3D.GetChild(0).GetChild(0).gameObject);
            weapon3D.GetChild(0).gameObject.SetActive(false);
            actualWeaponObject = null;
        }
    }

    public void DisableEvoWeapon(int id)
    {
        //weapon3D.GetChild(id).gameObject.SetActive(false);
        //actualWeaponObject = null;
    }

    public void ActivateWeapon(int id)
    {
        if(actualWeaponObject == null)
        {
            GameObject obj = GameObject.Instantiate(AssetDataManager.Instance.turretObject[id]);
            obj.transform.SetParent(weapon3D.GetChild(0));
            obj.transform.SetAsFirstSibling();
            actualWeaponObject = obj.transform;

            weapon3D.GetChild(0).GetComponent<Spaceship>().weapon.Clear();
            weapon3D.GetChild(0).GetComponent<Spaceship>().weapon.Add(obj.GetComponent<Weapon.Turret>());
            actualWeaponObject.localPosition = Vector3.zero;

            actualWeaponObject.gameObject.SetActive(true);
            weapon3D.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void ActivateEvoWeapon(int id)
    {
        //actualEvoWeaponObject = weapon3D.GetChild(id);
        //actualEvoWeaponObject.gameObject.SetActive(true);
    }

    public void UpdateStats()
    {
        if(actualWeaponData != null)
            weaponStats.UpdateStat(actualWeaponData.damage, actualWeaponData.frequency, actualWeaponData.lifeTime);
    }

    public void UpdateEvoStats()
    {
        weaponEvoStats.UpdateStat(actualWeaponData.damage, actualWeaponData.frequency, actualWeaponData.lifeTime);
    }

    public void Disable()
    {
        secondCamera.GetComponent<CameraOrthoPerspLerp>().enable();
        secondCamera.GetComponent<LerpToWhenEnabled>().enable();
        GetComponent<CanvasGroupFade>().enable();
    }

    private void OnDisable()
    {
        secondCamera.GetComponent<LerpToWhenEnabled>().setToTransform(ToCameraOrigin);
        secondCamera.GetComponent<LerpToWhenEnabled>().lerpRotate = false;
        DisableWeapon();
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
