using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS || UNITY_STANDALONE_OSX
using UnityEngine.SignInWithApple;
#endif

public class SignInAppleObject : MonoBehaviour
{
    public void Awake()
    {
#if UNITY_IOS || UNITY_STANDALONE_OSX
        gameObject.AddComponent<SignInWithApple>();
        Open();
#else
        gameObject.SetActive(false);
#endif
    }

#if UNITY_IOS || UNITY_STANDALONE_OSX
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ButtonPress()
    {
        var siwa = gameObject.GetComponent<SignInWithApple>();
        siwa.Login(OnLogin);
    }

    public void CredentialButton()
    {
        // User id that was obtained from the user signed into your app for the first time.
        var siwa = gameObject.GetComponent<SignInWithApple>();
        siwa.GetCredentialState("<userid>", OnCredentialState);
    }

    private void OnCredentialState(SignInWithApple.CallbackArgs args)
    {
        Debug.Log("User credential state is: " + args.credentialState);
    }

    private void OnLogin(SignInWithApple.CallbackArgs args)
    {
        Debug.Log("Sign in with Apple login has completed.");
    }
#endif
}
