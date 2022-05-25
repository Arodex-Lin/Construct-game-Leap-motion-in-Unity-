using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGameobjectChoiced : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.name != "Plane")
        if(collision.gameObject.transform.root.gameObject.tag == "Hand")
        {
            print(1);
            PlaceGround.ground = gameObject;
            PlaceGround.toDisappear = true;
        }
        //print(PlaceGround.ground);
    }
}
