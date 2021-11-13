using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyPathing1 : MonoBehaviour
{
    [Header("Spawn System")]
    //Transform to hold the spawn point of the SmallEnemy
    public Transform seSpawnpoint;
    //We create a list to hold the spawnpoint variables and create our public GameObject variables to hold our spawnpoint GameObjects
    public List<Transform> _spawnpoints;
    public Transform sp1L;
    public Transform sp2L;
    public Transform sp3R;
    public Transform sp4R;

    [Header("Movement & Boundaries")]
    //Transform to hold the Small Enemy waypoint
    private Transform waypoint;
    //We create a bool to hold the value to determine whether we spawned on the left or the right
    bool lSpawn;
    //This is probably the wrong way to do this but I'm creating a bool to differentiate between first loop and second loop. This will enable us to make the spawn move up one spawn and down on the next to give the enemies a zig zag movement
    bool run1;
    //We set a float for speed
    public float speed = .5f;
    //We create a public BoxCollider variable to hold our left, right, top, bottom & base out of bounds boxes
    public BoxCollider2D lOutOfBounds;
    public BoxCollider2D rOutOfBounds;
    public BoxCollider2D topOutOfBounds;
    public BoxCollider2D groundOutOfBounds;
    public BoxCollider2D baseOutOfBounds;
    //This bool is to test whether the SmallEnemy is in the top or ground out of bounds regions and act accordingly
    bool tOutOfBounds = false;
    bool gOutOfBounds = false;
    //We create two variables to track the time from the onset of spawn to 3 seconds later.
    //This allows us to conditionally spawn our SmallEnemy out of bounds while keeping it moving in the correct direction.
    private float timeToMove = 3.0f;
    public float timeFromSpawn = 0.0f;
    //Here we create the variable that will hold our segment prefab
    public Transform waypointPrefab;



    // Start is called before the first frame update
    private void Start()
    {
        //We declare the list of spawnpoints and assign the GameObjects to the list
        _spawnpoints = new List<Transform>();
        AddToList(sp1L, sp2L, sp3R, sp4R);

        //We call our functionRandomizePosition and make the position of this gameobject equal to that of seSPawnpoint.position
        RandomizePosition();
        this.transform.position = seSpawnpoint.position;
        //Here we instantiate our waypoint before calling the RandomizeEnemyWaypoint function
        waypoint = Instantiate(waypointPrefab);
        RandomizeEnemyWaypoint();

        //We create an if statement to tell us whether lSpawn is true or false
        if (seSpawnpoint == sp1L || seSpawnpoint == sp2L)
        {
            lSpawn = true;
        }
        else
        {
            lSpawn = false;
        }
        //Debug.Log($"seSpawnpoint = ${seSpawnpoint} while lSpawn = ${lSpawn}");
    }

    // Update is called once per frame
    private void Update()
    {
        //Here we make the waypoint spawn elsewhere when it overlaps with the SmallEnemy gameobject
        if (transform.position == waypoint.position)
        {
            //We call the randomizeEnemyWaypoint function
            RandomizeEnemyWaypoint();
        }

        //Here we tell the system to run the RandomizeEnemyWaypoint function again until it is not in the baseOutOfBounds region
        if (baseOutOfBounds.OverlapPoint(waypoint.position))
        {
            RandomizeEnemyWaypoint();
        }

        //Here we check to see whether our  left out of bounds grid area contains the waypoints position and if the timeFromSpawn exceeds timeToMove
        //Change the value of lSpawn to keep the SmallEnemy in the play field of view if both conditions are true
        if (lOutOfBounds.OverlapPoint(waypoint.position) && timeFromSpawn >= timeToMove)
        {
            lSpawn = true;
        }

        //Here we check to see whether our right out of bounds grid area contains the waypoints position and if the timeFromSpawn exceeds timeToMove
        //Change the value of lSpawn to keep the SmallEnemy in the play field of view if both conditions are true
        if (rOutOfBounds.OverlapPoint(waypoint.position) && timeFromSpawn >= timeToMove)
        {
            lSpawn = false;
        }

        //Here we test to see whether the waypoints position is out of bounds on top
        if (topOutOfBounds.OverlapPoint(waypoint.position))
        {
            tOutOfBounds = true;
        }
        else
        {
            tOutOfBounds = false;
        }

        //Here we test to see whether the waypoints position is out of bounds on the ground
        if (groundOutOfBounds.OverlapPoint(waypoint.position))
        {
            gOutOfBounds = true;
        }
        else
        {
            gOutOfBounds = false;
        }
    }

    private void FixedUpdate()
    {
        //We assign timeFromSpawn to Time.fixeddeltatime
        timeFromSpawn += Time.deltaTime;

        MoveToEWaypoint();
    }

    void AddToList(params Transform[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            _spawnpoints.Add(list[i]);
        }
    }

    //This function helps up to randomize the Small Enemy Spawn point
    public void RandomizePosition()
    {
        if (_spawnpoints != null)
        {
            //We declare a random number between 0 & 3
            int randNumber = Random.Range(0, 3);

            //Then we use that # to tell our SmallEnemy which spawnpoint it should spawn on.
            seSpawnpoint = _spawnpoints[randNumber];
        }
    }

    //Bool list to test all our variables and determine where the SE should head next
    private bool ShouldMoveLeftFI()
    {
        return lSpawn == false && run1 == true && tOutOfBounds == false && gOutOfBounds == false;
    }

    private bool ShouldMoveLeftSI()
    {
        return lSpawn == false && run1 == false && tOutOfBounds == false && gOutOfBounds == false;
    }

    private bool ShouldMoveRightFI()
    {
        return lSpawn == true && run1 == true && tOutOfBounds == false && gOutOfBounds == false;
    }

    private bool ShouldMoveRightSI()
    {
        return lSpawn == true && run1 == false && tOutOfBounds == false && gOutOfBounds == false;
    }

    private bool ShouldMoveUpRight()
    {
        return lSpawn == true && gOutOfBounds == true;
    }

    private bool ShouldMoveDownRight()
    {
        return lSpawn == true && tOutOfBounds == true;
    }

    private bool ShouldMoveUpLeft()
    {
        return lSpawn == false && gOutOfBounds == true;
    }

    private bool ShouldMoveDownLeft()
    {
        return lSpawn == false && tOutOfBounds == true;
    }


    //We use this function to randomize the enemy waypoint... Go figure...
    void RandomizeEnemyWaypoint()
    {
        //Here we create fioats and use them to hold random values that allow us to change the spawn of the waypoints next location
        float x = Random.Range(0.25f, .75f);
        float y = Random.Range(0.25f, .75f);
        int z = 0;


        //Here we check the criteria for where we want to spawn the waypoints position
        if (ShouldMoveUpRight())
        {
            waypoint.position = transform.TransformPoint(-x, y, z);
            return;
        }

        if (ShouldMoveDownRight())
        {
            waypoint.position = transform.TransformPoint(x, y, z);
            return;
        }

        if (ShouldMoveUpLeft())
        {
            waypoint.position = transform.TransformPoint(-x, -y, z);
            return;
        }

        if (ShouldMoveDownLeft())
        {
            waypoint.position = transform.TransformPoint(x, -y, z);
            return;
        }

        if (ShouldMoveLeftFI())
        {
            waypoint.position = transform.TransformPoint(x, -y, z);
            run1 = false;
            return;
        }

        if (ShouldMoveLeftSI())
        {
            waypoint.position = transform.TransformPoint(-x, -y, z);
            run1 = true;
            return;
        }

        if (ShouldMoveRightFI())
        {
            waypoint.position = transform.TransformPoint(x, y, z);
            run1 = false;
            return;
        }

        if (ShouldMoveRightSI())
        {
            waypoint.position = transform.TransformPoint(-x, y, z);
            run1 = true;
            return;
        }
    }

    //Simple script telling our SE where to go
    void MoveToEWaypoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoint.position, speed * Time.deltaTime);
    }
}
