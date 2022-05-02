namespace HandGuesterNameSpace
{
    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
    using Leap.Unity;

    public class HandGestures : MonoBehaviour
    {

        


        private const float displacement_sensitive = 0.015f;
        const float smallestVelocity = 0.1f, deltaVelocity = 0.5f;
        public static bool Once = true;
        static float handForwardDegree = 30;
        bool rotateOnce = true;

       

        
            public IEnumerator moveOnce()
            {
                yield return new WaitForSeconds(1f);
                Once = true;
            }
        IEnumerator moveRotateOnce()
        {
            yield return new WaitForSeconds(0.5f);
            rotateOnce = true;
        }
        public static bool isMoveRight(Hand hand)
        {
            return hand.PalmVelocity.x > deltaVelocity && !isStationary(hand);
        }
        public static bool isMoveLeft(Hand hand)
        {
            return hand.PalmVelocity.x < -deltaVelocity && !isStationary(hand);
        }
        public static bool isMoveDown(Hand hand)
        {
            return hand.PalmVelocity.y < -deltaVelocity && !isStationary(hand);
        }

        public static bool isStationary(Hand hand)
        {
            return hand.PalmVelocity.Magnitude < smallestVelocity;
        }
        public static float getDistance(Vector3 transform1, Vector3 transform2)
        {
            return Mathf.Sqrt(Mathf.Pow((transform1.x - transform2.x), 2) + Mathf.Pow((transform1.z - transform2.z), 2));
        }
        public static bool isPalmNormalSameDirectionWith (Hand hand, Vector3 dir)
        {
            return isSameDirection (hand.PalmNormal, UnityVectorExtension.ToVector (dir));
        }  
        public static bool isSameDirection (Vector a, Vector b)
        {
            //Debug.Log (angle2LeapVectors (a, b) + " " + b);
            return angle2LeapVectors (a, b) < handForwardDegree;
        }
        public static float angle2LeapVectors (Leap.Vector a, Leap.Vector  b)
        {
            return Vector3.Angle (UnityVectorExtension.ToVector3 (a), UnityVectorExtension.ToVector3 (b));
        }


    

    }


}