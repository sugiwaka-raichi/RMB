using MonobitEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player : MonobitEngine.MonoBehaviour
{
    // MonobitView コンポーネント
    MonobitEngine.MonobitView m_MonobitView = null;

    [SerializeField] string powertag = "";

    bool havemonster = false;

    GameObject mymonsterobj;
    MonsterBase monscript;

    [SerializeField]
    int HP;

    [SerializeField]
    Text nametxt;

    [SerializeField]
    Material material;

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

        transform.position = new Vector3(transform.position.x + inputHorizontal*0.1f, transform.position.y, transform.position.z + inputVertical*0.1f);
        nowposition = transform.position;
        direction = nowposition - pastposition;
        difdirec = direction - pastdirection;
        if(direction.magnitude > 0.01f && difdirec.magnitude > 0.01f )
        {
            transform.rotation = Quaternion.LookRotation(direction);
            Debug.Log("ChangeRot");
        }

        if (Input.GetButton("Fire1"))
        {
            Atack();
        }

        pastposition = nowposition;
        pastdirection = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collisionenter");
        if(collision.transform.tag == "Monster")
        {
            if (!havemonster)
            {
                collision.transform.parent = this.gameObject.transform;
                mymonsterobj = collision.gameObject;
                mymonsterobj.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);
                mymonsterobj.transform.rotation = transform.rotation;
                monscript = mymonsterobj.GetComponent<MonsterBase>();
                monscript.SetCatchFlg(true);
                havemonster = true;
            }
        }

        if(collision.transform.tag == powertag)
        {
            Destroy(this.gameObject);
        }
    }

    // 被ダメージ処理
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
            monscript.Attack();
        }
    }
}
