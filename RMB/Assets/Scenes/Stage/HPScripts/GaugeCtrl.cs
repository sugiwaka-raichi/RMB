using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeCtrl : MonoBehaviour
{
    // 最大値を設定
    private float myHP = 10.0f;
    // ゲージのオブジェクトを参照する変数
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        // ゲージの役割をさせるオブジェクト情報を取得
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            myHP-=2.0f;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            myHP = 50.0f;
        }

        image.fillAmount = myHP/10.0f;
    }
}
