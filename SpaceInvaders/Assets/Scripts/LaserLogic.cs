using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLogic : MonoBehaviour
{
    [Header("Damage Calculations")]
    //We create a variable to hold our field of impact.
    [SerializeField]
    private float fieldOfImpact = 0.0f;
    //We create a variable to hold our layermask to tell it what layers to effect.
    public LayerMask layerToHit;
    //We create a variable to hold the damage value of enemy lasers.
    [SerializeField]
    private float damage = 100.0f;
    //We create a variable to hold our positional data of where the collision occurred
    [SerializeField]
    private Transform collisionLocation;
    //We create a variable to hold our positional date of where we want the explosion to occur
    [SerializeField]
    private Transform explosionLocation;

    [Header("Visual Aethetics")]
    //Here we store our laser animation
    private Animator laserAnim;
    //We create a variable to hold our explosion effect prefab
    [SerializeField]
    private GameObject explosionEffect;

    [Header("Debug Assist")]
    [SerializeField]
    //Here we create a bool to store a value to determine whether the laser fieldOfImpact gizmo is on.
    private bool isGizmoOn;


    /*
     * 1. Let anim play
     * 2. Spawn in explosion when the anim switches to "LaserFire"
     * 3. Gather list of objects effected
     * 4. Calculate dmg dealt
     * 5. Apply dmg
     * 
     * 
    */

    // Start is called before the first frame update
    void Start()
    {
        laserAnim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        if (laserAnim.GetCurrentAnimatorStateInfo(0).IsName("LaserFire"))
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        //We collect all the collider2D objects in a list based on whether they were in the field of impact and had the correct layer to be effected
        Collider2D[] objects = Physics2D.OverlapCircleAll(collisionLocation.position, fieldOfImpact, layerToHit);

        //We iterate through the list of colliders effected and calculate a dmg multiplier then apply dmg as necessary
        foreach (Collider2D obj in objects)
        {
            float distanceFromObj = Vector3.Distance(obj.transform.position, collisionLocation.position);
            
            float dmgMultiplier = 1 / distanceFromObj;

            float dmgToPlayer = damage * dmgMultiplier;
                
            if (dmgToPlayer > 100)
            {
                dmgToPlayer = 100;
            }

            if (obj.gameObject.GetComponent<ObjectHealth>() != null)
            {
                obj.gameObject.GetComponent<ObjectHealth>().TakeDamage(dmgToPlayer);
            }
            else
            {
                Debug.Log($"ObjectHealth script not found for {obj}.");
            }
        }

        //Here we instantiate an explosion effect, have it last for half a second, then destroy both the explosion and the missile game objects
        GameObject ExplosionEffectIns = Instantiate(explosionEffect, explosionLocation.position, Quaternion.identity);
        Destroy(ExplosionEffectIns, .75f);
        Destroy(gameObject);
    }

    //We use this function to visually track the field of impact from our lasers
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (isGizmoOn)
        {
            Gizmos.DrawWireSphere(collisionLocation.position, fieldOfImpact);
        }
    }
}
