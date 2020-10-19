using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;

public class GManager : MonobitEngine.MonoBehaviour
{
    /*============================= 変数宣言 =============================*/

    // GameManagerのインスタンスを静的確保、ゲッターとセッター
    public static GManager  GMInstance { get; private set; }

    private GameObject      playerObject;

    // モンスターのPrefab情報を格納
    [SerializeField]
    private GameObject[]    monsterPrefab = null;
    // モンスターのPrefabと生成位置、回転などを格納する配列
    [SerializeField]
    private GameObject[]    monsterArray = null;

    // モンスターカウント用変数
    static private int      monsterCount = 0;
    // モンスター上限
    private const int       MONSTER_MAX = 20;
    // モンスターの最小数と最大数
    [SerializeField]
    private int             monsterMin = 0, monsterMax = 3;
    private int             monsterTemp;               // ランダム変数格納用
    private int             monsterTempStrage;         // 情報格納用

    // モンスターのPrefab情報を格納
    [SerializeField]
    private GameObject[]    unimonPrefab = null;
    // モンスターのPrefabと生成位置、回転などを格納する配列
    [SerializeField]
    private GameObject[]    unimonArray = null;

    // ユニモンカウント用変数
    static private int      unimonCount = 0;
    // ユニモン上限
    private const int       UNIMON_MAX = 5;
    // ユニモンの最小数と最大数
    [SerializeField]
    private int             unimonMin = 0, unimonMax = 2;
    private int             unimonTemp;               // ランダム変数格納用
    private int             unimonTempStrage;         // 情報格納用

    // 生成するモンスターのｘ座標の幅(最小値、最大値)
    [Header("Set X Position Min and Max")]
    [SerializeField]
    private float           xMinPos = -10.0f, xMaxPos = 10.0f;

    // 生成するモンスターのｙ座標設定(ｙはランダムじゃなくていい)
    [Header("Set Y Position Max")]
    [SerializeField]
    private float           yMaxPos = 10.0f;

    // 生成するモンスターのｚ座標の幅(最小値、最大値)
    [Header("Set Z Position Min and Max")]
    [SerializeField]
    private float           zMinPos = -10.0f, zMaxPos = 10.0f;

    // モンスタータイマ
    [SerializeField]
    private float           monsterGenerateTime = 5;       // 生成のディレイタイム指定
    private float           monsterElapsedTime;    // 経過時間カウント用

    // ユニモンタイマ
    [SerializeField]
    private float           unimonGenerateTime = 10;       // 生成のディレイタイム指定
    private float           unimonElapsedTime;    // 経過時間カウント用

    /*============================= Awake =============================*/
    // Awake：インスタンス化直後に呼ばれる(Startより先に呼ばれる)
    private void Awake()
    {
        // シングルトン
        if (GMInstance == null)
        {
            GMInstance = this;
//            DontDestroyOnLoad(this.gameObject);     // シーンを移動してもオブジェクト破棄されない
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // スタート
    /*============================= Start =============================*/
    protected void Start()
    {
        // スタートの各処理前にアクティブシーンをステージシーンに切り替え
        if(ManageSceneLoader.GetActiveScene() == "NetScene")
        {
            ManageSceneLoader.SetActiveScene(ManageSceneLoader.SceneType.StageScene);
        }

        // プレイヤーキャラクタが未搭乗の場合に登場させる
        if (playerObject == null)
        {
            Vector3 playerpos = Vector3.zero;
            switch (MonobitNetwork.player.ID)
            {
                case 1:
                    playerpos = new Vector3(-10.0f, 1.0f, 10.0f);
                    break;
                case 2:
                    playerpos = new Vector3(10.0f, 1.0f, -10.0f);
                    break;
                case 3:
                    playerpos = new Vector3(10.0f, 1.0f, 10.0f);
                    break;
                case 4:
                    playerpos = new Vector3(-10.0f, 1.0f, -10.0f);
                    break;
                case 5:
                    playerpos = new Vector3(-5.0f, 1.0f, 5.0f);
                    break;
                case 6:
                    playerpos = new Vector3(5.0f, 1.0f, -5.0f);
                    break;
                case 7:
                    playerpos = new Vector3(5.0f, 1.0f, 5.0f);
                    break;
                case 8:
                    playerpos = new Vector3(5.0f, 1.0f, -5.0f);
                    break;
                case 9:
                    playerpos = new Vector3(-2.0f, 1.0f, 0.0f);
                    break;
                case 10:
                    playerpos = new Vector3(2.0f, 1.0f, 0.0f);
                    break;
            }

            playerObject = MonobitNetwork.Instantiate(
                            "Player",
                            playerpos,
                            Quaternion.identity,
                            0, 
                            null, 
                            false, 
                            false, 
                            true);
        }
        monsterCount = 0;
        unimonCount = 0;
    }

    /*============================= Update =============================*/
    protected void Update()
    {
        // ホストじゃなければ返す
        if (!MonobitNetwork.isHost)
        {
            return;
        }

        // ただしぃのシーン完成次第置き換え
//        ChangeScene();          // シーン遷移
        CreateMonster();
        CreateUniqueMonster();
    }

    /*============================= SceneChange =============================*/
    private void ChangeScene()
    {
        // 最初に今のゲームシーンを取得(Getでもらうと楽)
        var sceneType = (ManageSceneLoader.SceneType)System.Enum.ToObject(typeof(ManageSceneLoader.SceneType), ManageSceneLoader.GetActiveScene());

        // タイトルからロビー
        if (sceneType == ManageSceneLoader.SceneType.TitleScene && Input.GetKey(KeyCode.Space))
        {
            ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.LobbyScene);
        }

        // ロビーからステージ
        else if (sceneType == ManageSceneLoader.SceneType.LobbyScene && Input.GetKey(KeyCode.Space))
        {
            ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.StageScene);
        }

        // ステージからロビー
        else if (sceneType == ManageSceneLoader.SceneType.StageScene && Input.GetKey(KeyCode.Space))
        {
            ManageSceneLoader.SceneChange(ManageSceneLoader.SceneType.LobbyScene);
        }
    }

