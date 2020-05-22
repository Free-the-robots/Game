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
    private List<ShipData> lockedShips;
    private List<ShipData> unlockedShips;

    public Text shipName;
    public Text Level;
    public FillUIParent xP;
    public StatsUpdate stats;

    public Camera secondCamera;

    public Transform shipTransform;

    public Transform shipContent;
    public GameObject ScrollButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        secondCamera.GetComponent<CameraOrthoPerspLerp>().enabled = true;

        //Get Data from user data
        UserDataManager userManager = UserDataManager.Instance;
        UserData.UserData userData = userManager.userData;
        actualShip = userData.ships.Find(i => i.id == userData.shipEquiped);
        actualShipData = userManager.spaceshipScriptableData.Find(obj => obj.id == actualShip.id);

        lockedShips = userData.ships.Where(i => !i.unlocked).OrderBy(obj=>obj.id).ToList();
        unlockedShips = userData.ships.Where(i => i.unlocked).OrderBy(obj => obj.id).ToList();

        //Update stats and info
        UpdateStats();

        //Active gameobjects
        shipTransform.gameObject.SetActive(true);
        shipTransform.GetChild(actualShip.id).gameObject.SetActive(true);

        ToggleGroup toggleGroup = shipContent.GetComponent<ToggleGroup>();
        for(int i = 0; i < 10; ++i)
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

        UserDataManager userManager = UserDataManager.Instance;
        UserData.UserData userData = userManager.userData;
        actualShip = userData.ships.Find(i => i.id == toggleGroup.ActiveToggles().FirstOrDefault().transform.GetSiblingIndex());
        actualShipData = userManager.spaceshipScriptableData.Find(obj => obj.id == actualShip.id);

        ActivateShip(actualShip.id);
    }

    public void DisableShip(int id)
    {
        shipTransform.GetChild(id).gameObject.SetActive(false);
    }

    public void ActivateShip(int id)
    {
        shipTransform.GetChild(actualShip.id).gameObject.SetActive(true);
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
        shipTransform.gameObject.SetActive(false);
        DisableShip(actualShip.id);
        secondCamera.GetComponent<CameraOrthoPerspLerp>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
