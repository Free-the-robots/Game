using System.Collections;
using System.IO;
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

    public Dictionary<int, Projectiles.ProjectileData> weaponData = new Dictionary<int, Projectiles.ProjectileData>();
    public List<NEAT.Person> evoWeaponData = new List<NEAT.Person>();
    public Dictionary<int,GameObject> turretObject = new Dictionary<int, GameObject>();

    public static AssetDataManager Instance { get { return instance; } }

    public bool loadedAssets = false;

    private string ships;
    private string weapons;

    private AsyncOperationHandle downloadAsync;
    private bool downloading = false;

    private float actualPercent = 0f;
    private float maxDownload = 0f;
    private List<float> downloadSize = new List<float>();
    private int downloadIndex = 0;

    private const string shipURL = "Assets/Prefabs/Spaceships/";
    private const string weaponURL = "Assets/Prefabs/Turrets/";

    public string version = "";

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
        {
            instance = this;

            StartCoroutine(Setup());
        }

        DontDestroyOnLoad(this);
    }

    private IEnumerator Setup()
    {
        yield return StartCoroutine(CheckVersion());
        yield return StartCoroutine(updateSetup());

        //Get list of spaceships and download dependencies
        if(maxDownload > 0)
            yield return StartCoroutine(DownloadAssets());

        yield return StartCoroutine(LoadAssets());
    }

    private IEnumerator CheckVersion()
    {
        //Debug.Log("retrieving version");
        //UnityWebRequest www = UnityWebRequest.Get("https://free-the-robots.s3.eu-central-1.amazonaws.com/version.txt");
        //yield return www.SendWebRequest();

        //if (www.isNetworkError || www.isHttpError)
        //{
        //    Debug.Log(www.error);
        //}
        //else
        //{
        //    version = www.downloadHandler.text;
        //}
        version = "0.1";
        if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + ".v.d"))
        {
            string localVersion = EncryptDecrypt.LoadDecryptFileString(Application.persistentDataPath + Path.DirectorySeparatorChar + ".v.d");
            if (version.Equals(localVersion))
            {
                ships = EncryptDecrypt.LoadDecryptFileString(Application.persistentDataPath + Path.DirectorySeparatorChar + ".ships.d");
                weapons = EncryptDecrypt.LoadDecryptFileString(Application.persistentDataPath + Path.DirectorySeparatorChar + ".weapons.d");
                weapons += "\nTurret100";
            }
            else
            {
                Debug.Log("local version different. updating version");
                yield return StartCoroutine(UpdateVersion());
            }
        }
        else
        {
            Debug.Log("no local version");
            yield return StartCoroutine(UpdateVersion());
        }

        yield return null;
    }

    private IEnumerator UpdateVersion()
    {
        yield return StartCoroutine(EncryptDecrypt.StoreEncryptFile(Application.persistentDataPath + Path.DirectorySeparatorChar + ".v.d", version));

        UnityWebRequest www = UnityWebRequest.Get("https://free-the-robots.s3.eu-central-1.amazonaws.com/assets.txt");
        yield return www.SendWebRequest();
        string assets = "";
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            assets = www.downloadHandler.text;
        }
        ships = assets.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries)[0];
        weapons = assets.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries)[1];

        yield return StartCoroutine(EncryptDecrypt.StoreEncryptFile(Application.persistentDataPath + Path.DirectorySeparatorChar + ".ships.d", ships));
        yield return StartCoroutine(EncryptDecrypt.StoreEncryptFile(Application.persistentDataPath + Path.DirectorySeparatorChar + ".weapons.d", weapons));
    }

    private void OnLoadDonePrefab(AsyncOperationHandle<GameObject> obj)
    {
        // In a production environment, you should add exception handling to catch scenarios such as a null result.
        Transform model3D = obj.Result.transform.GetChild(0);
        spaceshipScriptableData.Add(obj.Result.GetComponent<PlayerSpaceship>().spaceshipData);
        spaceshipMaterial.Add(model3D.GetComponent<MeshRenderer>().sharedMaterial);
        //spaceshipMesh.Add(model3D.GetComponent<MeshFilter>().sharedMesh);
        spaceshipObject.Add(obj.Result);
        actualPercent += 1f;
    }

    private void OnLoadDonePrefabWeapon(AsyncOperationHandle<GameObject> obj)
    {
        // In a production environment, you should add exception handling to catch scenarios such as a null result.
        turretObject[System.Convert.ToInt32(obj.Result.name.Remove(0,6))] = obj.Result;
        if(obj.Result.GetComponent<Weapon.Turret>().projectileData != null)
            weaponData[obj.Result.GetComponent<Weapon.Turret>().projectileData.id] = obj.Result.GetComponent<Weapon.Turret>().projectileData;
        actualPercent += 1f;
    }

    IEnumerator LoadAssets()
    {
        downloading = true;

        actualPercent = 0f;
        LoadingManager.Instance.resetLoading(ships.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.None).Length);
        LoadingManager.Instance.enableLoading("Loading Ships...");

        //Load Spaceships
        foreach (string line in ships.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            AsyncOperationHandle<GameObject> async =  Addressables.LoadAssetAsync<GameObject>(shipURL + line + ".prefab");
            async.Completed += OnLoadDonePrefab;
            downloadAsync = async;
            yield return async;
        }

        actualPercent = 0f;
        LoadingManager.Instance.resetLoading(weapons.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.None).Length);
        LoadingManager.Instance.enableLoading("Loading Turrets...");
        //Load Turrets
        foreach (string line in weapons.Split(new[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries))
        {
            AsyncOperationHandle<GameObject> async = Addressables.LoadAssetAsync<GameObject>(weaponURL + line + ".prefab");
            async.Completed += OnLoadDonePrefabWeapon;
            downloadAsync = async;
            yield return async;
        }

        LoadingManager.Instance.disableLoading();
        downloading = false;
        loadedAssets = true;
    }

    IEnumerator DownloadAssets()
    {
        actualPercent = 0f;
        LoadingManager.Instance.resetLoading(1f);
        LoadingManager.Instance.enableLoading("Downloading Assets...");
        downloading = true;

        downloadIndex = 0;
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

        LoadingManager.Instance.disableLoading();
        downloading = false;
    }

    private void FinishedDownloading(AsyncOperationHandle obj)
    {
        actualPercent += downloadSize[downloadIndex]/maxDownload;
        downloadIndex++;
    }

    private void Update()
    {
        if (downloading)
        {
            if(downloadAsync.PercentComplete != 1)
                LoadingManager.Instance.setPercent(actualPercent + downloadAsync.PercentComplete);
            else
                LoadingManager.Instance.setPercent(actualPercent);
        }
    }

    IEnumerator updateSetup()
    {
        downloadSize = new List<float>();
        maxDownload = 0f;
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

    private void MeasureDownloading(AsyncOperationHandle<long> obj)
    {
        maxDownload += obj.Result;
        downloadSize.Add(obj.Result);
    }
}
