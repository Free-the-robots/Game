using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
#endif

public class GooglePlayService : MonoBehaviour
{
    public GameObject googlePlayButton;
    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_ANDROID
        Open();
#else
        enabled = false;
        if (googlePlayButton != null)
            googlePlayButton.SetActive(false);
#endif
    }

    public void SignIn()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, SignInWithGoogle);
#endif
    }

#if UNITY_ANDROID
    private void Open()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // enables saving game progress.
        .EnableSavedGames()
        // registers a callback to handle game invitations received while the game is not running.
        //.WithInvitationDelegate(InvitationReceivedDelegate)
        // registers a callback for turn based match notifications received while the
        // game is not running.
        //.WithMatchDelegate(MatchReceivedDelegate)
        // requests the email address of the player be available.
        // Will bring up a prompt for consent.
        .RequestEmail()
        // requests a server auth code be generated so it can be passed to an
        //  associated back end server application and exchanged for an OAuth token.
        .RequestServerAuthCode(false)
        // requests an ID token be generated.  This OAuth token can be used to
        //  identify the player to other services such as Firebase.
        .RequestIdToken()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

    public void SignInSilent()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce,SignInWithGoogle);
    }

    private void InvitationReceivedDelegate(GooglePlayGames.BasicApi.Multiplayer.Invitation invitation, bool autoAccept)
    {

    }
    private void MatchReceivedDelegate(GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch match, bool autoAccept)
    {

    }

    private void SignInWithGoogle(SignInStatus status)
    {
        if(status == SignInStatus.Success)
        {
            if (Social.localUser.authenticated)
            {
                string user = Social.localUser.userName;
                string id = Social.localUser.id;

                //avatar
                //Social.localUser.image
            }
        }
        else
        {
            Debug.LogError("Failed Sign in : " + status);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
#endif
}
