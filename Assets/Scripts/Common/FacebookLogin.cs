using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookLogin : MonoBehaviour
{
    string Status;
    string LastResponse;
    List<string> friendsName = new List<string>();
    string fullname;
    string id;
    Sprite picture;

    void Awake()
    {
        if(UserData.UserDataManager.Instance.userData.userType == UserData.UserData.USERTYPE.FACEBOOK)
        {
            if (!FB.IsInitialized)
            {
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            }
            else
            {
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
            }
        }
    }

    public void FBSignIn()
    {
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends", "user_birthday" }, this.AuthCallback);
    }

    public void FBInvite(string message, string title)
    {
        FB.AppRequest(message, title: title);
    }

    public void FBShare(System.Uri content, string title, string description, System.Uri image)
    {
        FB.ShareLink(content, title, description, image,callback: ShareCallback);
    }

    //Shows only friends connected to app
    public void FBFriends()
    {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
         {
             Dictionary<string, object> dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
             List<object> friendsList = (List<object>)dictionary["data"];
             friendsName.Clear();
             foreach(Dictionary<string,object> dict in friendsList)
             {
                 friendsName.Add((string)dict["name"]);
             }
         });
    }

    public void FBUsername(System.Action action = null)
    {
        string query = "/me?fields=id,name";
        FB.API(query, HttpMethod.GET, result =>
        {
            if (result.Error == null)
            {
                id = (string)(result.ResultDictionary["id"]);
                fullname = (string)(result.ResultDictionary["name"]);
                if (action != null)
                {
                    action.Invoke();
                }
            }
            else
                Debug.LogError(result.Error);
        });
    }

    public void FBPicture()
    {
        string query = "/me/picture?type=square&height=200width=200";
        FB.API(query, HttpMethod.GET, result =>
        {

            if (result.Texture != null)
                picture = Sprite.Create(result.Texture, new Rect(0, 0, 200, 200), Vector2.zero);
            else
                Debug.LogError(result.Error);
        });
    }

    private void InitCallback()
    {
        this.Status = "Success - Check log for details";
        this.LastResponse = "Success Response: OnInitComplete Called\n";
        string logMessage = string.Format(
            "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
            FB.IsLoggedIn,
            FB.IsInitialized);

        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();

            if (AccessToken.CurrentAccessToken != null)
            {
                FBUsername();
                FBPicture();
                Debug.Log("already have access token");
            }
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }

        this.Status = "Success - Check log for details";
        this.LastResponse = string.Format("Success Response: OnHideUnity Called {0}\n", isGameShown);
    }

    private void AuthCallback(ILoginResult result)
    {
        if (result == null)
        {
            this.LastResponse = "Null Response\n";
            //LogView.AddLog(this.LastResponse);
            return;
        }

        //this.LastResponseTexture = null;

        // Some platforms return the empty string instead of null.
        if (!string.IsNullOrEmpty(result.Error))
        {
            this.Status = "Error - Check log for details";
            this.LastResponse = "Error Response:\n" + result.Error;
        }
        else if (result.Cancelled)
        {
            this.Status = "Cancelled - Check log for details";
            this.LastResponse = "Cancelled Response:\n" + result.RawResult;
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            this.Status = "Success - Check log for details";
            this.LastResponse = "Success Response:\n" + result.RawResult;
        }
        else
        {
            this.LastResponse = "Empty Response\n";
        }

        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }

            FBPicture();
            FBUsername(CreateUser);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    

    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("ShareLink Error: " + result.Error);
        }
        else if (!string.IsNullOrEmpty(result.PostId))
        {
            // Print post identifier of the shared content
            Debug.Log(result.PostId);
        }
        else
        {
            // Share succeeded without postID
            Debug.Log("ShareLink success!");
        }
    }

    private void CreateUser()
    {
        GetComponent<ConnectionScript>().username = id;
        GetComponent<ConnectionScript>().password = "Facebook!"+id+fullname;
        UserData.UserDataManager.Instance.userData.userType = UserData.UserData.USERTYPE.FACEBOOK;

        StartCoroutine(GetComponent<ConnectionScript>().CheckSignIn(fullname, UserData.UserData.USERTYPE.FACEBOOK));
    }
}
