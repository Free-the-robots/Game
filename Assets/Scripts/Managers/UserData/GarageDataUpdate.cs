using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UserData;

public class GarageDataUpdate : MonoBehaviour
{
    private ShipData actualShip;
    private SpaceshipData actualShipData;
    private Transform actualShipObject;
    private List<ShipData> lockedShips;
    private List<ShipData> unlockedShips;

    public Text shipName;
    public Text Level;
    public FillUIParent xP;
    public StatsUpdate stats;

    public Camera secondCamera;

    public Transform ship3D;

    public Transform shipContent;
    public Transform weaponContent;
    public GameObject ScrollButton;

    public ToggleGroup togglesGroup;
    public ToggleGroup weaponToggleGroup;
    public ToggleGroup weapon3DToggle;
    public GameObject weapon3DToggleObj;
    public RectTransform shipRawImage;

    private List<Weapon.Turret> actualTurrets = new List<Weapon.Turret>();

    // Start is called before the first frame update
    void OnEnable()
    {
        secondCamera.GetComponent<CameraOrthoPerspLerp>().enable();
        ship3D.gameObject.SetActive(true);

        //Get Data from user data
        UserData.UserData userData = UserDataManager.Instance.userData;
        AssetDataManager assetData = AssetDataManager.Instance;
        actualShip = userData.ships.Find(i => i.id == userData.shipEquiped);
        actualShipData = assetData.spaceshipScriptableData.Find(obj => obj.id == actualShip.id);

        lockedShips = userData.ships.Where(i => !i.unlocked).OrderBy(obj=>obj.id).ToList();
        unlockedShips = userData.ships.Where(i => i.unlocked).OrderBy(obj => obj.id).ToList();

        //Update stats and info
        UpdateStats();

        //Active gameobjects
        ship3D.gameObject.SetActive(true);
        ActivateShip(actualShip.id);

        ToggleGroup toggleGroup = shipContent.GetComponent<ToggleGroup>();

        for (int i = 0; i < AssetDataManager.Instance.spaceshipObject.Count; ++i)
        {
            GameObject button = GameObject.Instantiate(ScrollButton);
            Toggle toggle = button.GetComponent<Toggle>();
            button.transform.SetParent(shipContent);
            int tmpI = i;
            toggle.onValueChanged.AddListener((bool val) => UpdateShip(val, tmpI));
            toggle.group = toggleGroup;
            button.GetComponentInChildren<Text>().text = " " + i;
            //toggle.interactable = false;
            //if (i < userData.ships.Count)
            //{
            //    toggle.interactable = userData.ships[i].unlocked;
            //}
        }
        shipContent.GetComponentsInChildren<Toggle>()[actualShip.id].isOn = true;


        ToggleGroup toggleGroupWeapon = weaponContent.GetComponent<ToggleGroup>();
        
        for (int i = 0; i < userData.weapons.Count; ++i)
        {
            GameObject button = GameObject.Instantiate(ScrollButton);
            Toggle toggle = button.GetComponent<Toggle>();
            button.transform.SetParent(shipContent);
            int tmpI = userData.weapons[i].id;
            toggle.onValueChanged.AddListener((bool val) => UpdateWeapon(val, tmpI));
            toggle.group = toggleGroup;
            button.GetComponentInChildren<Text>().text = " " + i;
        }
    }

    public void UpdateShip(bool toggle, int index)
    {
        if (toggle)
        {
            UserData.UserData userData = UserDataManager.Instance.userData;
            AssetDataManager assetData = AssetDataManager.Instance;
            if (actualShipData.id != index)
            {
                actualShip = userData.ships.Find(i => i.id == index);
                actualShipData = assetData.spaceshipScriptableData.Find(obj => obj.id == index);

                UpdateStats();

                ActivateShip(index);
            }
            else
            {
                if(userData.shipEquiped != index && actualShip != null && actualShip.unlocked)
                {
                    Debug.Log("Equip " + index);
                    userData.shipEquiped = index;
                    UserDataManager.Instance.SaveData();
                }
                if(actualShip == null || !actualShip.unlocked)
                    Debug.Log("Not unlocked " + index);
            }
        }
        else
        {
            DisableShip(index);
        }
    }

    public void UpdateWeapon(bool toggle, int index)
    {
        if (toggle)
        {
            int i = weaponToggleGroup.ActiveToggles().FirstOrDefault().transform.GetSiblingIndex();
            //TODO Update weapon on 3D, and userdata

        }
    }

    public void DisableShip(int id)
    {
        ship3D.GetChild(id).gameObject.SetActive(false);
        actualShipObject = null;
    }

    public void ActivateShip(int id)
    {
        actualShipObject = ship3D.GetChild(id);
        actualShipObject.gameObject.SetActive(true);
    }

