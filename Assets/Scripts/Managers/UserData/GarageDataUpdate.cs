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

    private List<Weapon.Turret> actualTurrets = new List<Weapon.Turret>();

    // Start is called before the first frame update
    void OnEnable()
    {
        secondCamera.GetComponent<CameraOrthoPerspLerp>().enabled = true;
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
            button.transform.SetParent(shipContent);
            button.GetComponent<Toggle>().onValueChanged.AddListener((bool val) => UpdateShip(val));
            button.GetComponent<Toggle>().group = toggleGroup;
            toggleGroup.ActiveToggles().FirstOrDefault();
            button.GetComponentInChildren<Text>().text = " " + i;
        }
    }

    public void UpdateShip(bool toggle)
    {
        ToggleGroup toggleGroup = shipContent.GetComponent<ToggleGroup>();

        DisableShip(actualShip.id);

        UserData.UserData userData = UserDataManager.Instance.userData;
        AssetDataManager assetData = AssetDataManager.Instance;
        actualShip = userData.ships.Find(i => i.id == toggleGroup.ActiveToggles().FirstOrDefault().transform.GetSiblingIndex());
        actualShipData = assetData.spaceshipScriptableData.Find(obj => obj.id == actualShip.id);

        UpdateStats();

        ActivateShip(actualShip.id);
    }

    public void DisableShip(int id)
    {
        actualShipObject.gameObject.SetActive(false);
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

        xP.maxPercentile = actualShip.exp / 1000f;
        Level.text = "Lvl : " + actualShip.level;
    }

    private void OnDisable()
    {
        ship3D.gameObject.SetActive(false);
        DisableShip(actualShip.id);
        foreach(Transform transform in shipContent)
        {
            GameObject.Destroy(transform.gameObject);
        }
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
                toggleWeapon.interactable = toggleWeapon.transform.GetSiblingIndex() < actualTurrets.Count;
            }
        }
        else
        {
            ship3D.GetComponent<RotateWhenEnabled>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            t += Time.deltaTime;
            ship3D.rotation = Quaternion.Lerp(rotationShip, Quaternion.Euler(new Vector3(0f, 190f, 0f)), t * t * (3 - 2 * t));
            if(t > 1f)
            {
                t = 0f;
                animating = false;
                ship3D.rotation = Quaternion.Euler(new Vector3(0f, 190f, 0f));
            }
        }

        if(togglesGroup.ActiveToggles().FirstOrDefault().transform.GetSiblingIndex() == 1)
        {

        }
    }
}
