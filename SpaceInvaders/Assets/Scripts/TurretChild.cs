using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretChild : MonoBehaviour
{
    [SerializeField]
    private ObjectHealth parentScript;
    
    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.parent.GetComponent<ObjectHealth>();
    }
}
