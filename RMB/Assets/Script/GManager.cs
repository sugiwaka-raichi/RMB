using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;

public class GManager : MonobitEngine.MonoBehaviour
{
    /*============================= 変数宣言 =============================*/

    // GameManagerのインスタンスを静的確保、ゲッターとセッター
    public static GManager              GMInstance { get; private set; }

    private GameObject                  playerObject;       // プレイヤー格納用

    private MonobitPlayer[]             playerCount;        // プレイヤー情報格納用
    private static int                  playerNum;          // プレイヤー人数格納用

    // モンスターのPrefab情報を格納
    [SerializeField]
    private GameObject[]                monsterPrefab = null;
    // モンスターのPrefabと生成位置、回転などを格納する配列
    [SerializeField]
    private GameObject[]                monsterArray = null;

    // モンスターカウント用変数
    static private int                  monsterCount = 0;
    // モンスター上限
    private const int                   MONSTER_MAX = 20;
    // モンスターの最小数と最大数
    [SerializeField]
    private int                         monsterMin = 0, monsterMax = 3;
    private int                         monsterTemp;               // ランダム変数格納用
    private int                         monsterTempStrage;         // 情報格納用

    // モンスターのPrefab情報を格納
    [SerializeField]
    private GameObject                  unimonPrefab = null;
    // モンスターのPrefabと生成位置、回転などを格納する配列
    [SerializeField]
    private GameObject[]                unimonArray = null;

    // ユニモンカウント用変数
    static private int                  unimonCount = 0;
    // ユニモン最大数
    [SerializeField]
    private int                         UNIMON_MAX = 5;
    // ユニモンの最小数と最大数
    [SerializeField]
    private int                         unimonMin = 0, unimonMax = 1;       // 生成範囲指定
    private int                         unimonTemp;                 // ランダム変数格納用
    private int                         unimonTempStrage;           // 情報格納用

    private Vector3[]                   unimonPos;                  // ユニモン位置格納用

    // スポナーオブジェクトの設定
    [SerializeField]
    private GameObject[]                monsterSpawner = null;  // スポナーオブジェクト
    private Vector3[]                   spawnerPos = null;      // スポナーオブジェクトの位置
    private int                         spawnerNum;             // スポナーの数
    private int                         spawnerTemp;            // ランダム変数格納用
    private int                         spawnerTempStrage;      // 情報格納用
    // MonsterSpawnerのスケール格納
    private float                       xMinScale, xMaxScale;
    private float                       yMaxScale;
    private float                       zMinScale, zMaxScale;

    // ランダム生成で使用
    // 生成するモンスターのｘ座標の幅(最小値、最大値)
    [Header("Set X Position Min and Max")]
    [SerializeField]
    private float                       xMinPos = -10.0f;
    private float                       xMaxPos = 10.0f;
    // 生成するモンスターのｙ座標設定(ｙはランダムじゃなくていい)
    private float                       yMaxPos = 0.5f;
    // 生成するモンスターのｚ座標の幅(最小値、最大値)
    [Header("Set Z Position Min and Max")]
    [SerializeField]
    private float                       zMinPos = -10.0f;
    private float                       zMaxPos = 10.0f;

    // モンスタータイマ
    [SerializeField]
    private float                       monsterGenerateTime = 5;        // 生成のディレイタイム指定
    private float                       monsterElapsedTime;             // 経過時間カウント用

    // ユニモンタイマ
    [SerializeField]
    private float                       unimonGenerateTime = 10;        // 生成のディレイタイム指定
    private float                       unimonElapsedTime;              // 経過時間カウント用

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

    /*============================= Start =============================*/
    protected void Start()
    {
        // インスタンスがある場合の
        if(GManager.GMInstance != null)
        {
        }
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
                    playerpos = new Vector3(-32.0f, 1.0f, 22.0f);
                    break;
                case 2:
                    playerpos = new Vector3(0.0f, 1.0f, 23.5f);
                    break;
                case 3:
                    playerpos = new Vector3(32.0f, 1.0f, 22.0f);
                    break;
                case 4:
                    playerpos = new Vector3(-32.0f, 1.0f, 0.0f);
                    break;
                case 5:
                    playerpos = new Vector3(32.0f, 1.0f, 0.0f);
                    break;
                case 6:
                    playerpos = new Vector3(-32.0f, 1.0f, -22.0f);
                    break;
                case 7:
                    playerpos = new Vector3(0.0f, 1.0f, -23.5f);
                    break;
                case 8:
                    playerpos = new Vector3(32.0f, 1.0f, -22.0f);
                    break;
            }

