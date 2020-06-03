using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookLogin : MonoBehaviour
{
    string Status;
    string LastResponse;
    List<string> friendsName = new List<string>();
    string username;
    Sprite picture;

    void Awake()
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

    public void FBLogin()
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.AuthCallback);
    }

    public void FBInvite(string message, string title)
    {
        FB.AppRequest(message, title: title);
    }

    public void FBShare(System.Uri content, string title, string description, System.Uri image)
    {
        FB.ShareLink(content, title, description, image,callback: ShareCallback);
    }

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

    public void FBUsername()
    {
        string query = "/me?fields=first_name";
        FB.API(query, HttpMethod.GET, result =>
        {
            if (result.Error == null)
                username = (string)(result.ResultDictionary["first_name"]);
            else
                Debug.LogError(result.Error);
        });
    }

    public void FBPicture()
    {
        string query = "/me/picture?type=square&height=128&width=128";
        FB.API(query, HttpMethod.GET, result =>
        {

            if (result.Texture != null)
                picture = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), Vector2.zero);
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
            // Continue with Facebook SDK

            if (AccessToken.CurrentAccessToken != null)
            {
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
}
