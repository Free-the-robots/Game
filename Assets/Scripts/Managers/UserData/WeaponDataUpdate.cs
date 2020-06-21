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
    private Projectiles.ProjectileData actualEvoWeaponData;
    private Transform actualWeaponObject;
    private Transform actualEvoWeaponObject;

    public Text weaponName;
    public Text Level;
    public FillUIParent xP;

    public Transform weaponContent;
    public Transform evoweaponContent;
    public GameObject ScrollButton;

    public ToggleGroup togglesGroup;
    public RectTransform weaponRawImage;

    public WeaponStatsUpdate weaponStats;
    public WeaponStatsUpdate weaponEvoStats;

    public Transform ToCamera;
    public Transform ToCameraOrigin;
    public Camera secondCamera;

    public Transform weapon3D;

    public Transform EvoWeapons3D;

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
        {
            actualWeaponData = assetData.weaponData[actualWeapon.id];
        }

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


        ToggleGroup toggleGroupWeapon = evoweaponContent.GetComponent<ToggleGroup>();

        for (int i = 0; i < userData.evoweapons.Count; ++i)
        {
            GameObject button = GameObject.Instantiate(ScrollButton);
            Toggle toggle = button.GetComponent<Toggle>();
            button.transform.SetParent(evoweaponContent);
            int tmpI = i;
            toggle.onValueChanged.AddListener((bool val) => UpdateEvoWeapon(val, tmpI));
            toggle.group = toggleGroup;
            button.GetComponentInChildren<Text>().text = " " + i;
        }
    }

    public void UpdateWeaponPage(bool toggle)
    {
        UpdateWeapon(toggle, actualWeapon.id);
    }

    public void UpdateEvoWeaponPage(bool toggle)
    {
        UpdateEvoWeapon(toggle, actualEvoWeapon.id);
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
        if (toggle)
        {
            UserData.UserData userData = UserDataManager.Instance.userData;
            AssetDataManager assetData = AssetDataManager.Instance;

            actualEvoWeapon = userData.evoweapons[index];

            UpdateEvoStats();

            ActivateEvoWeapon(index);
        }
        else
        {
            DisableEvoWeapon();
        }
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

    public void DisableEvoWeapon()
    {
        if (actualEvoWeaponObject != null)
        {
            GameObject.Destroy(weapon3D.GetChild(0).GetChild(0).gameObject);
            weapon3D.GetChild(0).gameObject.SetActive(false);
            actualWeaponObject = null;

            EvoWeapons3D.GetComponent<DrawEvoWeapons>().enabled = false;
        }
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
        if (actualEvoWeaponObject == null)
        {
            GameObject obj = GameObject.Instantiate(AssetDataManager.Instance.turretObject[100]);
            UserData.UserData userData = UserDataManager.Instance.userData;

            Projectiles.ProjectileEvolutiveData pData = ScriptableObject.CreateInstance<Projectiles.ProjectileEvolutiveData>();
            pData.init(userData.evoweapons[id]);
            pData.velocity = 10;
            pData.lifeTime = 2;

            actualEvoWeaponData = pData;

            obj.GetComponent<Weapon.Turret>().projectileData = pData;
            obj.transform.SetParent(weapon3D.GetChild(0));
            obj.transform.SetAsFirstSibling();
            actualEvoWeaponObject = obj.transform;

            weapon3D.GetChild(0).GetComponent<Spaceship>().weapon.Clear();
            weapon3D.GetChild(0).GetComponent<Spaceship>().weapon.Add(obj.GetComponent<Weapon.Turret>());
            actualEvoWeaponObject.localPosition = Vector3.zero;

            actualEvoWeaponObject.gameObject.SetActive(true);
            weapon3D.GetChild(0).gameObject.SetActive(true);

            EvoWeapons3D.GetComponent<DrawEvoWeapons>().behaviour = pData.behaviour;
            EvoWeapons3D.GetComponent<DrawEvoWeapons>().enabled = true;
        }
    }

    public void UpdateStats()
    {
        if(actualWeaponData != null)
            weaponStats.UpdateStat(actualWeaponData.damage, actualWeaponData.frequency, actualWeaponData.lifeTime);
    }

    public void UpdateEvoStats()
    {
        if (actualEvoWeaponData != null)
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
        DisableEvoWeapon();
        foreach (Transform transform in weaponContent)
        {
            GameObject.Destroy(transform.gameObject);
        }
        weapon3D.gameObject.SetActive(false);
    }
}
