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
            }//延时1秒
        IEnumerator moveRotateOnce()
        {
            yield return new WaitForSeconds(0.5f);
            rotateOnce = true;
        }//旋转延时低一点
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
        public static float getDistance(Vector3 transform1, Vector3 transform2)//返回欧式距离
        {
            return Mathf.Sqrt(Mathf.Pow((transform1.x - transform2.x), 2) + Mathf.Pow((transform1.z - transform2.z), 2));
        }
        public static bool isPalmNormalSameDirectionWith (Hand hand, Vector3 dir)
        {
      //判断手的掌心方向于一个  向量   是否方向相同 
            return isSameDirection (hand.PalmNormal, UnityVectorExtension.ToVector (dir));
        }  
        public static bool isSameDirection (Vector a, Vector b)
        {
            //判断两个向量是否 相同 方向
            //Debug.Log (angle2LeapVectors (a, b) + " " + b);
            return angle2LeapVectors (a, b) < handForwardDegree;
        }
        public static float angle2LeapVectors (Leap.Vector a, Leap.Vector  b)
        {
           //向量转化成 角度
            return Vector3.Angle (UnityVectorExtension.ToVector3 (a), UnityVectorExtension.ToVector3 (b));
        }


    

    }


}