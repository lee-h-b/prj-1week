using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gacha : MonoBehaviour {
    int result;
    public GameObject oneMoreButton;
    public GameObject gachaButton;
    public Text resultText;
    public MenuUtilityScript MUS;
    private Vector3 buttonPos;
    float per = 0;
    void Start()
    {
        if (gachaButton == null) gachaButton = transform.Find("Box").gameObject;
        if (oneMoreButton == null) oneMoreButton = transform.Find("OneMore").gameObject;
        if (resultText == null) resultText = transform.Find("Result").Find("Text").GetComponent<Text>();
        if (MUS == null) MUS = transform.parent.parent.Find("TopMenuPanel").GetComponent<MenuUtilityScript>();
        buttonPos = gachaButton.transform.localPosition;
    }
    //박스 내려가는 애니메션용ㅎ
    IEnumerator BoxDown()
    {
        var Period = new Vector3(buttonPos.x, -500f, buttonPos.z);

        while (per < 1)
        {
            per += Time.deltaTime;
            gachaButton.transform.localPosition = Vector3.Lerp(buttonPos, Period, per);
            yield return null;
         }
        if (oneMoreButton.activeSelf == false)
            oneMoreButton.SetActive(true);
        //애니메이션 끝나고 갱신되라는뜻
        MUS.WritePlayerData();
        yield break;
    }
    public void PlayGacha()
    {
        if (GameManager.inst.PlayerInfo.Point < 2) return;
        gachaButton.GetComponent<Button>().enabled = false;//이버튼을 여러번 못누르게함
        GameManager.inst.PlayerInfo.AddPoint(-2);//2의차감을 꾀한다
        int value = Random.Range(0, 102);
        per = 0;
        //per/25한값이 결과
        StartCoroutine(BoxDown());
        
        
        result = value / 25;
        if (result < 0) result = 0;
        resultText.text = result.ToString() + "P";
        if (result >= 3) resultText.text += "!!";
        else if (result <= 1) resultText.text += "..";
        GameManager.inst.PlayerInfo.AddPoint(result);

    }
    public void OnDisable()
    {
        oneMoreButton.SetActive(false);
        gachaButton.GetComponent<Button>().enabled = true;
        gachaButton.transform.localPosition = buttonPos;
    }
}
