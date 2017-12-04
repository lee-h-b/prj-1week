using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComandTable : MonoBehaviour {

    // Use this for initialization
    //플레이어가 업데이트? 스타트? 도중에 돌리게 될거임
    //플레이어가 아니라면 다른 무언가가 대신 호출시킬거같음 war처럼
    //일단 여기선 orderscript의 배치와 거기의 order의 정의가 핵심
    //스킬을 전부 복제해서 Boader에 넣둘거임
    //여기서 복제 과정에서 현재 player와 맞는 코드 or 해금된것만 나올것
    //다른 player를 누르면 그걸로 바뀌겠지
    Transform board;
    [SerializeField]
    private int cur = 0;
    //이미지 놓을곳은 없음 그냥 스킬에서 빼올거

     [SerializeField]
    Transform[] memberTab = new Transform[3];//상단의 멤버탭
    //ㄴ 스타트에 설정은 안해둘것
    //원작은 클릭으로 충분하니 나도 따라서 클릭으로 퉁칠듯


    //val에 맞는 멤버로 변경
    //외적으론 ui가 변경되고 내적으론 SetOrder가 받는값이 바뀌는데 우선은 SetOrder를 청소하고 다시함
    public void OtherOrderMember(int val)
    {
        if (val == cur) return;//val = cur이면 메소드 실행 안함
        ColorBlock cb;
        //버튼클릭 시각화
        cb = memberTab[cur].GetComponent<Button>().colors;
        cb.normalColor = Color.white;
        cb.highlightedColor = cb.normalColor;
        memberTab[cur].GetComponent<Button>().colors = cb;

        cb = memberTab[val].GetComponent<Button>().colors;
        cb.normalColor = Color.gray;
        cb.highlightedColor = cb.normalColor;
        memberTab[val].GetComponent<Button>().colors = cb;

        memberTab[cur].Find("PickOrders").gameObject.SetActive(false);
        memberTab[val].Find("PickOrders").gameObject.SetActive(true);
        //여기서 체력,마력을 간접적으로 보여줌 갱신은? 다른곳에서 할듯
        memberTab[cur].Find("Hp").gameObject.SetActive(false);
        memberTab[cur].Find("Mp").gameObject.SetActive(false);


        memberTab[val].Find("Hp").gameObject.SetActive(true);
        memberTab[val].Find("Mp").gameObject.SetActive(true);
        SetBoard();
        cur = val;

    }
    //죽은건 1인데 빼는건 다른번호가됨
    //멤버가 죽었는가 체크 참이면 멤버탭에서 제외
    //1번만 하면되고 갱신을 이 창이 재활성 될때마다 하면되니 enable에서함 
    public void DisableMemberCheck()
    {
        for(int i = 0; i < PartyManager.inst.Member1.Count; i++)
        {
            if(PartyManager.inst.Member1[i].Dead == true)
            {
                memberTab[i].gameObject.SetActive(false);
                //cur를 바꿔서 활성을 바꿈

                if (cur == i) cur++;
                if (cur >= PartyManager.inst.Member1.Count) cur = 0;
                memberTab[cur].Find("PickOrders").gameObject.SetActive(true);//혹시 모르니 활성화
                //ㄴ 문제 그아래 자식은 활성화가 안되있음
            }
//            else
  //              memberTab[i].gameObject.SetActive(true);

        }
    }
    //스킬 놓기 추가,변경의 가능성 매우 높음
    void SetBoard()
    {
        //전부지우고 추가 왜냐면 추후 전용커맨드를 만들거같아서
        foreach(Transform child in board)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < GameManager.inst.orders.Count; i++)
        {
            var temp = Instantiate(GameManager.inst.orders[i], board);
            temp.GetComponent<Button>().onClick.AddListener(() => PickOrder(temp));
        }
    }
    //여기 픽은 임시임 스킬오브젝트를 옮기도록 할거같음  
    //스킬을 클릭하면 좌측하단 흰이미지에 좌측부터 들어가고 그스킬을 필드에 오브젝트가 받음  
    void PickOrder(GameObject order)
    {
        var path = memberTab[cur].Find("PickOrders");
        for (int i = 0; i < path.childCount; i++)
        {
            //이미지가 빈 깨끗한걸 발견
            if (path.GetChild(i).GetComponent<Image>().sprite == null)
            {
                //이렇게 이미지만넣어줌
                path.GetChild(i).GetComponent<Image>().sprite = order.GetComponent<OrderScript>().img;
                //명령은 멤버에게
                PartyManager.inst.AddOrder(order.GetComponent<OrderScript>().Order,cur);
                //                order.SetActive(false); 해당명령을 2번못하게하는건데 일단 안함
                //              여기에 인포이미지 그려주기
                transform.Find("Explanation").GetComponent<OrderExplanation>().Write(order.GetComponent<OrderScript>());

                break;
            }
            //스킬자체를 할당받음
            //게임매니저가 아닌 현재 나와있는 오브젝트에게 줘야함
//            GameManager.inst.PlayerMember[cur].AddOrder(order.GetComponent<OrderScript>() );
        }
    }
    //해당위치 오더지우기
    public void PopOrder(int val)
    {
        var path = memberTab[cur].Find("PickOrders");
        //이미지(스킬)이 있을경우에만
        if (path.GetChild(val - 1).GetComponent<Image>().sprite != null)
        {
            PartyManager.inst.DelOrder(cur, val - 1);
            //이미지지움
            path.GetChild(val - 1).GetComponent<Image>().sprite = null;

        }
    }
    //해당탭 청소용
    public void OrderClear(bool force =false)
    {
        Debug.Log(force);
        if (force == false)
        {
            var path = memberTab[cur].Find("PickOrders");
            for (int i = 0; i < path.childCount; i++)
            {
                if (path.GetChild(i).GetComponent<Image>().sprite != null)
                {
                    PartyManager.inst.DelOrder(cur, i);
                    //이미지지움
                    path.GetChild(i).GetComponent<Image>().sprite = null;
                }
            }
        }
        else
        {
            for(int i = 0; i< 3; i++)
            {
                var path = memberTab[i].Find("PickOrders");
                for(int j = 0; j < path.childCount; j++)
                {
                    path.GetChild(j).GetComponent<Image>().sprite = null;
                }
            }
        }
    }
    //턴플레이 즉 이걸 꺼도 될지? 확인
    public void CheckPlayTurn()
    {
        for(int i = 0; i < memberTab.Length; i++)
        {
            var path = memberTab[i].Find("PickOrders");
            //애초에 없었다면 넘어가기
            if (memberTab[i].gameObject.activeSelf == false) continue;

            for (int j = 0; j < memberTab[i].Find("PickOrders").childCount; j++)
            {
                if (path.GetChild(j).GetComponent<Image>().sprite == null)
                {
                    Debug.Log("안되");
                    return;//스킬이없음안되지
                }
            }
        }
        gameObject.SetActive(false);
        Debug.Log("됭");
        OrderClear(true);//이게 전부 지우는게아니라 1개만지움 <<정확히는 이미지가 남는다
        TurnManager.inst.GetComponent<AIchan>().SetSkill();
        TurnManager.inst.ActionTurn();
    }
    public void DrawHPMP()
    {
        for(int i = 0; i < PartyManager.inst.Member1.Count; i++)
        {
            //죽은캐릭은 클릭 자체를 못할거기에 예외처리는 일단 안함
            float per = (float)PartyManager.inst.Member1[i].Status.hp / (float)PartyManager.inst.Member1[i].MaxHp;
            memberTab[i].Find("Hp").Find("Cur").GetComponent<Image>().fillAmount = per;
            per = (float)PartyManager.inst.Member1[i].Status.mp / (float)PartyManager.inst.Member1[i].MaxMp;
            memberTab[i].Find("Mp").Find("Cur").GetComponent<Image>().fillAmount = per;

            memberTab[i].Find("Hp").Find("Cur").Find("Text").GetComponent<Text>().text = PartyManager.inst.Member1[i].Status.hp.ToString() + " / " + PartyManager.inst.Member1[i].MaxHp.ToString();
            memberTab[i].Find("Mp").Find("Cur").Find("Text").GetComponent<Text>().text = PartyManager.inst.Member1[i].Status.mp.ToString() + " / " + PartyManager.inst.Member1[i].MaxMp.ToString();
        }
    }
    void Start () {
        if (board == null) board = transform.Find("Board");

        if (memberTab[0] == null) Debug.LogError("멤버탭 확인");
        //하드코딩
        if (GameManager.inst.PlayerMember.Count < 3) memberTab[2].gameObject.SetActive(false);
        if (GameManager.inst.PlayerMember.Count < 2) memberTab[1].gameObject.SetActive(false);
        if (GameManager.inst.PlayerMember.Count < 1) memberTab[0].gameObject.SetActive(false);

        SetBoard();
        DrawHPMP();

    }
    void OnEnable()
    {
        DisableMemberCheck();
        //체력 마력 갱신 체력마력이 누를때마다 재정의 되는건 비효율적임
        DrawHPMP();
    }
	// Update is called once per frame
	void Update () {
		
	}
}
