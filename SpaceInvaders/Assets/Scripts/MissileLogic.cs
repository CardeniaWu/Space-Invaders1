using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLogic : MonoBehaviour
{
    [Header("Movement")]
    //We set the move speed of the missile.
    public float moveSpeed = 20.0f;
    //We create a RigidBody2D variable to hold our rigidbody.
    [SerializeField]
    private Rigidbody2D rb2D;
    //We create a variable to hold our field of impact.
    [SerializeField]
    private float fieldOfImpact = 0.0f;
    //We create a variable to hold our layermask to tell it what layers to effect.
    [SerializeField]
    public LayerMask layerToHit;


    [Header("Explosions!!!")]
    //We create a variable to hold the damage value of enemy missiles.
    [SerializeField]
    private float damageToPlayer = 20.0f;
    //We create a variable to hold our explosion effect.
    [SerializeField]
    private GameObject explosionEffect;

    [Header("UI")]
    //Here we create a bool to store a value to determine whether the missile fieldOfImpact is on.
    [SerializeField]
    private bool isGizmoOn = true;


    // Start is called before the first frame update
    void Start()
    {
        rb2D.velocity = transform.right * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.tag == "PlayerBase" || obj.tag == "Ground")
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, layerToHit);

        foreach (Collider2D obj in objects)
        {
            float distanceFromObj = Vector3.Distance(obj.transform.position, this.transform.position);

            float dmgMultiplier = 1 / distanceFromObj; 
           
            if (obj.gameObject.GetComponent<ObjectHealth>() != null)
            {
                obj.gameObject.GetComponent<ObjectHealth>().TakeDamage(damageToPlayer * dmgMultiplier);
            } else
            {
                Debug.Log($"ObjectHealth script not found for {obj}.");
            }
        }

        GameObject ExplosionEffectIns = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(ExplosionEffectIns, .5f);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        if (isGizmoOn)
        {
            Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
        }
    }
}
