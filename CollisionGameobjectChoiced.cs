using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGameobjectChoiced : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlaceGround.ground = gameObject;
        print(collision.gameObject.name);
        if(collision.gameObject.name != "Plane")
        {
            PlaceGround.toDisappear = true;
        }
    }
}
