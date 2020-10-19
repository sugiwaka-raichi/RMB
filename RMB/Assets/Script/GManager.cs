using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;

public class GManager : MonobitEngine.MonoBehaviour
{
    /*============================= 変数宣言 =============================*/

    // GameManagerのインスタンスを静的確保、ゲッターとセッター
    public static GManager GMInstance { get; private set; }

    private GameObject playerObject;

    // モンスターのPrefab情報を格納
    [SerializeField] private GameObject[] monsterPrefab = null;
    // モンスターのPrefabと生成位置、回転などを格納する配列
    [SerializeField] private GameObject[] monsterArray = null;

    // モンスターカウント用変数
    static private int monsterCount = 0;
    // モンスター上限
    private const int MONSTER_MAX = 20;
    // モンスターの最小数と最大数
    [SerializeField] private int monsterMin = 0, monsterMax = 3;
    private int Temp;               // ランダム変数格納用
    private int TempStrage;         // 情報格納用

    // 生成するモンスターのｘ座標の幅(最小値、最大値)
    [Header("Set X Position Min and Max")]
    [SerializeField] private float xMinPos = -10.0f, xMaxPos = 10.0f;

    // 生成するモンスターのｙ座標設定(ｙはランダムじゃなくていい)
    [Header("Set Y Position Max")]
    [SerializeField] private float yMaxPos = 10.0f;

    // 生成するモンスターのｚ座標の幅(最小値、最大値)
    [Header("Set Z Position Min and Max")]
    [SerializeField] private float zMinPos = -10.0f, zMaxPos = 10.0f;

    // タイマ
    [SerializeField] private float timeGenerate = 5;       // 生成のディレイタイム指定
    private float timeElapsedGenerate;                      // 経過時間カウント用

    // プレイヤー人数情報格納用
    public int playerNum;

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
            ManageSceneLoader.SceneMoveObject(playerObject, ManageSceneLoader.SceneType.StageScene);
        }
        monsterCount = 0;
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

        // ゲーム中のプレイヤーの状態を取得する処理
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
       if (timeElapsedGenerate >= timeGenerate)          // timeGenerateに指定した秒数に達すれば生成
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
                    Temp = Random.Range(monsterMin, monsterMax);
                    if (Temp != TempStrage)
                    {
                        monsterArray[Temp] = MonobitNetwork.Instantiate(monsterPrefab[Temp].name, GetRandomPosition(), Quaternion.identity, 0, null, false,false, true) as GameObject;

                        // Monsterの生成シーンをNetSceneからStageSceneへ移動
                        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("StageScene");
                        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(monsterArray[Temp], scene);
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
            TempStrage = Temp;      // 直前のモンスターの情報(番号)を格納
            timeElapsedGenerate = 0.0f;             // 経過時間リセット
       }
        // 生成タイマ
        timeElapsedGenerate += Time.deltaTime;      // 1秒ずつ加算
    }

    /*============================= DestroyedMonster =============================*/
    // モンスターカウントを減らす処理
    static void DestroyedMonster()
    {
        monsterCount--;
    }

    /*============================= GetPlayerStatus =============================*/
    // プレイヤーの状態を取得、ほかの処理に使用できるようにする
    protected void GetPlayerNumFromGM()
    {
        
    }

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