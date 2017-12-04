using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIDraw : MonoBehaviour {

    // Use this for initialization
    //게임매니저에 있을 온로드를 동적할당 해줄것 
    public Transform player1Gauges;
    public Transform player2Gauges;
    public Transform result;

    //멤버매니저에서 여기에 필요한정보를 겟
	void Start () {
        if (player1Gauges == null) player1Gauges = transform.Find("Player1Bars");
        if (player2Gauges == null) player2Gauges = transform.Find("Player2Bars");
        if (result == null) result = transform.Find("WinResult");
        
    }
    public void Restart()
    {
        //게임매니저를 인자로해서 넘기는게 안되길래 이렇게 따로만들어둠
        GameManager.inst.ReStart();
    }
    public void GoToMenu()
    {
        GameManager.inst.LoadScene("MainScene");
    }

    // Update is called once per frame
    //승리 결과 호출 이걸 호출하는 애는 자기를 다운 시켜야할것
    public void Win(int winCur = 0)
    {
        //턴도중에 이게 나오도록해서 테이블은 건들지도 않고 종료를 꾀함
        TurnManager.inst.gameObject.SetActive(false);
        result.gameObject.SetActive(true);
        if (winCur == 1) result.Find("Text").GetComponent<Text>().text = "Player1 Win!";
        else if(winCur == 2) result.Find("Text").GetComponent<Text>().text = "Player2 Win!";
        
    }
    void DrawUI(int cur)
    {
        Transform tempUI;
        List<CharaScript> temp;
        if (cur == 1)
        {
            temp = PartyManager.inst.Member1;
            tempUI = player1Gauges;
        }
        else
        {
            temp = PartyManager.inst.Member2;
            tempUI = player2Gauges;
        }

        for(int i = 0; i < temp.Count; i++)
        {
            if(temp[i].Dead == true)
            {
                tempUI.GetChild(i).gameObject.SetActive(false);
                continue;
            }

            tempUI.GetChild(i).gameObject.SetActive(true);
            //hp그리기
            float hpPer = (float)temp[i].Status.hp / (float)temp[i].MaxHp;
            Vector3 pos = temp[i].transform.position;
            pos.y += 4f;//한번 4f만

            tempUI.GetChild(i).Find("HPbar").Find("Cur").GetComponent<Image>().fillAmount = hpPer;
            tempUI.GetChild(i).Find("HPbar").Find("Text").GetComponent<Text>().text = temp[i].Status.hp.ToString() + "/" + temp[i].MaxHp;
            //mp그리기
            float mpPer = (float)temp[i].Status.mp / (float)temp[i].MaxMp;
            tempUI.GetChild(i).Find("MPbar").Find("Cur").GetComponent<Image>().fillAmount = mpPer;
            tempUI.GetChild(i).Find("MPbar").Find("Text").GetComponent<Text>().text = temp[i].Status.mp.ToString() + "/" + temp[i].MaxMp;

            tempUI.GetChild(i).transform.position = Camera.main.WorldToScreenPoint(pos);

        }
    }
	void Update () {
        //플레잉이 트루일때만 움직일거임 false 되는순간 탈락!
        if (TurnManager.inst.playing == true)
        {
            DrawUI(1);

            DrawUI(2);
        }
        else//true가 아닌 거짓
        {
            for (int i = 0; i < player1Gauges.childCount; i++)
            {
                player1Gauges.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < player2Gauges.childCount; i++)
            {
                player2Gauges.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
