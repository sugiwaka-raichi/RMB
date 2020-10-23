using Monobit;
using MonobitEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonobitEngine.MonoBehaviour
{
    // MonobitView コンポーネント
    MonobitEngine.MonobitView m_MonobitView = null;

    // モンスターの技のタグ
    [SerializeField] string powertag = "";

    // モンスター所持フラグ
    bool havemonster = false;

    // 所持モンスターのゲームオブジェクトとスクリプト
    GameObject mymonsterobj;
    MonsterBase monscript;

    // プレイヤー体力
    [SerializeField]
    int HP;

    // 名前表示用テキスト
    [SerializeField]
    Text nametxt;

    // 自分判別用マテリアル
    [SerializeField]
    Material material;

    //水状態異常用プレハブ
    [SerializeField]
    GameObject WaterStatePref;

    //火状態異常用プレハブ
    [SerializeField]
    GameObject FireStatePref;

    //木状態異常用プレハブ
    [SerializeField]
    GameObject WoodStatePref;

    //水状態異常用ゲームオブジェクト
    GameObject WaterStateObj;

    //火状態異常用ゲームオブジェクト
    GameObject FireStateObj;

    //木状態異常用ゲームオブジェクト
    GameObject WoodStateObj;

    //キー入力データ用変数
    private float inputHorizontal;
    private float inputVertical;

    Vector3 nowposition = Vector3.zero;         // 現在位置
    Vector3 pastposition = Vector3.zero;        // 1フレーム前の位置
    Vector3 direction = Vector3.zero;           // 動いた方向
    Vector3 pastdirection = Vector3.zero;       // 1フレーム前の方向
    Vector3 difdirec = Vector3.zero;            // 動きの変化量

    Vector3 monsterpos = new Vector3(0.0f, 0.0f, 1.0f);

    // 状態異常の属性
    public enum ABNORMAL_CONDITION_TYOE
    {
        AC_FIRE,    // 火の状態異常
        AC_WATER,   // 水の状態異常
        AC_WOOD,    // 木の状態異常
        AC_NONE,    // 状態異常無し
    }

    // 状態異常
    ABNORMAL_CONDITION_TYOE atype = ABNORMAL_CONDITION_TYOE.AC_NONE;

    void Awake()
    {
        // すべての親オブジェクトに対して MonobitView コンポーネントを検索する
        if (GetComponentInParent<MonobitEngine.MonobitView>() != null)
        {
            m_MonobitView = GetComponentInParent<MonobitEngine.MonobitView>();
        }
        // 親オブジェクトに存在しない場合、すべての子オブジェクトに対して MonobitView コンポーネントを検索する
        else if (GetComponentInChildren<MonobitEngine.MonobitView>() != null)
        {
            m_MonobitView = GetComponentInChildren<MonobitEngine.MonobitView>();
        }
        // 親子オブジェクトに存在しない場合、自身のオブジェクトに対して MonobitView コンポーネントを検索して設定する
        else
        {
            m_MonobitView = GetComponent<MonobitEngine.MonobitView>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // オブジェクト所有権を所持しなければ実行しない
        if (!m_MonobitView.isMine)
        {
            return;
        }

        Renderer renderer = GetComponent<Renderer>();
        renderer.material = material;
        transform.position = new Vector3(transform.position.x + inputHorizontal * 0.1f, transform.position.y, transform.position.z + inputVertical * 0.1f);
        nowposition = transform.position;
        pastposition = nowposition;
        m_MonobitView.RPC("Damage", MonobitTargets.All, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // オブジェクト所有権を所持しなければ実行しない
        if (!m_MonobitView.isMine)
        {
            return;
        }

        //キー入力を取得
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        //if (KeyManager.GetKey("Right"))
        //{
        //    inputHorizontal = 1.0f;
        //}
        //if (KeyManager.GetKey("Left"))
        //{
        //    inputHorizontal = -1.0f;
        //}
        //if (KeyManager.GetKey("UP"))
        //{
        //    inputVertical = 1.0f;
        //}
        //if (KeyManager.GetKey("Down"))
        //{
        //    inputVertical = -1.0f;
        //}

        transform.position = new Vector3(transform.position.x + inputHorizontal * 0.1f, transform.position.y, transform.position.z + inputVertical * 0.1f);
        nowposition = transform.position;
        direction = nowposition - pastposition;
        difdirec = direction - pastdirection;
        if (direction.magnitude > 0.01f)
        {
            Debug.Log("足音再生 : " + SoundManager.PlaySE("プレイヤー/足音"));
            if (difdirec.magnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
                Debug.Log("ChangeRot");
            }
        }

        if (Input.GetButton("Fire1"))
        {
            Atack();
        }
        //if (KeyManager.GetKeyDown("SHot"))
        //{
        //    Atack();
        //}

        pastposition = nowposition;
        pastdirection = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!m_MonobitView.isMine)
        {
            return;
        }

        Debug.Log("collisionenter");
        if (collision.transform.tag == "Monster")
        {
            if (!havemonster && !collision.gameObject.GetComponent<MonsterBase>().GetCatchFlg())
            {
                m_MonobitView.RPC("MonsterGetFlgOn", MonobitTargets.All, m_MonobitView.viewID, collision.gameObject.GetComponent<MonobitView>().viewID);
            }
        }

        if (collision.transform.tag == powertag)
        {
            if (collision.gameObject.GetComponent<AttackBase>().GetShotPlayer() != NetworkManager.GetPlayer().ID)
            {
                if (atype == ABNORMAL_CONDITION_TYOE.AC_NONE)
                {
                    AttackBase.ATK_TYPE recAType = collision.gameObject.GetComponent<AttackBase>().GetType();
                    switch (recAType)
                    {
                        case AttackBase.ATK_TYPE.AT_FIRE:
                            atype = ABNORMAL_CONDITION_TYOE.AC_FIRE;
                            FireStateObj = MonobitNetwork.Instantiate(FireStatePref.name, transform.position, Quaternion.identity, 0, null, false, true, false);
                            m_MonobitView.RPC("StateOn", MonobitTargets.All, m_MonobitView.viewID, FireStateObj.GetComponent<MonobitView>().viewID);
                            break;
                        case AttackBase.ATK_TYPE.AT_WARTER:
                            atype = ABNORMAL_CONDITION_TYOE.AC_WATER;
                            WaterStateObj = MonobitNetwork.Instantiate(FireStatePref.name, transform.position, Quaternion.identity, 0, null, false, true, false);
                            m_MonobitView.RPC("StateOn", MonobitTargets.All, m_MonobitView.viewID, WaterStateObj.GetComponent<MonobitView>().viewID);
                            break;
                        case AttackBase.ATK_TYPE.AT_WOOD:
                            atype = ABNORMAL_CONDITION_TYOE.AC_WOOD;
                            WoodStateObj = MonobitNetwork.Instantiate(FireStatePref.name, transform.position, Quaternion.identity, 0, null, false, true, false);
                            m_MonobitView.RPC("StateOn", MonobitTargets.All, m_MonobitView.viewID, WoodStateObj.GetComponent<MonobitView>().viewID);
                            break;
                    }
                    Debug.Log(atype);
                }
                SoundManager.PlaySE("プレイヤー/当たり判定");
                m_MonobitView.RPC("Damage", MonobitTargets.All, 20);
                Debug.Log("Hit");
            }
        }
    }

    [MunRPC]
    // 被ダメージ処理
    void Damage(int _damagevalue)
    {
        HP -= _damagevalue;
        NetworkControl.customParams["HP"] = HP;
        NetworkManager.SetPlayerCustomParameters(NetworkControl.customParams);
        if (HP <= 0)
        {
            MonobitNetwork.Destroy(this.gameObject);
            if (FireStateObj != null)
            {
                MonobitNetwork.Destroy(FireStateObj);
            }
            if (WaterStateObj != null)
            {
                MonobitNetwork.Destroy(WaterStateObj);
            }
            if (WoodStateObj != null)
            {
                MonobitNetwork.Destroy(WoodStateObj);
            }
            LoosePlayer();
        }
        nametxt.text = HP.ToString();
    }

    [MunRPC]
    void MonsterGetFlgOn(int _monobitviewID, int _monsterviewID)
    {
        MonsterGet(_monobitviewID, _monsterviewID);
    }

    void MonsterGet(int _monobitviewID, int _monsterviewID)
    {
        GameObject[] playerobjs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerobj in playerobjs)
        {
            if (playerobj.GetComponent<MonobitView>().viewID == _monobitviewID)
            {
                GameObject[] monsterobjs = GameObject.FindGameObjectsWithTag("Monster");
                foreach (GameObject monsterobj in monsterobjs)
                {
                    if (monsterobj.GetComponent<MonobitView>().viewID == _monsterviewID)
                    {
                        Debug.Log(_monobitviewID);
                        mymonsterobj = monsterobj;
                        mymonsterobj.GetComponent<MonobitTransformView>().m_SyncPosition.m_EnableSync = false;
                        mymonsterobj.transform.SetParent(playerobj.transform);
                        mymonsterobj.transform.localPosition = monsterpos;
                        mymonsterobj.transform.rotation = playerobj.transform.rotation;
                        monscript = mymonsterobj.GetComponent<MonsterBase>();
                        monscript.SetCatchFlg(true);
                        monscript.SetPlayerID(NetworkManager.GetPlayer().ID);
                        havemonster = true;
                        SoundManager.PlaySE("プレイヤー/装備時");
                        break;
                    }
                }
                break;
            }
        }
    }

    [MunRPC]
    void StateOn(int _monobitviewID, int _stateviewID)
    {
        GameObject[] playerobjs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerobj in playerobjs)
        {
            if (playerobj.GetComponent<MonobitView>().viewID == _monobitviewID)
            {
                GameObject[] stateobjs = GameObject.FindGameObjectsWithTag("State");
                foreach (GameObject stateobj in stateobjs)
                {
                    if (stateobj.GetComponent<MonobitView>().viewID == _stateviewID)
                    {
                        stateobj.transform.parent = playerobj.transform;
                        break;
                    }
                }
                break;
            }
        }
    }

    // 
    void LoosePlayer()
    {

    }


    // 攻撃
    void Atack()
    {
        if (havemonster)
        {
            havemonster = false;
            monscript.Attack();
        }
    }
}
