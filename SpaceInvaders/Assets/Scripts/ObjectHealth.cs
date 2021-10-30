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
    [SerializeField]
    private GameObject hBarGameobject;

    [Header("Timer")]
    //We set our wait time to 5 seconds
    [SerializeField]
    private float waitTime = 5.0f;
    [SerializeField]
    private float elapsedTime;

    [Header("Debug Assistance")]
    [SerializeField]
    private bool debugAssist;

    // Start is called before the first frame update
    void Start()
    {
        //We set current health to the maximum allowable health for this object
        currentHealth = maxHealth;
        //We set the healthbar to max health
        healthbar.SetMaxHealth(maxHealth);
        //We set the healthbar to inactive
        hBarGameobject.SetActive(false);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= waitTime)
        {
            hBarGameobject.SetActive(false);
        }    
    }

    //Function that deals damage
    public void TakeDamage(float damage)
    {
        //Subtract damage value from currentHealth
        currentHealth -= damage;

        //Set the healthbar to the current health value
        healthbar.SetHealth(currentHealth);

        //Debugging assistance
        if (debugAssist)
        {
            Debug.Log($"This {this} took {damage} damage and has {currentHealth} remaining.");
        }

        hBarGameobject.SetActive(true);
        elapsedTime = 0.0f;
    }
}
