using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutScript : MonoBehaviour
{
    [SerializeField]
    private float fadeOutTime;      //フェード間隔
    [SerializeField]
    private Image image;            //フェードに使うimage

    private static bool fadeflg = false;    //フェードアウト中かどうかのフラグ

    // Start is called before the first frame update
    void Start()
    {
        image.gameObject.SetActive(true);       //暗幕をON
        fadeOutTime = fadeOutTime / 10f;        //フェードアウト時間計算
        StartCoroutine(FadeOut());              //コルーチン呼び出し
        fadeflg = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeOut()
    {
        //Colorのアルファを0.1ずつあげていく
        for (float i = 0.0f;image.color.a <= 1.0f;i += 0.1f)
        {
            image.color = new Color(0f, 0f, 0f, i);     //変数iをもとにアルファ値を変更 (0~1)
            Debug.Log(i);

            //フェードアウト終了時
            if (i >= 1.0f)
            {
                fadeflg = false;
                yield return null;
            }
            //待機
            fadeflg = true;
            yield return new WaitForSeconds(fadeOutTime);
        }
    }

    //==========================================
    //フェード状態のフラグ
    //==========================================
    public static bool GetFadeFlag()
    {
        return fadeflg;
    }
}
