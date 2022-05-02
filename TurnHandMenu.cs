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
        OpenMenu(menuopen);
    }

    void OpenMenu(bool menuopen)
    {
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
