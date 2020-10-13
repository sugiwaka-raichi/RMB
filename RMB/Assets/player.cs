using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // MonobitView コンポーネント
    MonobitEngine.MonobitView m_MonobitView = null;

    string powertag = "powaer";

    bool havemonster = false;

    GameObject mymonsterobj;

    [SerializeField]
    int HP;

    //キー入力データ用変数
    private float inputHorizontal;
    private float inputVertical;

    Vector3 nowposition = Vector3.zero;
    Vector3 pastposition = Vector3.zero;
    Vector3 direction = Vector3.zero;
    Vector3 pastdirection = Vector3.zero;
    Vector3 difdirec = Vector3.zero;


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
        transform.position = new Vector3(transform.position.x + inputHorizontal * 0.2f, transform.position.y, transform.position.z + inputVertical * 0.2f);
        nowposition = transform.position;
        pastposition = nowposition;
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

        transform.position = new Vector3(transform.position.x + inputHorizontal*0.2f, transform.position.y, transform.position.z + inputVertical*0.2f);
        nowposition = transform.position;
        direction = nowposition - pastposition;
        difdirec = direction - pastdirection;
        if(direction.magnitude > 0.01f && difdirec.magnitude > 0.01f )
        {
            transform.rotation = Quaternion.LookRotation(direction);
            Debug.Log("ChangeRot");
        }

        pastposition = nowposition;
        pastdirection = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "monster")
        {
            if (!havemonster)
            {
                collision.transform.parent = this.gameObject.transform;
                mymonsterobj = collision.gameObject;
                havemonster = true;
            }
        }

        if(collision.transform.tag == powertag)
        {
            Damage(20);
        }
    }

    void Damage(int _damagevalue)
    {
        HP -= _damagevalue;
        if (HP < 0)
        {
            LoosePlayer();
        }
    }


    // 
    void LoosePlayer()
    {

    }

    
    // 攻撃
    void Atack()
    {
        if(havemonster)
        {
            //mymonsterobj.GetComponent<MonsterBase>();
        }
    }
}
