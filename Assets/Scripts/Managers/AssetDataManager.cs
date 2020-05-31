using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetDataManager : MonoBehaviour
{
    private static AssetDataManager instance;

    public List<SpaceshipData> spaceshipScriptableData = new List<SpaceshipData>();
    public List<GameObject> spaceshipObject = new List<GameObject>();
    public List<Material> spaceshipMaterial = new List<Material>();

    public Dictionary<int,GameObject> turretObject = new Dictionary<int, GameObject>();

    public static AssetDataManager Instance { get { return instance; } }

    private string ships;
    private string weapons;

    private AsyncOperationHandle downloadAsync;
    private bool downloading = false;

    private float maxPercent = 0f;
    private float actualPercent = 0f;
    private float maxDownload = 0f;

    public GameObject loadingObject;
    private RectTransform rectLoading;

    private const string shipURL = "Assets/Prefabs/Spaceships/";
    private const string weaponURL = "Assets/Prefabs/Turrets/";

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        DontDestroyOnLoad(this);

        rectLoading = loadingObject.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();

        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://free-the-robots.s3.eu-central-1.amazonaws.com/ships.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            ships = www.downloadHandler.text;
        }

        www = UnityWebRequest.Get("https://free-the-robots.s3.eu-central-1.amazonaws.com/weapons.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            weapons = www.downloadHandler.text;
        }
        yield return StartCoroutine(updateSetup());

        //Get list of spaceships and download dependencies
        if(maxDownload > 0)
            yield return StartCoroutine(DownloadAssets());

        yield return StartCoroutine(LoadAssets());
    }

    private void OnLoadDonePrefab(AsyncOperationHandle<GameObject> obj)
    {
        // In a production environment, you should add exception handling to catch scenarios such as a null result.
        Transform model3D = obj.Result.transform.GetChild(0);
        spaceshipScriptableData.Add(obj.Result.GetComponent<PlayerSpaceship>().spaceshipData);
        spaceshipMaterial.Add(model3D.GetComponent<MeshRenderer>().sharedMaterial);
        //spaceshipMesh.Add(model3D.GetComponent<MeshFilter>().sharedMesh);
        spaceshipObject.Add(obj.Result);
    }

    private void OnLoadDonePrefabWeapon(AsyncOperationHandle<GameObject> obj)
    {
        // In a production environment, you should add exception handling to catch scenarios such as a null result.
        turretObject[System.Convert.ToInt32(obj.Result.name.Remove(6))] = obj.Result;
    }

    IEnumerator LoadAssets()
    {
        resetLoading();

        loadingObject.SetActive(true);
        downloading = true;
        maxPercent = ships.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.None).Length;

        //Load Spaceships
        foreach (string line in ships.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            AsyncOperationHandle<GameObject> async =  Addressables.LoadAssetAsync<GameObject>(shipURL + line + ".prefab");
            async.Completed += OnLoadDonePrefab;
            downloadAsync = async;
            yield return async;
        }

        //Load Turrets
        foreach (string line in weapons.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            AsyncOperationHandle<GameObject> async = Addressables.LoadAssetAsync<GameObject>(weaponURL + line + ".prefab");
            async.Completed += OnLoadDonePrefabWeapon;
            downloadAsync = async;
            yield return async;
        }

        loadingObject.SetActive(false);
        downloading = false;
    }

    IEnumerator DownloadAssets()
    {
        resetLoading();

        loadingObject.SetActive(true);
        downloading = true;
        maxPercent = ships.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.None).Length;


        foreach (string line in ships.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            AsyncOperationHandle async = Addressables.DownloadDependenciesAsync(shipURL + line + ".prefab");
            async.Completed += FinishedDownloading;
            downloadAsync = async;
            yield return async;
        }

        foreach (string line in weapons.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            AsyncOperationHandle async = Addressables.DownloadDependenciesAsync(weaponURL + line + ".prefab");
            async.Completed += FinishedDownloading;
            downloadAsync = async;
            yield return async;
        }

        loadingObject.SetActive(false);
        downloading = false;
    }

    private void FinishedDownloading(AsyncOperationHandle obj)
    {
        actualPercent += 1f;
    }

    private void Update()
    {
        if (downloading)
        {
            float maxWidth = loadingObject.transform.GetChild(0).GetComponent<RectTransform>().rect.width;
            float percent = actualPercent + downloadAsync.PercentComplete;
            rectLoading.sizeDelta = new Vector2(percent/maxPercent*maxWidth, rectLoading.sizeDelta.y);
        }
    }

    IEnumerator updateSetup()
    {
        foreach (string line in ships.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            AsyncOperationHandle<long> async = Addressables.GetDownloadSizeAsync(shipURL + line + ".prefab");
            async.Completed += MeasureDownloading;
            yield return downloadAsync;
        }

        foreach (string line in weapons.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            AsyncOperationHandle<long> async = Addressables.GetDownloadSizeAsync(weaponURL + line + ".prefab");
            async.Completed += MeasureDownloading;
            yield return downloadAsync;
        }
    }

    private void resetLoading()
    {
        rectLoading.sizeDelta = new Vector2(0f, rectLoading.sizeDelta.y);

        maxDownload = 0f;
        actualPercent = 0f;
    }

    private void MeasureDownloading(AsyncOperationHandle<long> obj)
    {
        maxDownload += obj.Result;
    }
}
