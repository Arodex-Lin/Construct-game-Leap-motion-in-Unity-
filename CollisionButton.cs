using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionButton : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        switch(gameObject.name)
        {
            case "PlaceGround":
                print("PlaceGround");
                ButtonControl.SetPlaceGround();
                break;
            case "SelectGround":
                print("SelectGround");
                ButtonControl.SetSelectGround();
                break;
            case "SetArable":
                print("SetArable");
                ButtonControl.SetArable();
                break;
            case "SetPlant":
                ButtonControl.SetPlant();
                print("SetPlant");
                break;
        }
    }
}
