using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserData
{
    public class UserDataManager : MonoBehaviour
    {
        private static UserDataManager instance;
        public readonly UserData userData = new UserData();

        public static UserDataManager Instance { get { return instance; } }

        private string userDataPath = "";

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

        private IEnumerator SaveDataAsync()
        {
            LoadingManager.Instance.enableLoading();
            yield return StartCoroutine(userData.SaveSerialize(userDataPath));
            LoadingManager.Instance.disableLoading();
        }
    }
}