    /*============================= CreateMonster =============================*/
    // モンスター生成の処理
    private void CreateMonster()
    {
       if (monsterElapsedTime >= monsterGenerateTime)          // timeGenerateに指定した秒数に達すれば生成
       {
           // 三体同時生成
           // monsterArray配列の長さを調べて格納
           //for (int i = 0; i < monsterArray.Length; i++)
           //{
           //    // モンスターを上限まで生成
           //    if (monsterCount < MONSTER_MAX)
           //    {
           //        monsterArray[i] = MonobitNetwork.Instantiate(monsterPrefab[i].name, GetRandomPosition(), Quaternion.identity, 0) as GameObject;
           //        monsterCount++;     // モンスターの数加算
           //    }
           //    else
           //    {
           //        break;
           //    }
           //}

            // 一種類のみランダム発生(連続して同じものが発生しない)
            // モンスターを上限まで生成しているかチェック
            if (monsterCount < MONSTER_MAX)
            {
                while (true)
                {
                    monsterTemp = Random.Range(monsterMin, monsterMax);
                    if (monsterTemp != monsterTempStrage)
                    {
                        monsterArray[monsterTemp] = MonobitNetwork.Instantiate(monsterPrefab[monsterTemp].name, GetRandomPosition(), Quaternion.identity, 0, null, false,false, true) as GameObject;

                        monsterCount++;     // モンスターの数加算
                        Debug.Log("MonsterCreated" + monsterCount);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            monsterTempStrage = monsterTemp;      // 直前のモンスターの情報(番号)を格納
            monsterElapsedTime = 0.0f;            // 経過時間リセット
       }
        // 生成タイマ
        monsterElapsedTime += Time.deltaTime;      // 1秒ずつ加算
    }

    /*============================= CreateUniqueMonster =============================*/
    // ユニークモンスター生成
    private void CreateUniqueMonster()
    {
        if (unimonElapsedTime >= unimonGenerateTime)          // timeGenerateに指定した秒数に達すれば生成
        {
            // 一種類のみランダム発生(連続して同じものが発生しない)
            // モンスターを上限まで生成しているかチェック
            if (unimonCount < UNIMON_MAX)
            {
                while (true)
                {
                    unimonTemp = Random.Range(unimonMin, unimonMax);
                    if (unimonTemp != unimonTempStrage)
                    {
                        unimonArray[unimonTemp] = MonobitNetwork.Instantiate(unimonPrefab[unimonTemp].name, GetRandomPosition(), Quaternion.identity, 0, null, false, false, true) as GameObject;

                        unimonCount++;     // モンスターの数加算
                        Debug.Log("UnimonCreated" + unimonCount);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            unimonTempStrage = unimonTemp;      // 直前のモンスターの情報(番号)を格納
            unimonElapsedTime = 0.0f;             // 経過時間リセット
        }
        // 生成タイマ
        unimonElapsedTime += Time.deltaTime;      // 1秒ずつ加算
    }

    /*============================= DestroyedMonster =============================*/
    // モンスターカウントを減らす処理
    static void CountdownMonster()
    {
        monsterCount--;
    }

    /*============================= DestroyedUniqueMonster =============================*/
    // ユニモンカウントを減らす処理
    static void CountdownUnimon()
    {
        unimonCount--;
    }
    /*============================= GetPlayerStatus =============================*/
    // プレイヤーの状態を取得、ほかの処理に使用できるようにする

    /*============================= GetRandomPosition =============================*/
    //ランダムな位置を生成する関数
    private Vector3 GetRandomPosition()
    {
        //それぞれの座標をランダムに生成する
        float x = Random.Range(xMinPos, xMaxPos);
        float y = yMaxPos;
        float z = Random.Range(zMinPos, zMaxPos);

        //Vector3型のPositionを返す
        return new Vector3(x, y, z);
    }
}