            playerObject = MonobitNetwork.Instantiate(
                            "Player",
                            playerpos,
                            Quaternion.identity,
                            0, 
                            null, 
                            false, 
                            true, 
                            false);
        }
        monsterCount = 0;
        unimonCount = 0;

        // ルーム内のプレイヤー情報をplayerCount内に保存
        playerCount = GetPlayerNum();

        // ルーム内のプレイヤー人数をplayerNumに保存
        playerNum = playerCount.Length;

        // スポナーオブジェクトの数を格納
        for(int i = 0; i < monsterSpawner.Length; i++)
        {
            spawnerNum++;
        }
        
        // スポナーオブジェクトの箱を生成(箱無しだとエラーになるので注意)
        spawnerPos = new Vector3[spawnerNum];

        unimonPos = new Vector3[UNIMON_MAX];
    }

    /*============================= Update =============================*/
    protected void Update()
    {
        // ホストじゃなければ返す
        if (!MonobitNetwork.isHost)
        {
            return;
        }

        CreateMonster();
        CreateUniqueMonster();
    }

    /*============================= CreateMonster =============================*/
    // モンスター生成の処理
    private void CreateMonster()
    {
       if (monsterElapsedTime >= monsterGenerateTime)          // timeGenerateに指定した秒数に達すれば生成
       {
            // 一種類のみランダム発生(連続して同じものが発生しない)
            // モンスターを上限まで生成しているかチェック
            if (monsterCount < MONSTER_MAX)
            {
                while (true)
                {
                    // モンスターの生成する範囲
                    monsterTemp = Random.Range(monsterMin, monsterMax);

                    // 連続して出ないようにする
                    if (monsterTemp != monsterTempStrage)
                    {
                        // 生成処理
                        monsterArray[monsterTemp] = MonobitNetwork.Instantiate(monsterPrefab[monsterTemp].name, SetSpawnerPosition(), Quaternion.identity, 0, null, false,false, true) as GameObject;

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
        for (int i = 0; i < UNIMON_MAX; i++)
        {
            if (unimonArray[i] == null)
            {
                // 生成位置割り振り
                switch (i)
                {
                    case 0:
                        unimonPos[i] = new Vector3(-15.0f, 0.5f, 8.5f);
                        break;

                    case 1:
                        unimonPos[i] = new Vector3(13.0f, 0.5f, 10.5f);
                        break;

                    case 2:
                        unimonPos[i] = new Vector3(-13.0f, 0.5f, -10.5f);
                        break;

                    case 3:
                        unimonPos[i] = new Vector3(15.0f, 0.5f, -8.5f);
                        break;

                    case 4:
                        unimonPos[i] = new Vector3(0.0f, 0.0f, 0.0f);
                        break;

                    default:
                        break;
                }
                // 生成処理
                unimonArray[i] = MonobitNetwork.Instantiate(unimonPrefab.name, unimonPos[i], Quaternion.identity, 0, null, false, false, true) as GameObject;
            }
        }
    }

    /*============================= SetSpawnerPosition =============================*/
    private Vector3 SetSpawnerPosition()
    {
        // 各スポナーオブジェクトの原点位置を設定
        for (int i = 0; i < spawnerNum; i++)
        {
            spawnerPos[i] = monsterSpawner[i].transform.position;
        }

        while (true)
        {
            // spawnerNumの値を0～spawnerNumの範囲でランダム取得
            spawnerTemp = Random.Range(0, spawnerNum);

            // 前回生成位置とかぶらないようにする
            if (spawnerTemp != spawnerTempStrage)
            {
                // 違ったら次の処理へ
                break;
            }
            else
            {
                // かぶったら無限に回す
                continue;
            }
        }

        // 対応する番号で各自の大きさを計算して代入
        switch (spawnerTemp)
        {
            case 0:
                xMinScale = spawnerPos[spawnerTemp].x / 5;
                zMinScale = spawnerPos[spawnerTemp].z / 5;

                xMaxScale = spawnerPos[spawnerTemp].x * 2;
                zMaxScale = spawnerPos[spawnerTemp].z * 2;
                break;

            case 1:
                xMinScale = spawnerPos[spawnerTemp].x / 5;
                zMinScale = spawnerPos[spawnerTemp].z / 5;

                xMaxScale = spawnerPos[spawnerTemp].x * 2;
                zMaxScale = spawnerPos[spawnerTemp].z * 2;
                break;

            case 2:
                xMinScale = spawnerPos[spawnerTemp].x / 5;
                zMinScale = spawnerPos[spawnerTemp].z / 5;

                xMaxScale = spawnerPos[spawnerTemp].x * 2;
                zMaxScale = spawnerPos[spawnerTemp].z * 2;
                break;

            case 3:
                xMinScale = spawnerPos[spawnerTemp].x / 5;
                zMinScale = spawnerPos[spawnerTemp].z / 5;

                xMaxScale = spawnerPos[spawnerTemp].x * 2;
                zMaxScale = spawnerPos[spawnerTemp].z * 2;
                break;

            case 4:
                xMinScale = spawnerPos[spawnerTemp].x / 5;
                zMinScale = spawnerPos[spawnerTemp].z / 5;

                xMaxScale = spawnerPos[spawnerTemp].x * 2;
                zMaxScale = spawnerPos[spawnerTemp].z * 2;
                break;

            case 5:
                xMinScale = spawnerPos[spawnerTemp].x / 5;
                zMinScale = spawnerPos[spawnerTemp].z / 5;

                xMaxScale = spawnerPos[spawnerTemp].x * 2;
                zMaxScale = spawnerPos[spawnerTemp].z * 2;
                break;

            case 6:
                xMinScale = spawnerPos[spawnerTemp].x / 5;
                zMinScale = spawnerPos[spawnerTemp].z / 5;

                xMaxScale = spawnerPos[spawnerTemp].x * 2;
                zMaxScale = spawnerPos[spawnerTemp].z * 2;
                break;

            case 7:
                xMinScale = spawnerPos[spawnerTemp].x / 5;
                zMinScale = spawnerPos[spawnerTemp].z / 5;

                xMaxScale = spawnerPos[spawnerTemp].x * 2;
                zMaxScale = spawnerPos[spawnerTemp].z * 2;
                break;
        }

        // 前回値を保存する
        spawnerTempStrage = spawnerTemp;

        // 範囲内のランダムな生成位置をｘ、ｙ、ｚに格納
        float x = Random.Range(xMinScale, xMaxScale);
        float y = 0.5f;
        float z = Random.Range(zMinScale, zMaxScale);

        // Vector3型のPositionを返す
        return new Vector3(x, y, z);
    }

    /*============================= CountdownMonster =============================*/
    // モンスターカウントを減らす処理
    public static void CountdownMonster()
    {
        monsterCount--;
        Debug.Log("CountdownMonster" + monsterCount);
    }

    /*============================= CountdownUniqueMonster =============================*/
    // ユニモンカウントを減らす処理
    public static void CountdownUnimon()
    {
        unimonCount--;
    }

    /*============================= GetPlayerInfo =============================*/
    // 一人のプレイヤーを参照する関数
    public static MonobitPlayer GetPlayerInfo()
    {
        return NetworkManager.GetPlayer();
    }

    /*============================= GetPlayerStatus =============================*/
    // ルーム内のプレイヤーの人数を参照する関数
    public static MonobitPlayer[] GetPlayerNum()
    {
        return NetworkManager.GetPlayerList();
    }

    /*============================= SendPlayerDeath =============================*/
    // プレイヤーのデスによる人数変動の送信処理
    public void SendPlayerDeath()
    {
        ResultShow.ResultActive(playerNum, playerCount.Length);
        monobitView.RPC("ReceivePlayerDeath", MonobitTargets.All, 1);
    }

    /*============================= ReceivePlayerDeath =============================*/
    //  プレイヤーのデスによる人数変動の受信処理
    [MunRPC]
    public void ReceivePlayerDeath(int _playerNum)
    {
        playerNum -= _playerNum;
    }

    /*============================= ReferPlayerNum =============================*/
    // Aliveスクリプトと連携
    public int ReferPlayerNum()
    {
        return playerNum;
    }
}