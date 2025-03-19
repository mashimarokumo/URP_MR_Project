using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pie : MonoBehaviour
{
    public Transform pos;
    bool complete;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(pos.position, transform.position) < 0.1f)
        {
            transform.position = pos.position;
            transform.rotation = Quaternion.identity;
        }
    }
}
