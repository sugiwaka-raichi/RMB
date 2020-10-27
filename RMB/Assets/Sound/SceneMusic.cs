using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    { 
        
    }

    private void Update()
    {
        //現在のアクティブシーンがNetScene出ないときに処理を行う
        if (ManageSceneLoader.GetActiveScene() != ManageSceneLoader.SceneType.NetScene.ToString())
        {
            //現在のシーンタイプを取得
            switch (ManageSceneLoader.GetSceneType())
            {
                case ManageSceneLoader.SceneType.TitleScene:
                    SoundManager.PlayMusic(SoundData.MUSIC_LIST.Title.ToString());
                    break;
                case ManageSceneLoader.SceneType.LobbyScene:
                    SoundManager.PlayMusic(SoundData.MUSIC_LIST.Lobby.ToString());
                    break;
                case ManageSceneLoader.SceneType.StageScene:
                    SoundManager.PlayMusic(SoundData.MUSIC_LIST.Stage.ToString());
                    break;
            }
            Destroy(this);          //二度目は再ロードされるまでないので削除
        }
    }

}
