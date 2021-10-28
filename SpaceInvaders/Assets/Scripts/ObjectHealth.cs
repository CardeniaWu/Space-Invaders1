using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    [Header("Health")]
    //We are going to create the variables to set up our health system.
    [SerializeField]
    private float maxHealth = 500.0f;
    private float currentHealth;
    //This variable allows us to store the health bar for this game object
    [SerializeField]
    private HealthSystem healthbar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        healthbar.SetHealth(currentHealth);

        Debug.Log($"This {this} took {damage} damage and has {currentHealth} remaining.");
    }
}
