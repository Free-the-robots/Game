using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS || UNITY_STANDALONE_OSX
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using AppleAuth.Extensions;
#endif

public class SignInAppleObject : MonoBehaviour
{
#if UNITY_IOS || UNITY_STANDALONE_OSX
    private IAppleAuthManager appleAuthManager;
    public bool credentialOK = false;
#endif
    public GameObject appleButton;
    void Awake()
    {
#if UNITY_IOS || UNITY_STANDALONE_OSX
        Open();
#else
        enabled = false;
        if(appleButton != null)
            appleButton.SetActive(false);
#endif
    }

    public void SignIn()
    {
#if UNITY_IOS || UNITY_STANDALONE_OSX
        this.SignInWithApple();
#endif
    }

#if UNITY_IOS || UNITY_STANDALONE_OSX

    void Update()
    {
        if (this.appleAuthManager != null)
        {
            this.appleAuthManager.Update();
        }
    }

    private void Open()
    {
        if (appleButton != null)
            appleButton.SetActive(true);
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            this.appleAuthManager = new AppleAuthManager(deserializer);
            //AttemptQuickLogin();
        }
    }
    public IEnumerator CheckCredentialStatusForUserId(string appleUserId)
    {
        bool credentialGot = false;
        // If there is an apple ID available, we should check the credential state
        this.appleAuthManager.GetCredentialState(
            appleUserId,
            state =>
            {
                switch (state)
                {
                    // If it's authorized, login with that user id
                    case CredentialState.Authorized:
                        credentialOK = true;
                        return;

                    // If it was revoked, or not found, we need a new sign in with apple attempt
                    // Discard previous apple user id
                    case CredentialState.Revoked:
                    case CredentialState.NotFound:

                        return;
                }
                credentialGot = true;
            },
            error =>
            {
                Debug.LogWarning("Error while trying to get credential state " + error.ToString());
                credentialGot = true;
            });
        while (!credentialGot)
        {
            yield return null;
        }
    }

    private void AttemptQuickLogin()
    {
        var quickLoginArgs = new AppleAuthQuickLoginArgs();

        // Quick login should succeed if the credential was authorized before and not revoked
        this.appleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>
            {
                // If it's an Apple credential, save the user ID, for later logins
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    Debug.Log(credential.User);
                }
            },
            error =>
            {
                // If Quick Login fails, we should show the normal sign in with apple menu, to allow for a normal Sign In with apple
                Debug.LogWarning("Quick Login Failed " + error.ToString());
            });
    }

    private void SignInWithApple()
    {
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);
        this.appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                // If a sign in with apple succeeds, we should have obtained the credential with the user id, name, and email, save it

                IAppleIDCredential appleIdCredential = credential as IAppleIDCredential;
                IPasswordCredential passwordCredential = credential as IPasswordCredential;

                GetComponent<ConnectionScript>().username = credential.User;;
                GetComponent<ConnectionScript>().password = "Apple!"+credential.User+appleIdCredential.FullName.ToLocalizedString();
                UserData.UserDataManager.Instance.userData.userType = UserData.UserData.USERTYPE.APPLE;
                StartCoroutine(GetComponent<ConnectionScript>().CheckSignIn(appleIdCredential.FullName.ToLocalizedString()));

                //Debug.Log(appleIdCredential.Email);
            },
            error =>
            {
                Debug.LogWarning("Sign in with Apple failed " + error.ToString());
            });
    }
#endif
}
