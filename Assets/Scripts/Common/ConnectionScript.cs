using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class sqlUserData
{
    public int id;
    public string username;
    public string password;
    public string last;
    public byte[] data;
}

public class ConnectionScript : MonoBehaviour
{
    public string username { get; set; }
    public string password { get; set; }

    public bool loggedin = false;
    public string level = "Main";

    public void LogIn()
    {
        StartCoroutine(authenticateLog(username, password));
    }

    public void SingIn()
    {
        StartCoroutine(createLog(username, password));
    }

    public IEnumerator authenticateLog(string user, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", user);
        form.AddField("password", pass);

        using (UnityWebRequest www = UnityWebRequest.Post("https://free-the-robots.com/api/usersauth", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                if (!www.downloadHandler.text.Contains("Error"))
                {
                    UserData.UserDataManager userDataMan = UserData.UserDataManager.Instance;
                    UserData.UserDataManager.Instance.userAuth = JsonUtility.FromJson<sqlUserData>(www.downloadHandler.text);
                    loggedin = true;
                    yield return StartCoroutine(EncryptDecrypt.StoreEncryptFile(
                        Application.persistentDataPath + Path.DirectorySeparatorChar + ".udata2.dat",
                        userDataMan.userAuth.id + "\n" + userDataMan.userAuth.username + "\n" + pass));
                    if (SceneManager.GetActiveScene().name.Equals("Login"))
                        SceneManager.LoadScene(level);
                }
                else
                {
                    Debug.LogError("Error authenticating");
                }
            }
        }
        yield return null;
    }

    public IEnumerator createLog(string user, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", user);
        form.AddField("password", pass);

        using (UnityWebRequest www = UnityWebRequest.Post("https://free-the-robots.com/api/users", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                if (!www.downloadHandler.text.Contains("Error"))
                {
                    UserData.UserDataManager userDataMan = UserData.UserDataManager.Instance;
                    userDataMan.userAuth = JsonUtility.FromJson<sqlUserData>(www.downloadHandler.text);
                    loggedin = true;
                    yield return StartCoroutine(EncryptDecrypt.StoreEncryptFile(
                        Application.persistentDataPath + Path.DirectorySeparatorChar + ".udata2.dat",
                        userDataMan.userAuth.id + "\n" + userDataMan.userAuth.username + "\n" + pass));

                    UserData.UserDataManager.Instance.userData.CreateInitial();
                    UserData.UserDataManager.Instance.SaveData();

                    if (SceneManager.GetActiveScene().name.Equals("Login"))
                        SceneManager.LoadScene(level);
                }
                else
                {
                    Debug.LogError("Error creating user");
                }
            }
        }
        yield return null;
    }

    public IEnumerator getLog(string user)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://free-the-robots.com/api/users/"+username))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                if (!www.downloadHandler.text.Contains("Error"))
                {
                    UserData.UserDataManager.Instance.userAuth = JsonUtility.FromJson<sqlUserData>(www.downloadHandler.text);
                }
                else
                {
                    Debug.LogError("Error creating user");
                }
            }
        }
        yield return null;
    }

    public IEnumerator updateLog(int id, byte[] data)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", UserData.UserDataManager.Instance.userAuth.username);
        form.AddField("password", UserData.UserDataManager.Instance.userAuth.password);
        form.AddBinaryData("data", data);

        using (UnityWebRequest www = UnityWebRequest.Post("https://free-the-robots.com/api/users/" + id, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //Debug.Log(www.downloadHandler.text);
                if (!www.downloadHandler.text.Contains("Error"))
                {

                }
                else
                {
                    Debug.LogError("Error creating user");
                }
            }
        }
        yield return null;
    }
}
