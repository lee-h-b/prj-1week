using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

//예상구성 ScriptableDB -Character에서 있는 파일들을 전부 겟해옴그리고 그들의 이미지를 불러서
//자식으로 그려줌

    //2. ScriptableDB가 아니라 오브젝트위치에서 prefab을 겟해오고 그들에게서 이미지를 불러옴

    //누르면 불러와야 하니깐 이미지내에 캐릭오브젝트를 소지하고 있는다면?
    //ㄴ스크립트의 증가로 이어질듯
    //2번으로 만약 이미지를 누른다면 id를 불러서 그 id에 맞는 오브젝트 호출(id가아니라 순서가 될지도)
    //오브젝트의 복제편리를 위해 오브젝트를 복제함
public class CharaSelect : MonoBehaviour {
    public Transform spinModel;//도는 모델 단순히 메쉬를 바꾸는것부터 트랜스폼 자체를 대입받는식 2가지가 있겠음
    //ㄴ 그냥 부모로 잡기로함
    public List<GameObject> characters;

    public Transform imgZone;//이미지 놓을곳
    public Transform memberImgZone;//멤버 이미지's

    [SerializeField]
    private string path;
    public GameObject img;
    public Text board;//텍스트를 써넣을곳

    [SerializeField]
    int cur;//이건왜? 생각해보니 동료영입 누를때 있을 cur가 필요할거같아서

    // Use this for initialization
    //업데이트에서 이미지를 계속 그리라 시키는것보단 한번만 그리는게 나을거니깐 이건 캐릭추가,삭제할때
    public void DrawMember()
    {

        for(int i = 0; i < memberImgZone.childCount; i++)
        {
            Sprite img = GameManager.inst.GetImg(i);
            memberImgZone.GetChild(i).Find("Image").GetComponent<Image>().sprite = img;
            string name = GameManager.inst.GetName(i);
            if (name == null) name = "";
            memberImgZone.GetChild(i).Find("Text").GetComponent<Text>().text = name;
            
        }
    }
    //영입버튼을 담당
    public void AddMember()
    {        
        GameManager.inst.AddMember(characters[cur]);
        DrawMember();
    }
    //멤버제외 << 영입이나 제외나 이미지를 드래그 드랍하는식으로 하는게 세련되보일것
    public void DelMember()
    {
        GameManager.inst.DelMember();
        DrawMember();
    }
    //캐릭터들 가져옴
    public void GetAllChara()
    {
        //        string[] inFiles2 = Directory.GetFiles(Application.dataPath + "/Resources/Chara");
        //LoadAll로 결정 이유는 짧아서 그리고 빌드할때도 이게 나을지도?

        if (path == null) return;

        var loadData = Resources.LoadAll("Chara");
        Debug.Log(Resources.LoadAll("Chara").Length);
        Debug.Log(loadData.Length);
        for(int i = 0; i < loadData.Length; i++)
        {
            characters.Add(loadData[i] as GameObject);
            Debug.Log(characters[i]);
            SummonImg(characters[i].GetComponent<CharaScript>() );
        }
    }
    //이미지들을 소환 + 클릭시 이벤트 동적할당
    public void SummonImg(CharaScript chara)
    {
        GameObject temp;
        if (img == null)//만약 이미지가 없을경우
        {
            var go = new GameObject();
            temp = Instantiate(go, imgZone);
            temp.AddComponent<RectTransform>();
            temp.AddComponent<CanvasRenderer>();
            temp.AddComponent<Button>();
            temp.AddComponent<Image>();
            Destroy(go);
        }
        
        else temp = Instantiate(img, imgZone);

        var tempimg = temp.GetComponent<Image>();
        Button b = temp.GetComponent<Button>();
        Transform test = temp.transform;
        b.onClick.AddListener(() => SelectImg(test));

        tempimg.sprite = chara.Info.image;
    }

    //이미지 클릭하면 행하는 이벤트
    public void SelectImg(Transform img)
    {
        int val = img.transform.GetSiblingIndex();//img가 몇번째 자식인지 때고
        //계속 클릭하면 지우고 소환하고 반복할거아님? 그건 안좋을거같음
        if(cur != val )
        {
            cur = val;
            ChangeModelObject();
            WriteCharaInfo();
        }
    }
    //이건 이미지 클릭하면 호출할것
    public void ChangeModelObject()
    {
        //일단 지우고
        if (spinModel.childCount > 0)
            for (int i = 0; i < spinModel.childCount; i++)
                Destroy(spinModel.GetChild(i).gameObject );
        //오브젝트를 새로 복제해서 넣어줌
        Instantiate(characters[cur], spinModel);
    }
    //캐릭터의 정보,이름등을 써놓기맨
    public void WriteCharaInfo()
    {
        var info = characters[cur].GetComponent<CharaScript>().Info;
        board.text = "이름 : " + info.charaName +"\n";
        board.text += "체력 : " + info.hp + "\n";
        board.text += "공격력 : " + info.atk + "\n";
        board.text += "방어력 : " + info.def + "\n";
        board.text += "스피드 : " + info.speed + "\n";
        board.text += "설명 : " + info.description + "\n";
    }
    //상황에따라 enable로 개명,일부 이동 가능
    void Start () {
        if (spinModel == null) spinModel = transform.Find("Model");
        if (imgZone == null) imgZone = transform.Find("SeletView").Find("Viewport").Find("Content");
        if (board == null) board = transform.Find("Explanation").GetChild(0).GetComponent<Text>();
        if (memberImgZone == null) memberImgZone = transform.Find("MemberImg");
        GetAllChara();
        cur = characters.Count;//커서의 기본값을 카운트로 꾀한다
        DrawMember();
    }
	
	// Update is called once per frame
	void Update () {
        spinModel.Rotate(new Vector3(0, 20f * Time.deltaTime));
	}
}
