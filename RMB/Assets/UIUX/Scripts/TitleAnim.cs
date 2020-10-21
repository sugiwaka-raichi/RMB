using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnim : MonoBehaviour
{
    private bool animFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        animFlag = false;

        // 初期値設定
        // transformを取得
        Transform btnTrans = this.transform;
        // 座標を取得
        Vector3 btnPos = btnTrans.localPosition;

        btnPos.x = 350.0f;
        btnPos.y = -200.0f;

        btnTrans.localPosition = btnPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (animFlag)
        {
            // transformを取得
            Transform btnTrans = this.transform;

            // 座標を取得
            Vector3 btnPos = btnTrans.localPosition;

            btnPos.y += 10.0f;
            if (btnPos.y > 700.0f)
            {
                btnPos.y = 700.0f;
                animFlag = false;
            }

            // 座標を設定
            btnTrans.localPosition = btnPos;
        }
    }

    // ボタンが押されたときにアニメーションフラグをON
    // 他のスクリプトへ反映させるためのゲッタープロパティを実行
    public void OnFlg()
    {
        animFlag = true;
        Debug.Log(animFlag);
    }

    // ゲッター
    public bool GetFlg()
    {
        return animFlag;
    }
}
