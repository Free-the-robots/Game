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
                        userData.SaveSerialize(userDataPath);
                    }
                }
                else
                {
                    userData.CreateInitial();
                    userData.SaveSerialize(userDataPath);
                }

            }

            DontDestroyOnLoad(this);
        }
        // Start is called before the first frame update
        void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {

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
    }
}
