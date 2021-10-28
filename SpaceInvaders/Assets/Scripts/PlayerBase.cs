using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBase : MonoBehaviour
{
    [Header("Energy Systems")]
    //We create a list to hold the generator game objects
    private List<Transform> _generators;
    //We create a variable to store our Power Level value
    public GameObject pwrLvlText;
    //We create a variable to store our generator count
    private int generatorCount;
    //We create two variables to hold our initial generators
    public Transform generator;
    public Transform generator1;
    public float energyTime = 0.0f;
    public float energyTick = 0.0f;
    public int pwrLvl = 0;

    // Start is called before the first frame update
    void Start()
    {
        //We create the _generators list
        _generators = new List<Transform>();
        //And add in the two generator gameObjects
        _generators.Add(generator);
        _generators.Add(generator1);
    }

    // Update is called once per frame
    void Update()
    {
        pwrLvlText.GetComponent<TextMeshProUGUI>().text = pwrLvl.ToString();
    }

    private void FixedUpdate()
    {
        energyTime += Time.fixedDeltaTime;
        
        //We create a while loop to continually produce energy whenever the generators are alive
        if (_generators.Count > 0)
        {
            energyTick = (energyTime * (50 * _generators.Count));
            int energyTickInt = (int)energyTick;
            pwrLvl = energyTickInt;
        }
    }
}
