using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

public class lline : MonoBehaviour
{

    LeapProvider provider;
    private const float rotate_sensitive = 1500f;  //旋转灵敏度
    private const float displacement_sensitive = 0.015f; //位移灵敏度
    private const float rotate_initial_value = 0f;  //旋转初始位置值

    

    LineRenderer line;
    bool isLineOpen = false;
    bool Once = true;
    bool isRightOnce = true;
    RaycastHit hit;
    /// <summary>
    /// 判断条件  尽量勿动
    /// </summary>
    const float smallestVelocity = 0.1f;
    //const float deltaVelocity = 0.000001f;
    const float deltaVelocity = 0.5f;
    const float deltaCloseFinger = 0.06f;

    void Start()
    {
        
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        line = gameObject.GetComponent<LineRenderer>();
        //this.line = this.gameObject.GetComponent<LineRenderer>();
        //line.SetVertexCount(2);
    }

    void Update()
    {
        line.SetWidth(0.01f, 0.01f);
        //line.SetColors(Color.yellow, Color.red);
        //line.SetColors(Color.green, Color.green);
        Frame frame = provider.CurrentFrame;
        foreach (var hand in frame.Hands)
        {
            if (hand.IsLeft)
            {
                //Debug.Log("左手");
            }
            if (hand.IsRight)
            {
                //Debug.Log(hand.PalmVelocity.x);
                //Debug.Log("右手");
                if (this.isMoveRight(hand) && this.Once)
                    //if (isMoveRight(hand))
                {
                    Debug.Log("线开启");
                    this.Once = false;
                    StartCoroutine(this.rightMoveOnce());

                    this.isLineOpen = true;
                }
                if (this.isMoveLeft(hand) && this.Once)
                //if(isMoveLeft(hand))
                {
                    Debug.Log("线关闭");
                    this.Once = false;
                    StartCoroutine(this.rightMoveOnce());
                    this.isLineOpen = false;
                }
                if (isLineOpen)
                {
                    Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);
                    bool rayCast = Physics.Raycast(ray, out hit, 5000);
                    line.SetPosition(0, (new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z)));
                    line.SetPosition(1, hand.Direction.ToVector3().normalized * 100000);
                    if (rayCast)
                    {
                        //Debug.Log(hit.transform.name);
                        //hit.transform.Rotate(Vector3.up, 2.0f);
                    }
                }
                else
                {
                    this.gameObject.GetComponent<LineRenderer>().SetWidth(0.000000001f, 0.000000001f);
                    print(1);
                }


            }
        }
        //Scale();
        //Rotation();
        // Position();

    }
    
    IEnumerator rightMoveOnce()
    {
        yield return new WaitForSeconds(1.0f);
        this.Once = true;
    }

    /// <summary>
    /// 缩小
    /// </summary>
    public void Scale()
    {
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {

            if (isOpenFullHand(hand))
            {
                Debug.Log("大");

                Vector3 value = transform.localScale;
                value += new Vector3(value.x * 0.01f, value.y * 0.01f, value.z * 0.01f);
                //    Debug.Log(value);
                transform.localScale = value;

            }
            if (isCloseHand(hand))
            {
                Debug.Log("小");
                Vector3 value = transform.localScale;
                value -= new Vector3(value.x * 0.01f, value.y * 0.01f, value.z * 0.01f);
                //   Debug.Log(value);

                transform.localScale = value;

            }
        }
    }


    /// <summary>
    /// 旋转
    /// </summary>
    public void Rotation()
    {
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsLeft || hand.IsRight)
            {
                Vector3 value = transform.localEulerAngles;
                value = new Vector3(hand.PalmPosition.y * rotate_sensitive + rotate_initial_value, hand.PalmPosition.x * rotate_sensitive + rotate_initial_value, 0);
                transform.localEulerAngles = value;
            }
            else
            {
                hand.PalmPosition.y = transform.localEulerAngles.x;
                hand.PalmPosition.x = transform.localEulerAngles.y;
            }
        }
    }


    public void Position()
    {
        Frame frame = provider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            if (hand.IsLeft || hand.IsRight)
            {
                if (isMoveLeft(hand))
                {
                    transform.localPosition = new Vector3(hand.PalmPosition.y * displacement_sensitive, 0, 0) + transform.localPosition;
                }
                if (isMoveRight(hand))
                {
                    transform.localPosition = new Vector3(-hand.PalmPosition.y * displacement_sensitive, 0, 0) + transform.localPosition;
                }
            }
        }

    }


    protected bool isMoveRight(Hand hand)// 手划向右边
    {

        return hand.PalmVelocity.x > deltaVelocity && !isStationary(hand);
    }


    protected bool isMoveLeft(Hand hand)   // 手划向左边
    {
        //x轴移动的速度   deltaVelocity = 0.7f    isStationary (hand)  判断hand是否禁止 
        return hand.PalmVelocity.x < -deltaVelocity && !isStationary(hand);
    }

    protected bool isStationary(Hand hand)// 固定不动的 
    {
        return hand.PalmVelocity.Magnitude < smallestVelocity;
    }

    protected bool isCloseHand(Hand hand)     //是否握拳 
    {
        List<Finger> listOfFingers = hand.Fingers;
        int count = 0;
        for (int f = 0; f < listOfFingers.Count; f++)
        { //循环遍历所有的手~~
            Finger finger = listOfFingers[f];
            if ((finger.TipPosition - hand.PalmPosition).Magnitude < deltaCloseFinger)    // Magnitude  向量的长度 。是(x*x+y*y+z*z)的平方根。    //float deltaCloseFinger = 0.05f;
            {
                count++;
                //  if (finger.Type == Finger.FingerType.TYPE_THUMB)
                //  Debug.Log ((finger.TipPosition - hand.PalmPosition).Magnitude);
            }
        }
        return (count == 5);
    }

    protected bool isOpenFullHand(Hand hand)         //手掌全展开~
    {
        //Debug.Log (hand.GrabStrength + " " + hand.PalmVelocity + " " + hand.PalmVelocity.Magnitude);
        return hand.GrabStrength == 0;
    }



}
