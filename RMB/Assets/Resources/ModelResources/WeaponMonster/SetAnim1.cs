using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnim1 : MonoBehaviour
{
    private Animator animatorManage;    // Animator用変数

    // Start is called before the first frame update
    void Start()
    {
        // Animatorコンポーネントを取得
        animatorManage = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /** デバッグ用.*/
        if (Input.GetKeyDown("space"))  // 「Space」を押すと歩くモーションへ
        {
            animatorManage.SetBool("isWalk", true);
            animatorManage.SetBool("isStop", false);


        }
        if (Input.GetKeyDown("a"))     // 「a」を押すとアイドルモーションへ
        {
            animatorManage.SetBool("isWalk", false);
            animatorManage.SetBool("isStop", true);

        }
    }
}
