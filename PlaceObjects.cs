using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;

public class PlaceObjects : MonoBehaviour
{
    LeapProvider provider;
    public HandModelBase leftHandModel;
    public HandModelBase rightHandModel;
    public GameObject building1;
    private const float displacement_sensitive = 0.015f;
    const float smallestVelocity = 0.1f, deltaVelocity = 0.5f;
    bool Once = true;
    bool rotateOnce = true;
    bool createAnotherOne = true;
    public LayerMask LayerMask;
    bool isPlacing = false;
    bool isRotating = false;
    RaycastHit hit;
    GameObject build;
    GameObject swapBuild;
    // Start is called before the first frame update
    void Start()
    {
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    // Update is called once per frame
    void Update()
    {
        if(createAnotherOne)
        {
            build = Instantiate(building1, new Vector3(0, -10, 0), Quaternion.identity);
        }//已经放置成功，则让build为新的复制体
        createAnotherOne = false;//不需要重置build
        Frame frame = provider.CurrentFrame;
        foreach(var hand in frame.Hands)
        {
            if(hand.IsRight)
            {
                if (isMoveRight(hand)&&Once)
                {
                    Once = false;
                    StartCoroutine(moveOnce());//冷却1秒
                    isPlacing = true;
                }//右手向右移动，进入放置状态
                if(isMoveLeft(hand)&&Once)
                {
                    Once = false;
                    StartCoroutine(moveOnce());
                    isPlacing = false;
                    Destroy(build);
                    createAnotherOne = true;
                }//向左移动，销毁当前物品，退出放置状态，重置build
                if (isPlacing)
                {
                    Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);
                    bool rayCast = Physics.Raycast(ray, out hit, 5000, LayerMask);
                    print(hit.point);
                    build.transform.position = hit.point;
                }//获取手指方向，发射射线，放置物品
                
            }
            if(hand.IsLeft)
            {
                if (isMoveLeft(hand) && rotateOnce && isRotating)
                {
                    rotateOnce = false;
                    StartCoroutine(moveRotateOnce());
                    swapBuild.transform.Rotate(new Vector3(0, 45, 0));
                }//旋转物体
                if (isMoveRight(hand) && rotateOnce && isRotating)
                {
                    rotateOnce = false;
                    StartCoroutine(moveRotateOnce());
                    swapBuild.transform.Rotate(new Vector3(0, -45, 0));
                }//旋转
                if (isMoveDown(hand) && Once)
                {
                    Once = false;
                    isPlacing = false;
                    createAnotherOne = true;
                    StartCoroutine(moveOnce());
                    swapBuild = build;
                    isRotating = !isRotating;
                }//左手向下，放置完成,或已经是放置状态,进入旋转状态,或已经旋转状态,结束
            }
            
        }
    }
    IEnumerator moveOnce()
    {
        yield return new WaitForSeconds(1f);
        Once = true;
    }//延时1秒
    IEnumerator moveRotateOnce()
    {
        yield return new WaitForSeconds(0.5f);
        rotateOnce = true;
    }//旋转延时低一点
    protected bool isMoveRight(Hand hand)
    {
        return hand.PalmVelocity.x > deltaVelocity && !isStationary(hand);
    }
    protected bool isMoveLeft(Hand hand)
    {
        return hand.PalmVelocity.x < -deltaVelocity && !isStationary(hand);
    }
    protected bool isMoveDown(Hand hand)
    {
        return hand.PalmVelocity.y < -deltaVelocity && !isStationary(hand);
    }

    protected bool isStationary(Hand hand)
    {
        return hand.PalmVelocity.Magnitude < smallestVelocity;
    }

}
