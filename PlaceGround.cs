using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using HandGuesterNameSpace;
[SerializeField]
[Serializable]
public class PlaceGround : MonoBehaviour
{
    LeapProvider provider;

    //public static List<GameObject> ground = new List<GameObject>();
    public static GameObject ground;

    public List<GameObject> obi = new List<GameObject>();
    public int listcount;
    public List<Ground> groundA;
    static public bool available = false;
    public static bool toDisappear = false;//bool flag = true;
    public bool test = false;
    bool startPlace = false;
    RaycastHit hit;
    public LayerMask LayerMask; //地面的遮罩
    GameObject nowToPlace;
    float hiddenYPosition = 0.43f;
    bool afterPlace = true; //放置完地面后重置当前选中的随机地面
    bool Once= true; //判断手势的间隔


    //public HandGuesterNameSpace.HandGestures k;
    void Start()
    {
        groundA = new List<Ground>();
        provider = FindObjectOfType<LeapProvider>() as LeapProvider;
        afterPlace = true;
        SetBool(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        listcount = groundA.Count;
        
        if (test)
        {
            test = false;
            foreach(var one in groundA)
            {
                print(one.getState());
            }
            print("The End");
        }
        if (available)
        {
            SetBool(true);
            startPlace = true;
        }
        else
        {
            startPlace = false;
        }
        
        if (startPlace)
        {
            Frame frame = provider.CurrentFrame;
            if (afterPlace)
            {

                if (ground != null)
                {
                    afterPlace = false;
                }
                //nowToPlace = Instantiate(ground[UnityEngine.Random.Range(0, ground.Count)], new Vector3(0, -1, 0), Quaternion.identity);
                //nowToPlace.transform.Rotate(new Vector3(0, 90 * UnityEngine.Random.Range(0, 5), 0));//随机旋转
                nowToPlace = Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);
                float nur = GetCKG.GetFloatToScale(nowToPlace);
                nowToPlace.transform.localScale = new Vector3(nur, nowToPlace.transform.localScale.y, nur);

            }//每次随机选择地板样式
            castOnGround(frame);//调用投影函数
        }
        if (toDisappear)
        {
            SetBool(false);
        }
        
    }

    
    void SetBool(bool toset)
    {
        foreach(var obj in obi)
        {
            obj.gameObject.SetActive(toset);
        }
        
        //if((toset && obi[0].gameObject.transform.position.y<0)||(!toset && obi[0].gameObject.transform.position.y > 0))
        //{
        //    foreach(var obj in obi)
        //    {
        //        obj.gameObject.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y * -1, obj.transform.position.z);
        //    }
        //}
        
    }

    public enum State//地板状态枚举
    {
        Nothing,//啥也没有
        Placed,//上面放的有建筑
        Arable,//可耕种的
        Planted,//种着植物的
        Mature
    }

    //public class Ground
    //{
    //    public GameObject ground;
    //    public State State;
    //    public Ground(GameObject gameObject,State state)
    //    {
    //        ground = gameObject;
    //        State = state;
    //    }
    //}
    public struct Ground//地板结构体
    {
        public GameObject ground;
        public State State;

        public void SetArable() => State = State.Arable;

        public Ground(GameObject ground, State state, int i)
        {
            this.ground = ground;
            State = state;
        }

        public void setNothing() => State = State.Nothing;
        public State getState() => State;
    }

    void castOnGround(Frame frame)//在地面投影预制好的地面
    {
        foreach (var hand in frame.Hands)
        {
            if(hand.IsRight&&!afterPlace)//右手并且处于放置状态，实时发射线投影地面
            {

                int nowState = getNowState(nowToPlace);
                Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);
                //Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), new Vector3(0,0,0));
                bool rayCast = Physics.Raycast(ray, out hit, 5000, LayerMask);
                Vector3 groundPlace = hit.point;
                if (hit.point != null)
                {
                    switch (nowState)
                    {
                        case 1:
                            foreach (var nowObi in groundA)
                            {
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(0.5f, 0, 0), hit.point) < 0.25)
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(0.5f, 0, 0);
                                        break;
                                    }
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(-0.5f, 0, 0), hit.point) < 0.25)
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(-0.5f, 0, 0);
                                        break;
                                    }
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(0, 0, 0.5f), hit.point) < 0.25)
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(0, 0, 0.5f);
                                        break;
                                    }
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(0, 0, -0.5f), hit.point) < 0.25)
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(0, 0, -0.5f);
                                        break;
                                    }
                            }
                            break;
                        case 2:
                            groundPlace = hit.point;
                            break;
                        default:
                            break;
                    }////屎山代码，多跑了个遍历，但懒得改了,吸附功能核心代码
                }
                nowToPlace.transform.position = groundPlace;
                if (pdRight(frame)&&nowState!=0)//手向右确认状态，锁定地面位置，扔进数组，重置布尔值（重新选择被放置地面）
                {
                    afterPlace = true;
                    //Ground ground = new Ground();
                    Ground ground;
                    ground.ground = nowToPlace;
                    //ground.setNothing();
                    //ground.setArable();
                    ground.State = State.Nothing;
                    groundA.Add(ground);
                }
            }
           
        }
    }

    int getNowState(GameObject gameObject)//一个返回当前地面状态的函数
    {
        float minn = 1e6f;
        foreach(var nowObj in groundA)
        {
            if (gameObject.transform.position.x > nowObj.ground.transform.position.x - 0.25f && gameObject.transform.position.x < nowObj.ground.transform.position.x + 0.25f && gameObject.transform.position.z > nowObj.ground.transform.position.z - 0.25f && gameObject.transform.position.z < nowObj.ground.transform.position.z + 0.25f)
            {
                return 0;
            }
            minn = Mathf.Min(minn, getDistance(gameObject.transform.position, nowObj.ground.transform.position));//每次更新最小值
        }
        if (minn < 1f)
        {
            return 1;
        }//当前地面中心点与最近地面中心点距离小于1，返回1
        return 2;//否则返回2
    }
    
    float getDistance(Vector3 transform1,Vector3 transform2)//返回欧式距离
    {
        return Mathf.Sqrt(Mathf.Pow((transform1.x - transform2.x), 2) + Mathf.Pow((transform1.z - transform2.z), 2));
    }


    List<Vector3> returnAnchorPos(GameObject gameObject)//返回地面周围四个锚点数组的函数
    {
        List<Vector3> toBeReturn = new List<Vector3>();
        toBeReturn.Add(gameObject.transform.position + new Vector3(0.5f, 0, 0));
        toBeReturn.Add(gameObject.transform.position + new Vector3(-0.5f, 0, 0));
        toBeReturn.Add(gameObject.transform.position + new Vector3(0, 0, 0.5f));
        toBeReturn.Add(gameObject.transform.position + new Vector3(0, 0, -0.5f));
        return toBeReturn;

    }//后续可优化屎山swith循环

    bool pdRight(Frame frame)//检测手是否向右的函数
    {
        foreach (var hand in frame.Hands)
        {
            if (HandGestures.isMoveRight(hand) && Once)
            {
                Once = false;
                StartCoroutine(moveOnce());
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    static public void SetAviailableTrue()
    {
        available = true;
    }
    static public void SetAviailableFalse()
    {
        available = false;
    }
    public IEnumerator moveOnce()
    {
        yield return new WaitForSeconds(1f);
        Once = true;
    }//延时1秒

    public static implicit operator PlaceGround(Ground v)
    {
        throw new NotImplementedException();
    }
}
