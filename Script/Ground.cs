using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Envi
{
    //절대값을 활용할것
    public int maxX;
    public int maxY;
    //x축을 기준으로 할거고 수정될 이유가없기에 보류
    public int gridSize;
}
public class Ground : MonoBehaviour {
    //일단은 간단하게 캐릭의 스폰만 담당함
    //최대위치,최소위치도 필요할까?
    public Envi envi;
    Transform Spawn1, Spawn2;
	// Use this for initialization
    //캐릭소환술 
    void CharaSpawn(List<int> charaCode, bool p2 = false)
    {
        Transform temp;
        //위치를 랜덤하게 하고 싶었는데 시간이 오래걸릴듯
        if (p2 == true)
            temp = Spawn2;
        else temp = Spawn1;

        for(int i = 0; i < temp.childCount; i++)
        {
            if (i >= charaCode.Count || charaCode[i] == 0) break;
            int num = GameManager.inst.characters.FindIndex
                (item => item.GetComponent<CharaScript>().Info.ID == charaCode[i]);

            if (num != -1)
            {
                //로테이션까지하기 번거로우니깐 부모위치로 해서 위치등을 동기화시키고 해제함
                var obj = Instantiate(GameManager.inst.characters[num],temp.GetChild(i));
                if (p2 == false)
                {
                    PartyManager.inst.AddMember(obj.GetComponent<CharaScript>());
//                    obj.transform.SetParent(pPos1);
                    obj.tag = "P1";
//                    obj.transform.SetParent(null);
                }
                else
                {
                    PartyManager.inst.AddMember(obj.GetComponent<CharaScript>() , 2);
                    obj.tag = "P2";
                    //                    obj.transform.SetParent(pPos2);
                }
            }
        }
    }
    void Awake()
    {

        if (Spawn1 == null) Spawn1 = transform.Find("Pos").Find("Left");
        CharaSpawn(GameManager.inst.PlayerMember, false);//명령넣을때 보여줘야 하기에 미리만듬
    }
	void Start () {
        if (Spawn2 == null) Spawn2 = transform.Find("Pos").Find("Right");
        //5를 노림ㅎ
        envi.maxX = (int)transform.localScale.x * transform.Find("Row1").childCount;
        envi.maxY = (int)transform.localScale.z * transform.Find("Row1").childCount;
        envi.gridSize = (int)transform.localScale.x * 2;
        GameManager.inst.mapEnvi = envi;

        CharaSpawn(PartyManager.inst.GetComponent<AIchan>().memberCode,true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
