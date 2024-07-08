using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseDoor : MonoBehaviour
{
    public PressurePlate plate1;
    public PressurePlate plate2;

    void Update()
    {
        if (plate1.isActivated && plate2.isActivated)
        {
            Destroy(gameObject);
        }
    }
}

