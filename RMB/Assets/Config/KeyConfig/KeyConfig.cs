using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KeyConfig : MonoBehaviour
{
    const KeyCode release = KeyCode.Escape;     //入力処理中止の際使うキーを設定

    [SerializeField]
    private Text keyText;       //設定されているキーを表示
    bool inputFlg = false;      //入力待ちかどうかを判断する
    string keyName;             //登録するkeyの名前

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Keyの変更
        if (inputFlg)
        {
            //キーを取得
            KeyCode keyCode = KeyManager.KeyInput();
            if (keyCode == 0)
            {
                //入力されていなければ何もせず返す
                return;
            }
            else if(keyCode == release)
            {
                //入力待ちを解除するキーだった場合
                inputFlg = false;       //入力待ちをやめる
                return;
            }
            else
            {
                //入力されていれば以下
                Debug.Log("入力されたキー:" + keyCode);
                KeyManager.SetKey(keyName, keyCode);    //入力されたキーを設定する

                keyText.text = keyCode.ToString();      //入力されたキーを表示する
                inputFlg = false;                       //入力待ちを解除する
                
            }
        }
    }

    //=======================================================
    //入力待ちの開始
    //=======================================================
    public void StartKeyInput(string _keyName)
    {
        inputFlg = true;

        keyName = _keyName;     //扱うkeyを渡す
        //keynameが登録されていなければ登録する
        if (KeyManager.CheckKeyType(keyName))
        {
            //登録されている
        }
        else
        {
            //されていないので登録する
            KeyManager.AddKey(keyName);
        }
    }
}
