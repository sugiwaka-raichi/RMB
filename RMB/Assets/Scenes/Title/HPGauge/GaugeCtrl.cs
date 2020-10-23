//----------------------------------------------------------------------------
// @filename ダメージコントロールメソッド
// @filenote ダメージゲージのコントロール及び死亡後のメソッドを確立
// @creator  山本正
//----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeCtrl : MonoBehaviour
{
    // 最大HPを設定
    [SerializeField]
    float maxHP = 100.0f;         // HP最大量

    // 保持HPを設定
    [SerializeField]
    private float myHP = 0.0f;   // 保持HP変数

    // ゲージのオブジェクトを参照する変数
    private Image image;


    // Start is called before the first frame update
    void Start()
    {
        // ゲージの役割をさせるオブジェクト情報を取得
        image = this.GetComponent<Image>();

        // HP量初期化
        myHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ダメージコントロール（ダメージ数値はint型）
    public void DamageCtrl(int damage)
    {
        // HP数値が残っている間動作
        if (image.fillAmount > 0)
        {
            myHP -= damage;                     // ダメージ数値分減らす
            image.fillAmount = myHP / maxHP;   // ダメージ量を反映（最適化）

            Debug.Log($"{damage}ダメージ！");
        }
    }
}
