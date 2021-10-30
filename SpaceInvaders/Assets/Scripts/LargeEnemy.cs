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
    //We create a Vector3 to hold our movement value
    private Vector3 diff;

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
    //We create a variable to hold an initial rotational value
    private Quaternion startRotation;
    //We create a variable to determine whether the LE spawned on the left or the right
    private bool lSpawn;

    [Header("Shooting System")]
    [SerializeField]
    private Animator leBayDoorT;
    [SerializeField]
    private Animator leBayDoorB;


    [Header("Debug Assistance")]
    //We create a variable to allow us to turn debug assistance on or off
    [SerializeField]
    private bool debugAssist;

    // Start is called before the first frame update
    void Start()
    {
        se_Rigidbody2D = GetComponent<Rigidbody2D>();
        _LEspawnpoints = new List<Transform>();
        AddToList(sp1, sp2, sp3, sp4);
        RandomizePosition();

        this.transform.position = leSpawnpoint.position;

        if (this.transform.position == sp1.position || this.transform.position == sp2.position)
        {
            lSpawn = true;
        }
    }

    void Update()
    {
        if (leBarrier.bounds.Contains(this.transform.position))
        {
            shouldMove = false;
            leBayDoorT.SetBool("InRange", true);
            leBayDoorB.SetBool("InRange", true);
        }
    }
    
    void FixedUpdate()
    {
        //We check if shouldMove is true or false and call our function MoveToLEWaypoint if it is true
        if (shouldMove)
        {
            MoveToLEWaypoint();
        }

        diff = this.transform.position - PBase.position;
        diff.Normalize();
        float rotZ = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;

        if (lSpawn)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, rotZ);

        } else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, rotZ - 180);
        }
  
        /*
        //We create a variable to hold our movement vector
        movement = this.transform.position - PBase.position;

        //We create a new Quaternion named targetRotation and use the movement to set its LookRotation
        Quaternion targetRotation = Quaternion.LookRotation(movement);

        //We modify the new Quaternion to set a rotate towards value
        targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.fixedDeltaTime);

        //We use MoveRotation to adjust our rotation based on the values formulated by targetRotation
        se_Rigidbody2D.MoveRotation(targetRotation);
        */

        if (debugAssist)
        {
            Debug.Log($"LE target rotation is {this.transform.rotation.z}");
        }
    }

    void MoveToLEWaypoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, PBase.position, speed * Time.deltaTime);
    }

    public void RandomizePosition()
    {
        if (_LEspawnpoints != null)
        {
            //We declare a random number between 0 & 3
            int randNumber = Random.Range(0, 3);

            //Then we use that # to tell our SmallEnemy which spawnpoint it should spawn on.
            leSpawnpoint = _LEspawnpoints[0];
        }
    }

    void AddToList(params Transform[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            _LEspawnpoints.Add(list[i]);
        }
    }
}
