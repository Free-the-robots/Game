﻿using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserData
{
    public class UserDataManager : MonoBehaviour
    {
        private static UserDataManager instance;
        public readonly UserData userData = new UserData();
        public sqlUserData userAuth = null;

        public static UserDataManager Instance { get { return instance; } }

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
                        Debug.LogError(e.Message);
                        Debug.LogError("Recreating intial user data");
                        userData.CreateInitial();
                        SaveData();
                    }
                }
                else
                {
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
    }
}
