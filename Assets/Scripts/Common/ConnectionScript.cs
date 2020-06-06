using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class sqlUserData
{
    public int id;
    public string username;
    public string password;
    public string last;
    public string data;
}

public class ConnectionScript : MonoBehaviour
{
    public string username { get; set; }
    public string password { get; set; }

    public bool loggedin = false;
    public string level = "Main";

    public Text debugMessage;

    public void LogIn()
    {
        StartCoroutine(authenticateLog(username, password));
    }

    public void SingIn()
    {
        if (username.Length > 3 && password.Length > 3)
            StartCoroutine(createLog(username, password, username));
        else
        {
            Debug.LogError("username and password not long enough");
            if (debugMessage != null)
                debugMessage.text = "Username or Password not long enough. You need at least 3 characters.";
        }
    }

    public void SingIn(string name)
    {
        StartCoroutine(createLog(username, password, name));
    }

    public IEnumerator CheckSignIn()
    {
        yield return StartCoroutine(authenticateLog(username, password));
        if (!loggedin)
            yield return StartCoroutine(createLog(username, password));
    }

    public IEnumerator CheckSignIn(string name)
    {
        yield return StartCoroutine(authenticateLog(username, password));
        if (!loggedin)
        {
            yield return StartCoroutine(createLog(username, password, name));
        }
    }

    public IEnumerator authenticateLog(string user, string pass)
    {
        debugMessage.text = "Authenticating...";

        WWWForm form = new WWWForm();
        form.AddField("username", user);
        form.AddField("password", pass);

        using (UnityWebRequest www = UnityWebRequest.Post("https://free-the-robots.com/api/usersauth", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (debugMessage != null)
                    debugMessage.text = www.error;
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                if (!www.downloadHandler.text.Contains("Error"))
                {
                    UserData.UserDataManager userDataMan = UserData.UserDataManager.Instance;
                    userDataMan.userAuth = JsonUtility.FromJson<sqlUserData>(www.downloadHandler.text);
                    loggedin = true;
                    if (userDataMan.userAuth.data.Length > 0)
                    {
                        userDataMan.userAuth = JsonUtility.FromJson<sqlUserData>(www.downloadHandler.text);
                        byte[] data = EncryptDecrypt.decrypt(System.Text.Encoding.Default.GetBytes(userDataMan.userAuth.data));
                        userDataMan.userData.LoadSerialize(data);
                    }
                    else
                        Debug.LogError("User Data NULL");
                    yield return StartCoroutine(EncryptDecrypt.StoreEncryptFile(
                        Application.persistentDataPath + Path.DirectorySeparatorChar + ".udata2.dat",
                        userDataMan.userAuth.id + "\n" + userDataMan.userAuth.username + "\n" + pass));
                    if (SceneManager.GetActiveScene().name.Equals("Login"))
                        SceneManager.LoadScene(level);
                }
                else
                {
                    Debug.LogError("Error authenticating");
                    if (debugMessage != null)
                        debugMessage.text = "Error authenticating";
                }
            }
        }
        yield return null;
    }

    public IEnumerator createLog(string user, string pass, string name = null)
    {
        debugMessage.text = "Creating account...";

        WWWForm form = new WWWForm();
        form.AddField("username", user);
        form.AddField("password", pass);

        using (UnityWebRequest www = UnityWebRequest.Post("https://free-the-robots.com/api/users", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (debugMessage != null)
                    debugMessage.text = www.error;
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                if (!www.downloadHandler.text.Contains("Error"))
                {
                    UserData.UserDataManager userDataMan = UserData.UserDataManager.Instance;
                    userDataMan.userAuth = JsonUtility.FromJson<sqlUserData>(www.downloadHandler.text);
                    loggedin = true;
                    if (name == null)
                        name = "User" + userDataMan.userAuth.id;
                    yield return StartCoroutine(EncryptDecrypt.StoreEncryptFile(
                        Application.persistentDataPath + Path.DirectorySeparatorChar + ".udata2.dat",
                        userDataMan.userAuth.id + "\n" + userDataMan.userAuth.username + "\n" + pass + "\n" + name));

                    userDataMan.userData.CreateInitial();
                    yield return StartCoroutine(userDataMan.SaveDataAsync());

                    if (SceneManager.GetActiveScene().name.Equals("Login"))
                        SceneManager.LoadScene(level);
                }
                else
                {
                    Debug.LogError("Error creating User");
                    if (debugMessage != null)
                        debugMessage.text = "Error creating User";
                }
            }
        }
        yield return null;
    }

    public IEnumerator getLog(string user)
    {
        debugMessage.text = "Getting account...";

        using (UnityWebRequest www = UnityWebRequest.Get("https://free-the-robots.com/api/users/"+username))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (debugMessage != null)
                    debugMessage.text = www.error;
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                if (!www.downloadHandler.text.Contains("Error"))
                {
                    UserData.UserDataManager userMan = UserData.UserDataManager.Instance;
                    userMan.userAuth = JsonUtility.FromJson<sqlUserData>(www.downloadHandler.text);
                    byte[] data = EncryptDecrypt.decrypt(System.Text.Encoding.Default.GetBytes(userMan.userAuth.data));
                    userMan.userData.LoadSerialize(data);
                }
                else
                {
                    Debug.LogError("Error getting User");
                    if (debugMessage != null)
                        debugMessage.text = "Error getting User";
                }
            }
        }
        yield return null;
    }

    public IEnumerator updateLog(int id, byte[] data)
    {
        debugMessage.text = "Updating account...";
        WWWForm form = new WWWForm();
        form.AddField("username", UserData.UserDataManager.Instance.userAuth.username);
        form.AddField("password", UserData.UserDataManager.Instance.userAuth.password);
        form.AddField("data", System.Text.Encoding.Default.GetString(data));

        using (UnityWebRequest www = UnityWebRequest.Post("https://free-the-robots.com/api/users/" + id, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                if (debugMessage != null)
                    debugMessage.text = www.error;
            }
            else
            {
                if (!www.downloadHandler.text.Contains("Error"))
                {
                    Debug.Log(www.downloadHandler.text);
                }
                else
                {
                    Debug.LogError("Error updating User");
                    if (debugMessage != null)
                        debugMessage.text = "Error updating User";
                }
            }
        }
        yield return null;
    }
}
