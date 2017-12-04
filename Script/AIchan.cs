using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIchan : MonoBehaviour {

    // Use this for initialization
    //스킬을 알아서 넣는 무언가
    public List<int> memberCode;//멤버로 넣는게 더 보기도 편할텐데 일단2P의 가능성도 있고하니까

    //호출시점은 턴이 넘어가는 그순간
    public void SetSkill()
    {
        var members = PartyManager.inst.Member2;
        var playerMembers = PartyManager.inst.Member1;
        int targetNum = 0;

        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].Dead == true) continue;//죽으면 넘어감
            int distance = 999;
            for(int j = 0; j < playerMembers.Count; j++)
            {
                //타겟이 누군지는 알필요없음 가까우면 장땡
                int temp = (int)Vector3.Distance(members[i].transform.position, playerMembers[j].transform.position) - 10;
                if (temp < distance && playerMembers[j].Dead == false)
                {
                    distance = temp;
                    targetNum = j;
                }
            }
            for (int j = 0; j < 3; j++)
            {
                for(int k = GameManager.inst.orders.Count -1; k >=  0; k--)
                {
                    //역순으로 움직이고 확률에따라 내가 저걸 공격할수 있다면 공격스킬만, 아니면 이동스킬만
                    //스킬주머니에 넣는방식
                    var order = GameManager.inst.orders[k].GetComponent<OrderScript>().Order;
                    int varX = order.x * GameManager.inst.mapEnvi.gridSize;
                    int varY = order.y * GameManager.inst.mapEnvi.gridSize;
                    //ㄴ 저오브젝트가 공격스킬이고, 현재에너미 + varX,varY를 했을때 충분히 공격범위내+ MP가 충분하면
                    Vector3 atkRange = new Vector3(members[i].transform.position.x, 0, members[i].transform.position.z );
                    Vector3 targetPos = new Vector3(Mathf.Abs(playerMembers[targetNum].transform.position.x), 0, Mathf.Abs(playerMembers[targetNum].transform.position.z) );

                    if (GameManager.inst.orders[k].GetComponent<OrderScript>().Order as AttackScriptable != null && (atkRange.x + varX) - targetPos.x >= 0 && (atkRange.z + varY) - targetPos.y >= 0)
                    {
                        //칼이 첫방에 쏠때 데미지 안먹는게 있나?
                        if(members[i].Orders[j] == null)
                        {
                            Debug.Log("첫스킬!");
                            members[i].AddOrder(GameManager.inst.orders[k].GetComponent<OrderScript>().Order);
                        }
                        //50%로 교체
                        else if(Random.Range(0,2) %2 == 0)
                        {
                            Debug.Log("첫스킬교체");
                            members[i].DelOrder(j);//j번째에 있을거라고 가정하고 지우는거임
                            members[i].AddOrder(GameManager.inst.orders[k].GetComponent<OrderScript>().Order);
                        }
                    }

                    else if(GameManager.inst.orders[k].GetComponent<OrderScript>().Order as AttackScriptable == null && (atkRange.x + varX) - targetPos.x < 0 && (atkRange.z + varY) - targetPos.y < 0)
                    {
                        //그냥 실패 했음! 즉 여기는이동
                        if (members[i].Orders[j] == null)
                        {
                            Debug.Log("첫이동!");
                            members[i].AddOrder(GameManager.inst.orders[k].GetComponent<OrderScript>().Order);
                        }
                        //50%로 교체
                        else if (Random.Range(0, 2) % 2 == 0)
                        {
                            Debug.Log("첫ㅇㄷ교체");
                            members[i].DelOrder(j);//j번째에 있을거라고 가정하고 지우는거임
                            members[i].AddOrder(GameManager.inst.orders[k].GetComponent<OrderScript>().Order);
                        }
                    }

                }
            }
        }
    }
	public void SettingMember () {
        for (int i = 0; i < GameManager.inst.MaxMember; i++)
        {
            var cur = Random.Range(0, GameManager.inst.characters.Count);
            //플레이어가 유닛넣듯이 기동
            memberCode.Add(GameManager.inst.characters[cur].GetComponent<CharaScript>().Info.ID );
        }
	}
	void Start()
    {
        SettingMember();//다시하기하면 이걸 다시실행할피료있음
    }
	// Update is called once per frame
	void Update () {
		
	}
}
