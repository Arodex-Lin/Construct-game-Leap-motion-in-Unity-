using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using HandGuesterNameSpace;
using UnityEngine.UI;

public class TurnHandMenu : MonoBehaviour
{
    LeapProvider LeapProvider;
    public GameObject ButtonPlaceGround , ButtonSelectGround, ButtonSetArable, ButtonSetPlant;
    Vector3 offset1 = new Vector3(0,0,0.2f);
    Vector3 offset2 = new Vector3(0.2f,0,0.2f);
    Vector3 offset3 = new Vector3(0.4f,0,0.2f);
    Vector3 offset4 = new Vector3(0.6f,0,0.2f);
    bool menuopen = false;
    void Start()
    {
         LeapProvider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = LeapProvider.CurrentFrame;
        menuopen = GetMenuOpen(frame);
        OpenMenu(menuopen,frame);
    }

    void OpenMenu(bool menuopen, Frame frame)
    {
        Vector3 palmPos = new Vector3 (0,0,0);
        if (menuopen)
        {
            foreach(var hand in frame.Hands)
            {
                if (hand.IsLeft)
                {
                    palmPos = UnityVectorExtension.ToVector3(hand.PalmPosition);
                }
            }
            ButtonPlaceGround.transform.position = palmPos + offset1;
            ButtonSelectGround.transform.position = palmPos + offset2;
            ButtonSetArable.transform.position = palmPos + offset3;
            ButtonSetPlant.transform.position = palmPos + offset4;
            //ButtonPlaceGround.transform.Rotate(-80, 0, 180);
        }
    }
    

    bool GetMenuOpen(Frame frame)
    {
        foreach(var hand in frame.Hands)
        {
            if(hand.IsLeft)
            {
                if (HandGestures.isPalmNormalSameDirectionWith(hand, new Vector3(0, 1, 0)))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
