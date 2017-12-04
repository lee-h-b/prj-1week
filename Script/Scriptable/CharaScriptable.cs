using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//상속으로 쓰지않음 캐릭은 사실상 이동하는 껍데기
//간단하게 db 이상으로 나아가진 않을거같고 캐릭터가 이걸 has하도록할듯

[CreateAssetMenu(menuName = "Create/DefaultChara")]
public class CharaScriptable : UnitStatSystem{
//    public Stat stat;
    //이걸 갖는 애가 최소 프로텍티드의 보안 수준을 지니고 애는 public수준으로 함
    [SerializeField]
    public int ID;//쓸런지? 어쩌면 잡코드로 전락함 // << 모델 호출할때 코드로 사용될듯

    [SerializeField]
    public string charaName;

    public Sprite image;//2Dimg값정도?
    public string description;//text는 한정적이여서 못했지만 여기라면 충분히 가능함

    //death를 체크하는건 여기가 아니라 캐릭터 자체가
    
    
}
