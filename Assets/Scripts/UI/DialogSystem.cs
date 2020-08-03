using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class DialogSystem : MonoBehaviour
{
    //public GameObject DialogSystemCanvas { get { return DialogSystemGO; } set { DialogSystemGO = value; } }

    [SerializeField] private Button SkipDialogSystem;
    [SerializeField] private Button SkipSentence;
    [SerializeField] private RawImage ImageDisplayed;
    [SerializeField] private TextMeshProUGUI TextDisplayed;
    
    private const string PATH_DIALOG_SYSTEM = "Assets/DialogSystemComponent/";
    private const string EXTENSION_FILE_DIALOG = ".json";
    private const string DEBUG_SCRIPT = "Dialog System : ";
    //private AsyncOperationHandle _currentJSONOperationHandle;

    public RawImage ImageAboveText { get; set; }

    private DialogDataList mListData = new DialogDataList();
    private DialogData mDialogDataToDisplay = new DialogData();
    private Action<bool> DialogDone;

    private GameObject mObjectToDisplay;

    private bool mIsDoneParse;

    private int mCounterListDialog;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        mCounterListDialog = 0;
        SkipDialogSystem.onClick.AddListener(ButtonSkipDialog);
        mIsDoneParse = false;
        //DialogData Data1 = new DialogData();
        //Data1.NameEntitySpeaking = "Character1";
        //Data1.PathImageDisplayed = "Path://Asset/Img/";
        //Data1.TextDisplayed = "eazryeryjijogNZOUGHUIg.iohrniutnrhntorhnon, iroethiu.";
        //DialogData Data2 = new DialogData();
        //Data2.NameEntitySpeaking = "Character2";
        //Data2.PathImageDisplayed = "Path://Asset/Img/test/";
        //Data2.TextDisplayed = "erzytqjj. tqtytyqy. qeytqy teqyqety.";
        //mListData.ListDialogData = new List<DialogData>();
        //mListData.ListDialogData.Add(Data1);
        //mListData.ListDialogData.Add(Data2);
        //mJson = JsonUtility.ToJson(mListData);
        //SaveDataToFile("Assets/test.json", mJson);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Check if finger is over a UI element
            if (/*EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)*/IsPointerOverUIObject())
            {
                Debug.Log("Touched the UI");
                mDialogDataToDisplay.NameEntitySpeaking = "test click";
                mDialogDataToDisplay.PathImageDisplayed = mListData.ListDialogData[0].PathImageDisplayed;
                mDialogDataToDisplay.TextDisplayed = "testclick";

                TextDisplayed.SetText(mDialogDataToDisplay.NameEntitySpeaking + " : \n" + mDialogDataToDisplay.TextDisplayed);
                //Change the line of the dialog from the dialog opened
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    /// <summary>
    /// Display the text using TextMeshpro
    /// </summary>
    private void DisplayText()
    {
        if(mListData.ListDialogData == null || mListData.ListDialogData.Count == 0)
        {
            Debug.LogError(DEBUG_SCRIPT + " The list you tried to use to display the dialog is null or empty.");
        }
        else
        {
            if (mCounterListDialog <= mListData.ListDialogData.Count)
            {
                mDialogDataToDisplay.NameEntitySpeaking = mListData.ListDialogData[mCounterListDialog].NameEntitySpeaking;
                mDialogDataToDisplay.PathImageDisplayed = mListData.ListDialogData[mCounterListDialog].PathImageDisplayed;
                mDialogDataToDisplay.TextDisplayed = mListData.ListDialogData[mCounterListDialog].TextDisplayed;

                TextDisplayed.SetText(mDialogDataToDisplay.NameEntitySpeaking + " : \n" + mDialogDataToDisplay.TextDisplayed);
                ++mCounterListDialog;
            }

        }
    }

    /// <summary>
    /// Instantiate the dialog system
    /// </summary>
    /// <param name="iDialogSystemGO">The gameobject which represent the dialog system</param>
    /// <param name="iDialogDataName">Name of the Json to be used for the dialog system</param>
    public void InstantiateDialogSystem(GameObject iDialogSystemGO, string iDialogDataName, Action<bool> iFunctionEndDialog)
    {
        iDialogSystemGO.SetActive(true);
        ParseDialogData(iDialogDataName);
        Debug.Log(iDialogDataName);
    }

    /// <summary>
    /// Go to the next sentence/image
    /// </summary>
    public void ButtonSkipCharacter()
    {

    }

    /// <summary>
    /// Disable the Dialog system
    /// </summary>
    public void ButtonSkipDialog()
    {
        //Disable or destroy the DialogSystem gameobject
        this.gameObject.SetActive(false);

    }

    /// <summary>
    /// Parse JSON.
    /// For the moment it will be normal text and then it will be JSON dont forget to integrate NewtonSoft api because there is some problem with deserialize Json from unity
    /// </summary>
    /// <param name="iNameJSON"></param>
    /// <returns></returns>
    private void ParseDialogData(string iNameJSON)
    {
        Debug.Log("PARSE DIALOG DATA JSON NAME : " + iNameJSON);
        StartCoroutine(DownloadAdressableJSON(iNameJSON));
    }

    public void SaveDataToFile(string path, string json)
    {
        var filePath = Path.Combine(Application.persistentDataPath, path);
        Debug.Log(filePath);
        File.WriteAllText(path, json);
    }

    private IEnumerator DownloadAdressableJSON(string iNameJSON)
    {
        Debug.LogError("enumerator addressable JSON : " + iNameJSON);
        //if (_currentJSONOperationHandle.IsValid())
        //    Addressables.Release(_currentJSONOperationHandle);
        //Debug.Log(AddressableTextAsset[0].Asset.name);
        //Debug.LogError(PATH_DIALOG_SYSTEM + iNameJSON + EXTENSION_FILE_DIALOG);
        AsyncOperationHandle<TextAsset> async = Addressables.LoadAssetAsync<TextAsset>(PATH_DIALOG_SYSTEM + iNameJSON + EXTENSION_FILE_DIALOG);
        async.Completed += handle => 
        {
            if(handle.Result == null)
            {
                Debug.LogError(DEBUG_SCRIPT + " There is no result for the AsyncOperationHandle for this path - " + PATH_DIALOG_SYSTEM + iNameJSON + EXTENSION_FILE_DIALOG);
            }
            else
            {
                Debug.Log(handle.Result);
                try
                {
                    mListData = JsonUtility.FromJson<DialogDataList>(handle.Result.ToString());
                    Debug.Log(mListData.ListDialogData[0].NameEntitySpeaking + " : " + mListData.ListDialogData[0].TextDisplayed + " with img : " + mListData.ListDialogData[0].PathImageDisplayed);
                    mIsDoneParse = true;
                }
                catch (Exception e)
                {
                    Debug.LogError(DEBUG_SCRIPT + " Error parsing with the file at this path - " + PATH_DIALOG_SYSTEM + iNameJSON + EXTENSION_FILE_DIALOG + ". Error : " + e.Message);
                }
            }
               
        };
        yield return new WaitUntil (() => mIsDoneParse);
        DisplayText();
        //AddressableTextAsset.Find(item => item.ToString().Contains(iNameJSON));
        //var mTextAsset = AssetRef.LoadAssetAsync<TextAsset>();
        //yield return mTextAsset;
        //if (mTextAsset.Result != null)
        //{
        //    TextAsset testTextAsset;
        //    testTextAsset = mTextAsset.Result;
        //    Debug.Log("AssetREF TEXT ASSET : " + testTextAsset.text);
        //}
        //else
        //    Debug.LogError("PROBLEME ADDRESSABLE COROUTINE");


    }

    private void DialogSystem_Completed(AsyncOperationHandle<TextAsset> obj)
    {
        throw new System.NotImplementedException();
    }


}
