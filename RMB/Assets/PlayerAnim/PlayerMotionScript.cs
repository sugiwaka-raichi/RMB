using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionScript : MonoBehaviour
{
    private Animator playerAnimator;    // プレイヤーアニメーション用変数

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();  // コンポーネント取得
    }

    // Update is called once per frame
    void Update()
    {

        // 走る（未装備時）,長押しに設定
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerAnimator.SetBool("toRun", true);
            playerAnimator.SetBool("toIdle", false);
            playerAnimator.SetBool("toEpicRun", false);
            
        }
        // 走る（モンスター装備時）,長押しに設定
        else if (Input.GetKeyDown(KeyCode.E))
        {
            playerAnimator.SetBool("toEpicRun", true);
            playerAnimator.SetBool("toRun", false);
            playerAnimator.SetBool("toIdle", false);
            
           
        }
        // アイドル（キーが押されていない時）
        else
        {
            playerAnimator.SetBool("toIdle", true);
            playerAnimator.SetBool("toRun", false);
            playerAnimator.SetBool("toEpicRun", false);

        }
    }
}
