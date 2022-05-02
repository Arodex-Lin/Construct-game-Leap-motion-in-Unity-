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
        }//�Ѿ����óɹ�������buildΪ�µĸ�����
        createAnotherOne = false;//����Ҫ����build
        Frame frame = provider.CurrentFrame;
        foreach(var hand in frame.Hands)
        {
            if(hand.IsRight)
            {
                if (isMoveRight(hand)&&Once)
                {
                    Once = false;
                    StartCoroutine(moveOnce());//��ȴ1��
                    isPlacing = true;
                }//���������ƶ����������״̬
                if(isMoveLeft(hand)&&Once)
                {
                    Once = false;
                    StartCoroutine(moveOnce());
                    isPlacing = false;
                    Destroy(build);
                    createAnotherOne = true;
                }//�����ƶ������ٵ�ǰ��Ʒ���˳�����״̬������build
                if (isPlacing)
                {
                    Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);
                    bool rayCast = Physics.Raycast(ray, out hit, 5000, LayerMask);
                    print(hit.point);
                    build.transform.position = hit.point;
                }//��ȡ��ָ���򣬷������ߣ�������Ʒ
                
            }
            if(hand.IsLeft)
            {
                if (isMoveLeft(hand) && rotateOnce && isRotating)
                {
                    rotateOnce = false;
                    StartCoroutine(moveRotateOnce());
                    swapBuild.transform.Rotate(new Vector3(0, 45, 0));
                }//��ת����
                if (isMoveRight(hand) && rotateOnce && isRotating)
                {
                    rotateOnce = false;
                    StartCoroutine(moveRotateOnce());
                    swapBuild.transform.Rotate(new Vector3(0, -45, 0));
                }//��ת
                if (isMoveDown(hand) && Once)
                {
                    Once = false;
                    isPlacing = false;
                    createAnotherOne = true;
                    StartCoroutine(moveOnce());
                    swapBuild = build;
                    isRotating = !isRotating;
                }//�������£��������,���Ѿ��Ƿ���״̬,������ת״̬,���Ѿ���ת״̬,����
            }
            
        }
    }
    IEnumerator moveOnce()
    {
        yield return new WaitForSeconds(1f);
        Once = true;
    }//��ʱ1��
    IEnumerator moveRotateOnce()
    {
        yield return new WaitForSeconds(0.5f);
        rotateOnce = true;
    }//��ת��ʱ��һ��
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