    public void UpdateStats()
    {
        //Update stats and info
        float[] stat = new float[6] { actualShipData.life / actualShipData.lifeMax, actualShipData.speed, actualShipData.shield, actualShipData.damage, actualShipData.armor, actualShipData.rarity / 6f };
        stats.updateStats(stat);

        shipName.text = actualShipData.name;

        if(actualShip != null)
        {
            xP.maxPercentile = actualShip.exp / 1000f;
            Level.text = "Lvl : " + actualShip.level;
        }
        else
        {
            xP.maxPercentile = 0f;
            Level.text = "Lvl : 0";
        }
    }

    private void OnDisable()
    {
        DisableShip(actualShipData.id);
        foreach (Transform transform in shipContent)
        {
            GameObject.Destroy(transform.gameObject);
        }
        ship3D.gameObject.SetActive(false);
    }

    bool animating = false;
    float t = 0f;
    private Quaternion rotationShip;

    public void UpdateWeaponPage(bool toggle)
    {
        if (toggle)
        {
            ship3D.GetComponent<RotateWhenEnabled>().enabled = false;
            animating = true;
            t = 0f;
            rotationShip = ship3D.rotation;

            actualTurrets = actualShipObject.GetComponentsInChildren<Weapon.Turret>().ToList();
            foreach(Toggle toggleWeapon in weaponToggleGroup.GetComponentsInChildren<Toggle>())
            {
                int i = toggleWeapon.transform.GetSiblingIndex();
                toggleWeapon.interactable = i < actualTurrets.Count && i >= (actualTurrets.Count - actualShipData.modifiableTurretCount);
            }

            updateToggleWeapon();
        }
        else
        {
            ship3D.GetComponent<RotateWhenEnabled>().enabled = true;
            foreach(Transform trans in weapon3DToggle.transform)
            {
                GameObject.Destroy(trans.gameObject);
            }
        }
    }

    private void updateToggleWeapon()
    {
        //Project 3D object to screen
        RawImage rawImage = shipRawImage.GetComponent<RawImage>();
        Vector2 cornerPos = shipRawImage.rect.min + new Vector2(shipRawImage.position.x, shipRawImage.position.y);
        foreach (Weapon.Turret turret in actualTurrets)
        {
            Vector3 pos = secondCamera.WorldToScreenPoint(turret.transform.position);
            pos.Scale(new Vector3(shipRawImage.rect.width / rawImage.mainTexture.width, shipRawImage.rect.height / rawImage.mainTexture.height, 0f));
            pos += new Vector3(cornerPos.x, cornerPos.y, 0f);
            GameObject wpT = GameObject.Instantiate(weapon3DToggleObj);
            wpT.transform.SetParent(weapon3DToggle.transform);
            wpT.GetComponent<RectTransform>().position = pos;
            Toggle toggle = wpT.GetComponent<Toggle>();
            toggle.group = weapon3DToggle;
            toggle.onValueChanged.AddListener((bool val) => updateWeaponFrom3D(val));
            toggle.isOn = false;
        }
        weapon3DToggle.transform.GetChild(0).GetComponent<Toggle>().isOn = true;
    }

    private void updateWeaponFrom3D(bool toggle)
    {
        if (toggle && weapon3DToggle.ActiveToggles().FirstOrDefault() != null)
        {
            int i = weapon3DToggle.ActiveToggles().FirstOrDefault().transform.GetSiblingIndex();
            weaponToggleGroup.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
        }
    }

    public void updateWeaponFromToggle(bool toggle)
    {
        if (toggle)
        {
            int i = weaponToggleGroup.ActiveToggles().FirstOrDefault().transform.GetSiblingIndex();
            weapon3DToggle.transform.GetChild(i).GetComponent<Toggle>().isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            t += Time.deltaTime;
            ship3D.rotation = Quaternion.Lerp(rotationShip, Quaternion.Euler(new Vector3(0f, 190f, 0f)), t * t * (3 - 2 * t));

            RawImage rawImage = shipRawImage.GetComponent<RawImage>();
            Vector2 cornerPos = shipRawImage.rect.min + new Vector2(shipRawImage.position.x, shipRawImage.position.y);
            int i = 0;
            foreach (Transform weaponToggle in weapon3DToggle.transform)
            {
                Vector3 pos = secondCamera.WorldToScreenPoint(actualTurrets[i].transform.position);
                pos.Scale(new Vector3(shipRawImage.rect.width / rawImage.mainTexture.width, shipRawImage.rect.height / rawImage.mainTexture.height, 0f));
                pos += new Vector3(cornerPos.x, cornerPos.y, 0f);
                weaponToggle.position = pos;
                i++;
            }
            if (t > 1f)
            {
                t = 0f;
                animating = false;
                ship3D.rotation = Quaternion.Euler(new Vector3(0f, 190f, 0f));
            }
        }
    }
}
