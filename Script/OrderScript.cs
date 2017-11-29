using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//커맨드 스크립트 아마 여기서 커맨드들을 입력받아서 할거임
//갖는건 스킬이미지가 갖게됨
//실행을 어떻게 할지가 관건

    //오더에게 유저를 넘기는게 될듯
[RequireComponent ( typeof(Image))]
public class OrderScript : MonoBehaviour {
    
    [SerializeField]
    OrderScriptableObj order;//스킬의 정보 이거는 커맨드창에서 할당해줄것


    [SerializeField]
    GameObject user;//사용할 사람
    public Sprite img;//이 오브젝트 아이콘 이미지

    
	// Use this for initialization
	void Start () {
        if (GetComponent<Image>().sprite == null) GetComponent<Image>().sprite = img;
        order.envi = GameManager.inst.mapEnvi;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
