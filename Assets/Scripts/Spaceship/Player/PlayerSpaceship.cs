using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpaceship : Spaceship
{
    private Plane plane;

    public List<Weapon.Abilities> abilities = new List<Weapon.Abilities>();

    /// <summary>
    /// Movement script from POC
    /// </summary>
    [SerializeField]
    private FloatingJoystick JoystickMoving = null;
    private Vector2 mInformationFromJoystickMoving;

    [SerializeField]
    private FloatingJoystick JoystickRotate = null;
    private Vector2 mInformationFromJoystickRotate;

    [SerializeField]
    private GameObject CharacterToMove = null;

    private Vector3 mRotation;
    //private float mRotationSpeed;

    private float mAngleHeadingChar;

    private Vector3 mNormalizedDirection;

    //public GameEvent playerHealthUpdate;
    //public GameEvent hitEvent;
    //public GameEventInt changeWeapon;
    //public GameEvent newWeaponEvent;

    // Start is called before the first frame update
    protected override void Setup()
    {
        plane = new Plane(Vector3.up, Vector3.zero);
        abilities.Add(new Weapon.CircleDeath());
        if(JoystickRotate != null)
            JoystickRotate.OnActiveJoystick += Rotate;
        else{
            JoystickRotate = GameObject.Find("RotateJoystick").GetComponent<FloatingJoystick>();
            JoystickMoving = GameObject.Find("MoveJoystick").GetComponent<FloatingJoystick>();
            JoystickRotate.OnActiveJoystick += Rotate;
        }
        //mRotationSpeed = 20F;
    }


    //private float t = 0f;
    // Update is called once per frame
    protected override void Behaviour()
    {
        //t += Time.deltaTime;

        //if (Input.GetMouseButton(0))
        //{
        //    if (t > 1F / spaceshipData.freq)
        //    {
        //        for(int i = 0; i < weapon.Count(); ++i)
        //        {
        //            weapon[i].Fire();
        //        }
        //        t = 0f;
        //    }
        //}

        //if (Input.GetMouseButton(1))
        //{
        //    if(abilities.Count > 0)
        //    {
        //        abilities[0].RunAbility(transform);
        //    }
        //}
        if (JoystickMoving != null && JoystickMoving.IsJoystickActive)
        {
            MoveCharacter();
        }
        else
        {
            StopMoving();
        }
        if (JoystickRotate != null && JoystickRotate.IsJoystickActive)
        {
            for (int i = 0; i < weapon.Count(); ++i)
            {
                if(weapon[i].projectileData is Projectiles.ProjectileGuidedData)
                {
                    ((Projectiles.ProjectileGuidedData)weapon[i].projectileData).target = GameObject.FindGameObjectWithTag("Enemy").transform;
                }
                if(weapon[i] is Weapon.TurretGuided)
                {
                    ((Weapon.TurretGuided)weapon[i]).target = GameObject.FindGameObjectWithTag("Enemy").transform;
                }
                weapon[i].UpdateRotation();
                weapon[i].Fire();
            }
        }


    }

    protected override void FixedBehaviour()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float distance;
        //if (plane.Raycast(ray, out distance))
        //{
        //    // some point of the plane was hit - get its coordinates
        //    Vector3 hitPoint = ray.GetPoint(distance);
        //    // use the hitPoint to aim your cannon
        //    Vector3 forward = (hitPoint - transform.position).normalized;
        //    forward.Scale(new Vector3(1f, 0f, 1f));
        //    transform.forward = forward.normalized;
        //    //transform.position = Vector3.MoveTowards(player.transform.position, hitPoint, Time.deltaTime * spaceshipData.speed);
        //}

        //Vector3 dir = new Vector3(-Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));
        //rbody.velocity = dir*spaceshipData.speed;
    }

    public override void loseHealth(int health)
    {
        spaceshipData.life -= health;

        if (spaceshipData.life <= 0)
            spaceshipData.life = 0;

        //playerHealthUpdate.Raise();
    }

    public void addHealth(int health)
    {
        spaceshipData.life += health;

        if (spaceshipData.life > spaceshipData.lifeMax)
            spaceshipData.life = spaceshipData.lifeMax;

        //playerHealthUpdate.Raise();
    }

    public void newWeapon(NEAT.Person weaponC)
    {

        //newWeaponEvent.Raise();
    }

    private void Rotate()
    {
        mAngleHeadingChar = Mathf.Atan2(JoystickRotate.Direction.x, JoystickRotate.Direction.y) - Mathf.PI / 2f;
        if(CharacterToMove != null)
            CharacterToMove.transform.rotation = Quaternion.Euler(0F, mAngleHeadingChar * Mathf.Rad2Deg, 0F);
    }

    private void MoveCharacter()
    {
        mNormalizedDirection.x = -JoystickMoving.Direction.y;
        mNormalizedDirection.y = 0F;
        mNormalizedDirection.z = JoystickMoving.Direction.x;
        //CharacterToMove.transform.position += mNormalizedDirection * mMaxSpeed * Time.deltaTime;
        rbody.velocity = mNormalizedDirection * spaceshipData.speed * Time.deltaTime;
    }

    private void StopMoving()
    {
        //mNormalizedDirection.x = 0F;
        //mNormalizedDirection.y = 0F;
        //mNormalizedDirection.z = 0F;
        //CharacterToMove.transform.position += mNormalizedDirection * mMaxSpeed * Time.deltaTime;
        rbody.velocity = Vector3.zero;
    }

    public void updateModifiableTurrets()
    {
        UserData.UserData userData = UserData.UserDataManager.Instance.userData;
        AssetDataManager assetDataManager = AssetDataManager.Instance;

        UserData.ShipData actualData= userData.ships.Find(i => i.id == spaceshipData.id);
        int turretCount = GetComponentsInChildren<Weapon.Turret>().Length;
        int modTurretsCount = userData.ships.Find(i => i.id == spaceshipData.id).turrets.Count;

        if (modTurretsCount > 0)
        {
            for (int i = 0; i < modTurretsCount; ++i)
            {
                if(actualData.turrets[i] > -1)
                {
                    GameObject turret = GameObject.Instantiate(assetDataManager.turretObject[userData.weapons[actualData.turrets[i]].id]);
                    turret.transform.SetParent(transform.GetChild(turretCount + i));
                    turret.transform.localPosition = new Vector3(0f, 0f, 0f);
                }
            }
        }
    }

    public void clearModifiableTurrets()
    {
        int turretCount = GetComponentsInChildren<Weapon.Turret>().Length;
        UserData.UserData userData = UserData.UserDataManager.Instance.userData;
        int modTurretsCount = userData.ships.Find(i => i.id == spaceshipData.id).turrets.Count;
        for (int i = 0; i < modTurretsCount; ++i)
        {
            GameObject.Destroy(transform.GetChild(turretCount + i).GetChild(0).gameObject);
        }
    }
}