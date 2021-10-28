using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : MonoBehaviour
{
    public Transform turretBarrelRight;
    public Transform turretBarrelLeft;
    
    /*
    [Header("Turret System")]
    public float range;
    private Vector2 turretDirection;
    private bool up = true;
    */

    /*
     * Plans for the Enemy AI pathing
     * ***All this is done in a class containing this enemies specific logic for every newly instantiated enemy*** 
     * 1. Store SmallEnemy as a prefab on the GameHandler script and use the level count to choose how many to instantiate at the start of a round
     * 2. Create a naming convention for all new enemies that are instantiated **don't think this is necessary**
     * 3. Create a list to hold our SmallEnemy variables as they're instantiated
     * 4. Use the current levelCount from GameHandler to determine how many SmallEnemies need to be spawned
     * 5. Create spawnpoints to spawn our enemies ***completed***
     * 6. Create a function to randomize the spawn point of the enemies ***completed***
     * 7. Create a waypoint GameObject for each SmallEnemy to path to ***completed***
     * 8. Create a function that generates a randomized value within certain constraints to spawn the next waypoint after it ***completed***
     * 9. Generate a new randomized waypoint location when the ship reaches it's intended destination (use an if statement to test whether the SmallEnemy position overlays that of the waypoint GameObject) ***completed***
     * 
     * 
     * Turret functionality:
     * 1. Create rotation limits
     * 2. Create script to direct the logic of rotations
     * 3. Create logic to direct the turret to rotate and up and down continuously
    */

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
