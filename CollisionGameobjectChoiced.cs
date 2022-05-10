using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGameobjectChoiced : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        //if(collision.gameObject.name != "Plane")
        if(collision.gameObject.transform.root.gameObject.tag == "Hand")
        {
            PlaceGround.ground = gameObject;
            PlaceGround.toDisappear = true;
        }
        //print(PlaceGround.ground);
    }
}
