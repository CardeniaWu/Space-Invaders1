using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeEnemy : MonoBehaviour
{
    [Header("Movement")]
    //We create a variable to hold our desired waypoint
    [SerializeField]
    private Transform PBase;
    //We create a variable to hold our speed variable
    [SerializeField]
    private float speed;
    //We create a variable to hold a rigidbody2D
    private Rigidbody2D se_Rigidbody2D;
    //We create a variable to hold the collider that will serve as our barrier
    [SerializeField]
    private CircleCollider2D leBarrier;
    //We create a bool to tell us whether to move forward or not
    private bool shouldMove = true;
    //We create a Vector2 to hold our movement value
    private Vector2 movement;

    [Header("Spawn System")]
    //We create a list to hold the spawnpoint variables and create our public GameObject variables to hold our spawnpoint GameObjects
    [SerializeField]
    private List<Transform> _LEspawnpoints;
    [SerializeField]
    private Transform sp1;
    [SerializeField]
    private Transform sp2;
    [SerializeField]
    private Transform sp3;
    [SerializeField]
    private Transform sp4;
    //We create a transform variable to hold the value of the LE spawn point
    private Transform leSpawnpoint;

    
    [Header("Shooting System")]
    //We create a two variables to hold our bay door animators 
    [SerializeField]
    private Animator leBayDoorT;
    [SerializeField]
    private Animator leBayDoorB;
    //We create a variable to hold our fire animator which comes after the bay doors are open
    [SerializeField]
    private Animator LEFire;
    //We create a transform variable to hold our prefabbed laser
    [SerializeField]
    private Transform prefabLaser;
    //And one to hold the instantiated laser
    private Transform instantiatedLaser;
    //We create a transform variable to hold the position of the gun from which we will shoot the laser 
    [SerializeField]
    private Transform laserSpawn;
    //And a bool to tell us whether we should shoot the laser or not
    private bool shouldFire = true;
    


    [Header("Debug Assistance")]
    //We create a variable to allow us to turn debug assistance on or off
    [SerializeField]
    private bool debugAssist;
    [SerializeField]
    private bool turnOnGizmo;

    // Start is called before the first frame update
    void Start()
    {
        //We set our rigidbody variable, create our spawn in list and populate it with spawn points, randomize our start location and set the initial position to our randomized spawn
        se_Rigidbody2D = GetComponent<Rigidbody2D>();
        _LEspawnpoints = new List<Transform>();
        AddToList(sp1, sp2, sp3, sp4);
        RandomizePosition();

        this.transform.position = leSpawnpoint.position;
    }

    void Update()
    {
        //We check to see whether we should continue moving. If we are close enough to the base, we stop and initiate our bay door anims
        if (leBarrier.bounds.Contains(this.transform.position))
        {
            shouldMove = false;
            leBayDoorT.SetBool("InRange", true);
            leBayDoorB.SetBool("InRange", true);
        }

        //Once the bay doors are in firing position, we heat up our laser gun
        if (leBayDoorB.GetCurrentAnimatorStateInfo(0).IsName("LEBDBFiring"))
        {
            LEFire.SetBool("FireReady", true);
        }

        //Once the laser gun is heated, we fire our laser once
        if (LEFire.GetCurrentAnimatorStateInfo(0).IsName("LEBOOM") && shouldFire)
        {
            FireLaser();
            shouldFire = false;
        }

        //Once the laser gun fires once and returns to heating up, we return our shouldFire variable to true to fire once again
        if (LEFire.GetCurrentAnimatorStateInfo(0).IsName("LEFire"))
        {
            shouldFire = true;
        }
    }
    
    void FixedUpdate()
    {
        //We check if shouldMove is true or false and call our function MoveToLEWaypoint if it is true
        if (shouldMove)
        {
            MoveToLEWaypoint();
        }

        //We calculate our movement value
        movement = this.transform.position - PBase.position;

        //If we are moving, we align our movement with the direction we are heading
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            angle = angle - 180.0f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        //Debug assistance message that tells us the target rotation in euler angles
        if (debugAssist)
        {
            Debug.Log($"LE target rotation is {this.transform.eulerAngles}");
        }
    }

    //Here we tell our ship to move towards the player base
    void MoveToLEWaypoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, PBase.position, speed * Time.deltaTime);
    }

    //We initialize our spawn point by randamizing between four initial spawn points.
    public void RandomizePosition()
    {
        if (_LEspawnpoints != null)
        {
            //We declare a random number between 0 & 3
            int randNumber = Random.Range(0, 3);

            //Then we use that # to tell our SmallEnemy which spawnpoint it should spawn on.
            leSpawnpoint = _LEspawnpoints[randNumber];
        }
    }

    //Pew Pew
    void FireLaser()
    {
        //Convert the quaternion to euler angles, then modify the Z to get the roration we want
        Vector3 myEulerAngles = this.transform.rotation.eulerAngles;
        Quaternion laserRotation = Quaternion.Euler(myEulerAngles.x, myEulerAngles.y, myEulerAngles.z - 90);

        //Instantiate the laser
        instantiatedLaser = Instantiate(prefabLaser, laserSpawn.position, laserRotation);
    }

    //Nifty AddToList function
    void AddToList(params Transform[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            _LEspawnpoints.Add(list[i]);
        }
    }

    // Here we draw Gizmo rays from the turrets to enable us to see the prospective missile trajectory
    private void OnDrawGizmos()
    {
        if (turnOnGizmo)
        {
            Gizmos.color = Color.red;
            Vector3 direction = transform.TransformDirection(Vector3.right) * 25;
            Gizmos.DrawRay(transform.position, direction);
        }
    }
}
