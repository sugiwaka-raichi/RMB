using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AliveNow : MonoBehaviour
{
    //public int aliveNow = 0;    // 残存数（デバッグ用）

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /** デバッグ用.*/
        //if (aliveNow > 0)
        //{
        //    NowAlive(aliveNow);

        //}

    }


    // 残存数の反映処理
    public void NowAlive()    
    {
        // オブジェクトからTextコンポーネントを取得
        this.GetComponent<Text>().text = "プレイヤー残存数："; // GameManagerから値を引き取って反映する。
    }
}
