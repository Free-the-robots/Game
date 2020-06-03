using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserData
{
    [RequireComponent(typeof(ConnectionScript))]
    [RequireComponent(typeof(FacebookLogin))]
    public class UserDataManager : MonoBehaviour
    {
        public readonly UserData userData = new UserData();
        public sqlUserData userAuth = null;

        private static UserDataManager instance;
        public static UserDataManager Instance { get { return instance; } }

        public CanvasGroupFade fader = null;

        private string userDataPath = "";
        private string userDataPath2 = "";

        private void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this.gameObject);
            else
            {
                instance = this;

                userDataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + ".udata.d";
                if (File.Exists(userDataPath))
                {
                    try
                    {
                        userData.LoadSerialize(userDataPath);
                    }
                    catch (System.Exception e)
                    {
                        //TODO : REMOVE FOR PRODUCTION
                        Debug.LogError(e.Message);
                        Debug.LogError("Recreating intial user data");
                        userData.CreateInitial();
                        SaveData();
                    }
                }
                else
                {
                    //TODO : REMOVE FOR PRODUCTION
                    userData.CreateInitial();
                    SaveData();
                }

                userDataPath2 = Application.persistentDataPath + Path.DirectorySeparatorChar + ".udata2.dat";
                if (File.Exists(userDataPath2))
                {
                    StartCoroutine(checkUser());
                }
            }

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            if (fader != null)
            {
                fader.enable();
            }
        }

        public void AddWeapon(Projectiles.ProjectileData projectile)
        {
            WeaponData weapon = new WeaponData();
            weapon.id = (ushort)projectile.id;
            userData.weapons.Add(weapon);
        }

        public void AddEvoWeapon(Projectiles.ProjectileEvolutiveData projectile)
        {
            WeaponData weapon = new WeaponData();
            weapon.id = (ushort)projectile.id;
            userData.weapons.Add(weapon);
        }

        public void AddShip(SpaceshipData shipData)
        {
            ShipData ship = new ShipData();
            ship.id = (ushort)shipData.id;
            userData.ships.Add(ship);
        }

        public void SaveData()
        {
            StartCoroutine(SaveDataAsync());
        }

        public void LoadData()
        {
            StartCoroutine(LoadDataAsync());
        }

        private IEnumerator SaveDataAsync()
        {
            LoadingManager.Instance.enableLoading();
            yield return StartCoroutine(userData.SaveSerialize(userDataPath));
            if(GetComponent<ConnectionScript>().loggedin)
                yield return StartCoroutine(GetComponent<ConnectionScript>().updateLog(userAuth.id, EncryptDecrypt.encrypt(userData.Serialize())));
            LoadingManager.Instance.disableLoading();
        }

        private IEnumerator LoadDataAsync()
        {
            LoadingManager.Instance.enableLoading();
            if (GetComponent<ConnectionScript>().loggedin)
                yield return StartCoroutine(GetComponent<ConnectionScript>().getLog(userAuth.username));
            LoadingManager.Instance.disableLoading();
        }

        private IEnumerator checkUser()
        {
            string udata = Encoding.Default.GetString(EncryptDecrypt.LoadDecryptFile(userDataPath2));
            string[] data = udata.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            yield return StartCoroutine(GetComponent<ConnectionScript>().authenticateLog(data[1], data[2]));
        }

        public void Share(string title, string description, Texture2D image)
        {
            string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
            File.WriteAllBytes(filePath, image.EncodeToPNG());
            Share(title, description, filePath);
        }

        public void Share(string title, string description, string imageFileName)
        {
            new NativeShare().AddFile(imageFileName).SetTitle(title).SetText(description).Share();
        }

        public void Share(string title, string description)
        {
            new NativeShare().SetTitle(title).SetText(description).Share();
        }

        public void ShareSocialMedia(System.Uri content, string title, string description, System.Uri image)
        {
            if(userData.userType == UserData.USERTYPE.FACEBOOK)
            {
                GetComponent<FacebookLogin>().FBShare(content, title, description, image);
            }
        }
    }
}
