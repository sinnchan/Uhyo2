using System;
using Scripts.Main.Domain.Entities;
using UniRx;

namespace Scripts.Main.Presentation
{
    public sealed class SceneController
    {
        private static SceneController _sceneController = new SceneController();

        private readonly Subject<Scenes> _sceneStream = new Subject<Scenes>();

        // singleton
        private SceneController()
        {
        }

        public static SceneController GetInstance()
        {
            return _sceneController;
        }

        public IObservable<Scenes> GetScenesStream()
        {
            return _sceneStream;
        }

        public void ShowHomeMenu()
        {
            _sceneStream.OnNext(Scenes.HomeMenu);
        }

        public void ShowPlaySetting()
        {
            _sceneStream.OnNext(Scenes.OfflinePlaySetting);
        }

        public void ShowPlayScene(PlayMode mode)
        {
            switch (mode)
            {
                case PlayMode.Solo:
                    _sceneStream.OnNext(Scenes.OfflinePlay);
                    break;
                case PlayMode.TwoPlayer:
                    _sceneStream.OnNext(Scenes.OfflinePlay);
                    break;
                case PlayMode.Online:
                    _sceneStream.OnNext(Scenes.OnlinePlayView);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public void ShowMatching()
        {
            _sceneStream.OnNext(Scenes.OnlineMatching);
        }
    }
}
