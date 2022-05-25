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
    public LayerMask LayerMask; //���������
    GameObject nowToPlace;
    float hiddenYPosition = 0.43f;
    bool afterPlace = true; //�������������õ�ǰѡ�е��������
    bool Once= true; //�ж����Ƶļ��


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
        listcount = groundA.Count;//��ȡ��ǰ����ĸ���
        
        if (test)//���Ե�һ���ж�
        {
            test = false;
            foreach(var one in groundA)
            {
                print(one.getState());
            }
            print("The End");
        }
        if (available)//�Ƿ�ʼ���õ��淽�����ǴӰ�ť���Ƶ�
        {
            SetBool(true);
            startPlace = true;//��ʼ���õ���
        }
        else
        {
            startPlace = false;
        }
        
        if (startPlace)//�����ʼ���õ���
        {
            Frame frame = provider.CurrentFrame;
            if (afterPlace)//�Ѿ�������һ����������һ������
            {

                if (ground != null)
                {
                    afterPlace = false;
                }
                //nowToPlace = Instantiate(ground[UnityEngine.Random.Range(0, ground.Count)], new Vector3(0, -1, 0), Quaternion.identity);
                //nowToPlace.transform.Rotate(new Vector3(0, 90 * UnityEngine.Random.Range(0, 5), 0));//�����ת
                nowToPlace = Instantiate(ground, new Vector3(0, 0, 0), Quaternion.identity);//����
                float nur = GetCKG.GetFloatToScale(nowToPlace);//����һ���ޱ����ϵĺ���������������ʵĴ�С
                nowToPlace.transform.localScale = new Vector3(nur, nowToPlace.transform.localScale.y, nur);

            }//ÿ�����ѡ��ذ���ʽ
            castOnGround(frame);//����ͶӰ����
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

    public enum State//�ذ�״̬ö��
    {
        Nothing,//ɶҲû��
        Placed,//����ŵ��н���
        Arable,//�ɸ��ֵ�
        Planted,//����ֲ���
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
    public struct Ground//�ذ�ṹ��
    {
        public GameObject ground;
        public State State;//����һ��������Ϸ����͵ذ�״̬

        public void SetArable() => State = State.Arable;//��״̬��ΪArable

        public Ground(GameObject ground, State state)//���캯��
        {
            this.ground = ground;
            State = state;
        }

        public void setNothing() => State = State.Nothing;//״̬��ΪNothing
        public State getState() => State;//��ȡ��ǰ�ذ�״̬
    }

    void castOnGround(Frame frame)//�ڵ���ͶӰԤ�ƺõĵ���
    {
        foreach (var hand in frame.Hands)
        {
            if(hand.IsRight&&!afterPlace)//���ֲ��Ҵ��ڷ���״̬��ʵʱ������ͶӰ����
            {

                int nowState = getNowState(nowToPlace);//��ȡ��ǰ״̬
                Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), hand.Direction.ToVector3().normalized);//����ָָ��ط���������
                //Ray ray = new Ray(new Vector3(hand.WristPosition.ToVector3().x, hand.WristPosition.ToVector3().y + 0.04f, hand.WristPosition.ToVector3().z), new Vector3(0,0,0));
                bool rayCast = Physics.Raycast(ray, out hit, 5000, LayerMask);//ͨ��out��ȡhit��Ϣ
                Vector3 groundPlace = hit.point;//����������ཻ��Ϊ��Ҫ���õ�λ��
                if (hit.point != null)
                {
                    switch (nowState)
                    {
                        case 1://������
                            foreach (var nowObi in groundA)//�ĸ�ѭ�������ϵ������ĸ��������������ʵ��
                            {
                                    if (getDistance(nowObi.ground.transform.position + new Vector3(0.5f, 0, 0), hit.point) < 0.25)//ͶӰ�����Ҫ�����ĵ�֮��ľ����㹻Сʱ
                                    {
                                        groundPlace = nowObi.ground.transform.position + new Vector3(0.5f, 0, 0);//��Ҫ���õ�λ��ֱ������Ϊ�����㣨�����������λ����0.5f����
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
                            groundPlace = hit.point;//������ĵ���ľ��벻����������������λ����δͶӰ��
                            break;
                        default:
                            break;
                    }////ʺɽ���룬�����˸������������ø���,�������ܺ��Ĵ���
                }
                nowToPlace.transform.position = groundPlace;//�ѵ�ǰ����λ����ΪĿ��λ��
                if (pdRight(frame)&&nowState!=0)//�����һ�ȷ�Ϸ��ã���������λ�ã��ѵ����ӽ��б����ò���ֵ������ѡ�񱻷��õ��棩
                {
                    afterPlace = true;
                    //Ground ground = new Ground();
                    Ground ground;
                    ground.ground = nowToPlace;
                    //ground.setNothing();
                    //ground.setArable();
                    ground.State = State.Nothing;//����һ���ṹ��
                    groundA.Add(ground);//�ӽ�groundA�б���
                }
            }
           
        }
    }

    int getNowState(GameObject gameObject)//һ�����ص�ǰ����״̬�ĺ���
    {
        float minn = 1e6f;
        foreach(var nowObj in groundA)
        {
            if (gameObject.transform.position.x > nowObj.ground.transform.position.x - 0.25f && gameObject.transform.position.x < nowObj.ground.transform.position.x + 0.25f && gameObject.transform.position.z > nowObj.ground.transform.position.z - 0.25f && gameObject.transform.position.z < nowObj.ground.transform.position.z + 0.25f)
            {
                return 0;
            }
            minn = Mathf.Min(minn, getDistance(gameObject.transform.position, nowObj.ground.transform.position));//ÿ�θ�����Сֵ
        }
        if (minn < 1f)
        {
            return 1;
        }//��ǰ�������ĵ�������������ĵ����С��1������1
        return 2;//���򷵻�2
    }
    
    float getDistance(Vector3 transform1,Vector3 transform2)//���ؾ���
    {
        return Mathf.Sqrt(Mathf.Pow((transform1.x - transform2.x), 2) + Mathf.Pow((transform1.z - transform2.z), 2));
    }


    List<Vector3> returnAnchorPos(GameObject gameObject)//���ص�����Χ�ĸ�ê������ĺ���
    {
        List<Vector3> toBeReturn = new List<Vector3>();
        toBeReturn.Add(gameObject.transform.position + new Vector3(0.5f, 0, 0));
        toBeReturn.Add(gameObject.transform.position + new Vector3(-0.5f, 0, 0));
        toBeReturn.Add(gameObject.transform.position + new Vector3(0, 0, 0.5f));
        toBeReturn.Add(gameObject.transform.position + new Vector3(0, 0, -0.5f));
        return toBeReturn;

    }//�������Ż�ʺɽswithѭ����Ŀǰûɶ��

    bool pdRight(Frame frame)//������Ƿ����ҵĺ���
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

    static public void SetAviailableTrue()//ͨ����ť���ã�����available��ֵ
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
    }//��ʱ1��

   }
