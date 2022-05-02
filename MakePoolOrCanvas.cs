using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class MakePoolOrCanvas : MonoBehaviour
{
    LeapProvider provider;
    public HandModelBase leftHandModel;
    public HandModelBase rightHandModel;

    private const float displacement_sensitive = 0.015f;
    protected float handForwardDegree = 30;
    const float smallestVelocity = 0.1f, deltaVelocity = 0.5f;

    bool Once = true;
    bool createAnotherOne = true;
    bool placePool = false;
    bool isPointing = false;
    bool suring = false;

    string defaultMaterialName = "Default-Material (Instance)";
    public LayerMask LayerMask;
    
    public GameObject prefabPool;
    int count;
    public Transform startPos, endPos;
    RaycastHit startHit, endHit;
    public Material poolMaterial;
    RaycastHit hit;
    void Start()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().material = poolMaterial;
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //print(startHit.point);
        Frame frame = provider.CurrentFrame;
        switch (count)
        {
            case 0:
                foreach (var hand in frame.Hands)
                {
                    if (hand.IsLeft)
                    {
                        if(isPalmNormalSameDirectionWith(hand,new Vector3(0,1,0)))
                        {
                            print("yes");
                        }
                        if(isMoveDown(hand)&&Once)
                        {
                            Once = false;
                            StartCoroutine(moveOnce());
                            suring = true;
                            print("起始点确认"+Time.time);
                        }//左手向下，确定起始点
                    }

                    if (hand.IsRight)
                    {
                        if (isMoveRight(hand) && Once)
                        {
                            Once = false;
                            StartCoroutine(moveOnce());
                            isPointing = true;

                        }//右手向右，开始选定起始点。
                        if(isPointing)
                        {
                            Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);
                            bool raycast = Physics.Raycast(ray, out hit, 5000, LayerMask);
                            if(suring&&raycast)
                            {
                                suring = false; 
                                startHit = hit;
                                count++;//存起始点，进入下一状态
                            }
                        }//确定起始点
                    }
                }
                break;
            case 1:

                foreach (var hand in frame.Hands)
                {
                    if(hand.IsRight)
                    {
                        if(isPointing)
                        {
                            Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);
                            bool raycast = Physics.Raycast(ray, out hit, 5000, LayerMask);
                            if(raycast)
                            {
                                endHit = hit;
                            }//动态确定终止点
                            
                        }
                    }
                    if(hand.IsLeft)
                    {
                        if(isMoveDown(hand)&&Once)
                        {
                            StartCoroutine(moveOnce());
                            count++;//进入下一状态
                            placePool = true;
                        }
                    }
                }
                if (endHit.point.x > startHit.point.x && startHit.point.z > endHit.point.z)
                {
                    DrawSquare(startHit.point, startHit.point + new Vector3(Mathf.Abs(startHit.point.x - endHit.point.x), 0, 0), endHit.point - new Vector3(Mathf.Abs(endHit.point.x - startHit.point.x), 0, 0), endHit.point);
                    //终止点在起始点右下
                }
                if (endHit.point.x > startHit.point.x && endHit.point.z > startHit.point.z)
                {
                    DrawSquare(endHit.point - new Vector3(Mathf.Abs(startHit.point.x - endHit.point.x), 0, 0), endHit.point, startHit.point, startHit.point + new Vector3(Mathf.Abs(startHit.point.x - endHit.point.x), 0, 0));
                    //终止点在起始点右上
                }
                if (startHit.point.x > endHit.point.x && startHit.point.z > endHit.point.z)
                {
                    DrawSquare(startHit.point - new Vector3(Mathf.Abs(startHit.point.x - endHit.point.x), 0, 0), startHit.point, endHit.point, endHit.point + new Vector3(Mathf.Abs(startHit.point.x - endHit.point.x), 0, 0));
                    //终止点在起始点左下
                }
                if (startHit.point.x > endHit.point.x && endHit.point.z > startHit.point.z)
                {
                    DrawSquare(endHit.point, endHit.point + new Vector3(Mathf.Abs(startHit.point.x - endHit.point.x), 0, 0), startHit.point - new Vector3(Mathf.Abs(startHit.point.x - endHit.point.x), 0, 0), startHit.point);
                    //终止点在起始点左上
                }
                break;
            case 2:
                if(placePool)
                {
                    placePool = false;
                    GameObject newpool = Instantiate(prefabPool, new Vector3((startHit.point.x+endHit.point.x)/2, 0.01f, (startHit.point.z+endHit.point.z)/2), Quaternion.identity);
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    newpool.transform.localScale = new Vector3(Mathf.Abs(startHit.point.x - endHit.point.x), 0.0001f, Mathf.Abs(startHit.point.z - endHit.point.z));
                    //print(1);
                    startHit = new RaycastHit();
                    suring = false;
                    isPointing = false;
                    StartCoroutine(countToZero());
                }
                break;
        }
    }
    void DrawSquare(Vector3 pointOne,Vector3 pointTwo,Vector3 pointThree,Vector3 pointFour)
    {
        
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        pointOne = pointOne + new Vector3(0, 0.01f, 0);
        pointTwo = pointTwo + new Vector3(0, 0.01f, 0);
        pointThree = pointThree + new Vector3(0, 0.01f, 0);
        pointFour = pointFour + new Vector3(0, 0.01f, 0);
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = new Vector3[] { pointOne, pointTwo, pointThree, pointFour };
        mesh.triangles = new int[]
        {
            0,1,2,
            1,3,2
        };
    }

    IEnumerator moveOnce()
    {
        yield return new WaitForSeconds(1f);
        Once = true;
    }//延时1秒
    IEnumerator countToZero()
    {
        yield return new WaitForSeconds(1f);
        count = 0;
    }
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
    protected float angle2LeapVectors(Leap.Vector a, Leap.Vector b)
    {
        return Vector3.Angle(UnityVectorExtension.ToVector3(a), UnityVectorExtension.ToVector3(b));
    }
    protected bool isSameDirection(Vector a, Vector b)
    {
        return angle2LeapVectors(a, b) < handForwardDegree;
        //Debug.Log (angle2LeapVectors (a, b) + " " + b);
    }
    protected bool isPalmNormalSameDirectionWith(Hand hand, Vector3 dir)
    {
        return isSameDirection(hand.PalmNormal, UnityVectorExtension.ToVector(dir));
    }
}
