using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class MoveController : MonoBehaviour
{
    Controller controller;
    float HandPalmPitch;
    float HandPalmYaw;
    float HandPamRoll;
    float HandWristRot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        controller = new Controller();
        Frame frame = controller.Frame();
        List<Hand> hands = frame.Hands;
        if(frame.Hands.Count>0)
        {
            Hand firstHand = hands[0];
        }
        HandPalmPitch = hands[0].PalmNormal.Pitch;
        HandPamRoll = hands[0].PalmNormal.Roll;
        HandPalmYaw = hands[0].PalmNormal.Yaw;

        HandWristRot = hands[0].WristPosition.Pitch;

        Debug.Log("Pitch: " + HandPalmPitch);
        Debug.Log("Roll: " + HandPamRoll);
        Debug.Log("Yaw: " + HandPalmYaw);

        if (HandPalmYaw > -2f && HandPalmYaw < 3.5f) 
        {
            transform.Translate(new Vector3(0, 0, 1 * Time.deltaTime));
        }else if (HandPalmYaw < -2.2f)
        {
            transform.Translate(new Vector3(0, 0, -1 * Time.deltaTime));
        }
    }
}
