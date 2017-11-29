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
        SetBoard();
        cur = val;
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
            Instantiate(GameManager.inst.orders[i], board);
    }
    //여기 픽은 임시임 스킬오브젝트를 옮기도록 할거같음  
    //스킬을 클릭하면 좌측하단 흰이미지에 좌측부터 들어가고 그스킬을 필드에 오브젝트가 받음  
    void PickOrder(GameObject order)
    {
        var path = memberTab[cur].Find("PickOrders");
        for (int i = 0; i < path.childCount; i++)
        {
            //이미지가 빈 깨끗한걸 발견
            if(path.GetChild(i).GetComponent<Image>().sprite == null)
                //이렇게 이미지만넣어줌
                path.GetChild(i).GetComponent<Image>().sprite = order.GetComponent<OrderScript>().img;
            //스킬자체를 할당받음
            //게임매니저가 아닌 현재 나와있는 오브젝트에게 줘야함
//            GameManager.inst.PlayerMember[cur].AddOrder(order.GetComponent<OrderScript>() );
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
