using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionButton : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)//检测目前碰撞的按钮
    {
        switch(gameObject.name)//把按钮的名字设置成不同功能的名字
        {
            case "PlaceGround"://当前碰撞的按钮名字是PlaceGround
                print("PlaceGround");//方便检测当前状态，输出
                ButtonControl.SetPlaceGround();//调用按钮设置的函数（ButtonControl脚本里）
                break;
            case "SelectGround"://下面同理
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
