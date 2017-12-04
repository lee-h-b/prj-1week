using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//스킬설명 클릭하면 들어간 스킬이 어떤스킬인지 알려줌
//이름,이미지는 한통속
//나머지는 합쳐서 써줌 
//클릭하면 넣음과 동시에 여기에 설명이 등판
public class OrderExplanation : MonoBehaviour {

    public GameObject head;//이름 +이미지
    public Text infoText;//명령의 정보글들
	// Use this for initialization
	void Start () {
        if(head == null)
            head = transform.Find("OrderImg").gameObject;
        if (infoText == null)
            infoText = transform.Find("Text").GetComponent<Text>();
	}
    public void Write(OrderScript order)
    {
        infoText.text = null;
        var orderObj = order.Order;
        head.transform.GetComponent<Image>().sprite = order.img;
        head.transform.Find("Name").GetComponent<Text>().text = "이름 : " + orderObj.orderName;
        
        if(orderObj as AttackScriptable != null)
        {
            var convert = orderObj as AttackScriptable;
            infoText.text += "기본 공격력 : " + convert.dmg.ToString() + "\n";
            infoText.text += "코스트 : " + convert.cost.ToString() + "\n";
            //나머지는 시각적으로 표현되니 넘어감
            infoText.text += "이미지내 타일\n 빨간타일 = 공격범위 \n 연두타일 = 시전위치 \n 주황타일 = 빨간타일 + 연두타일 \n 하늘타일 = 빨간타일 + 이동";
        }
        else
        {            
        }
//        infoText.text += 
    }
	// Update is called once per frame
	void Update () {
		
	}
}
