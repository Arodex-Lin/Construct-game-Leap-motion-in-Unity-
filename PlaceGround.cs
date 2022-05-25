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
        listcount = groundA.Count;//获取当前地面的个数
        
        if (test)//测试的一个判断
        {
            test = false;
            foreach(var one in groundA)
            {
                print(one.getState());
            }
            print("The End");
        }
        if (available)//是否开始放置地面方法，是从按钮控制的
        {
            SetBool(true);
            startPlace = true;//开始放置地面
        }
        else
        {
            startPlace = false;
        }
        
        if (startPlace)//如果开始放置地面
        {
            Frame frame = provider.CurrentFrame;
            if (afterPlace)//已经放完这一个，就另复制一个备用
            {

                if (ground != null)
                {
                    afterPlace = false;
                }
                //nowToPlace = Instantiate(ground[UnityEngine.Random.Range(0, ground.Count)], new Vector3(0, -1, 0), Quaternion.identity);
                //nowToPlace.transform.Rotate(new Vector3(0, 90 * UnityEngine.Random.Range(0, 5), 0));//随机旋转
                nowToPlace = Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);//复制
                float nur = GetCKG.GetFloatToScale(nowToPlace);//利用一个无比智障的函数把物体调到合适的大小
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
        public State State;//包含一个地面游戏对象和地板状态

        public void SetArable() => State = State.Arable;//把状态设为Arable

        public Ground(GameObject ground, State state)//构造函数
        {
            this.ground = ground;
            State = state;
        }

        public void setNothing() => State = State.Nothing;//状态设为Nothing
        public State getState() => State;//获取当前地板状态
    }

    void castOnGround(Frame frame)//在地面投影预制好的地面
    {
        foreach (var hand in frame.Hands)
        {
            if(hand.IsRight&&!afterPlace)//右手并且处于放置状态，实时发射线投影地面
            {

                int nowState = getNowState(nowToPlace);//获取当前状态
                Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);//向手指指向地方发射射线
                //Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), new Vector3(0,0,0));
                bool rayCast = Physics.Raycast(ray, out hit, 5000, LayerMask);//通过out获取hit信息
                Vector3 groundPlace = hit.point;//射线与地面相交处为需要放置的位置
                if (hit.point != null)
                {
                    switch (nowState)
                    {
                        case 1://可吸附
                            foreach (var nowObi in groundA)//四个循环，左上到右下四个方向，完成吸附的实现
                            {
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(0.5f, 0, 0), hit.point) < 0.25)//投影点和需要吸附的点之间的距离足够小时
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(0.5f, 0, 0);//需要放置的位置直接设置为吸附点（即物体的中心位置右0.5f处）
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
                            groundPlace = hit.point;//离最近的地面的距离不够吸附条件，放置位置仍未投影点
                            break;
                        default:
                            break;
                    }////屎山代码，多跑了个遍历，但懒得改了,吸附功能核心代码
                }
                nowToPlace.transform.position = groundPlace;//把当前地面位置设为目标位置
                if (pdRight(frame)&&nowState!=0)//手向右挥确认放置，锁定地面位置，把地面扔进列表，重置布尔值（重新选择被放置地面）
                {
                    afterPlace = true;
                    //Ground ground = new Ground();
                    Ground ground;
                    ground.ground = nowToPlace;
                    //ground.setNothing();
                    //ground.setArable();
                    ground.State = State.Nothing;//构造一个结构体
                    groundA.Add(ground);//扔进groundA列表里
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
    
    float getDistance(Vector3 transform1,Vector3 transform2)//返回距离
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

    }//后续可优化屎山swith循环，目前没啥用

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

    static public void SetAviailableTrue()//通过按钮调用，更改available的值
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

   }
