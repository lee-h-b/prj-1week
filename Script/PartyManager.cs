using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//게임매니저가 파티멤버를 관리하는건 어색하다고 판단해서 멤버관리는 얘가함
//파티전멸등을 체크해줄거임
public class PartyManager : MonoBehaviour {
    [SerializeField]
    List<CharaScript> p1Members;//1p

    [SerializeField]
    List<CharaScript> p2Members;//AI or 2p

    public List<CharaScript> Member1
    {
        get { return p1Members; }
    }
    public List<CharaScript> Member2
    {
        get { return p2Members; }
    }
    static public PartyManager inst;
    //플레이어 1에게만 주는구조 뒤의 playerCur가 2p,ai도 할당할것
    //스크립터블오브젝을 줄것인가? 스크립트를 줄것인가?
    //스크립트의 이름이 필요없을거같음
    public void AddOrder(OrderScriptableObj order,int cur, int playerCur = 1)
    {
        List<CharaScript> member;
        if (playerCur % 2 == 1) member = p1Members;
        else member = p2Members;
        Debug.Log("cur = " + cur);
        p1Members[cur].AddOrder(order);
    }
    //걍 addOrder복붙 +수정
    public void DelOrder(int playerNum,int orderNum, int playerCur = 1)
    {
        List<CharaScript> member;
        if (playerCur % 2 == 1) member = p1Members;
        else member = p2Members;
        p1Members[playerNum].DelOrder(orderNum);

    }
    //멤버추가 pC는 위의 플레이어Cur의 약자
    public List<CharaScript> Copy(int val)
    {
        List<CharaScript> output = new List<CharaScript>();
        List<CharaScript> target;
        if (val % 2 == 1)
            target = Member1;
        else target = Member2;
        for(int i = 0; i<target.Count; i++)
        {
            output.Add(target[i]);
        }
        return output;
    }
    public void AddMember(CharaScript member, int pC =1)
    {
        if (pC % 2 == 1)
        {           
            p1Members.Add(member);
        }
        else
            p2Members.Add(member);
    }
    //멤버가 전부 죽었는지 P1부터 돌려보기
    public int CheckMemberAnnihilation()
    {
        bool liveGauger = true;
        for(int i = 0; i < p1Members.Count; i++)
        {
            if (p1Members[i].Dead == false) liveGauger = false;
            //만약 1명이라도 살아있다면 liveGauger는 거짓이될거임
        }
        if (liveGauger == true) return 2;//2가 이겼엉
        liveGauger = true;
        for (int i = 0; i < p2Members.Count; i++)
        {
            if (p2Members[i].Dead == false) liveGauger = false;
        }

        if (liveGauger == true) return 1;//1이 이김
        return 0;//암도안이김
    }
    void Awake()
    {
        if (inst == null) inst = this;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
