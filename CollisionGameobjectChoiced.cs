using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionGameobjectChoiced : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //bool flag = true;
        //foreach (var obj in PlaceGround.ground)
        //{
        //    if(obj.gameObject.name == gameObject.name)
        //    {
        //        flag = false;
        //    }
        //}
        //if (flag)
        //{
        //    PlaceGround.ground.Add(gameObject);
        //}
        PlaceGround.ground = gameObject;
    }
}
