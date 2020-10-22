using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInScript : MonoBehaviour
{
    [SerializeField]
    private float fadeInTime;

    //背景Image
    [SerializeField]
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("StartFadeIN");
        //コルーチンで使用する待ち時間を計測
        fadeInTime = fadeInTime / 10.0f;
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {
        //Colorのアルファを0.1ずつ下げていく
        for(float i = 1.0f;i >= 0;i -= 0.1f)
        {
            //透明度変更
            image.color = new Color(0f, 0f, 0f, i);
            Debug.Log("in color:" + i);

            if (image.color.a >= 0.0f)
            {
                Debug.Log("Fadetime:" + fadeInTime);
                //指定秒数待つ
                yield return new WaitForSeconds(fadeInTime);

            }
            yield return null;
        }
        Debug.Log("ループ処理抜け");
        image.gameObject.SetActive(false);      //オブジェクトを無効にする
        //FadeOutScript.fadeflag = false;
    }
}
