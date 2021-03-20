using System;
using Main.Domain.Entities;
using Main.Presentation;
using Scripts.Main.Domain.Entities;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Main.Presentation.View
{
    public sealed class ScenePresenter
    {
        private static ScenePresenter _instance = new ScenePresenter();
        private readonly IObservable<Scenes> _sceneManagerStream;

        private ScenePresenter()
        {
            var sceneManager = SceneController.GetInstance();
            _sceneManagerStream = sceneManager.GetScenesStream();
        }

        /// <summary>
        ///     テスト用コンストラクタ
        /// </summary>
        /// <param name="sceneController"></param>
        public ScenePresenter(SceneController sceneController)
        {
            _sceneManagerStream = sceneController.GetScenesStream();
        }

        public static ScenePresenter GetInstance()
        {
            return _instance;
        }

        public void Init()
        {
            _sceneManagerStream.Subscribe(
                scenes =>
                {
                    Debug.Log(this + " showScene: " + scenes);
                    switch (scenes)
                    {
                        case Scenes.HomeMenu:
                            break;
                        case Scenes.MainMenuSetting:
                            break;
                        case Scenes.OfflinePlaySetting:
                            SceneManager.LoadScene("PlaySetting");
                            break;
                        case Scenes.OfflinePlay:
                            break;
                        case Scenes.PlayFinish:
                            break;
                        case Scenes.OnlineMatching:
                            break;
                        case Scenes.OnlinePlayView:
                            break;
                        case Scenes.OnlineFinish:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(scenes), scenes, null);
                    }
                }
            );
        }
    }
}
