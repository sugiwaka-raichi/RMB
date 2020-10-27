using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    bool startMusic = false;

    // Start is called before the first frame update
    void Start()
    { 
        
    }

    private void Update()
    {
        if (!startMusic && ManageSceneLoader.GetActiveScene() != ManageSceneLoader.SceneType.NetScene.ToString())
        {
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
            startMusic = true;
        }
    }

}
