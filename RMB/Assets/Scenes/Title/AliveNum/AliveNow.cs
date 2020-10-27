using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MonobitEngine;

public class AliveNow : MonobitEngine.MonoBehaviour
{
    //public int aliveNow = 0;    // 残存数（デバッグ用）

    private int  alivePlayer;   // ゲームマネージャから参照した残存数表示用

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
        alivePlayer = GManager.ReferPlayerNum();
        // オブジェクトからTextコンポーネントを取得
        this.GetComponent<Text>().text = "プレイヤー残存数：" + alivePlayer; // GameManagerから値を引き取って反映する。
    }
}
