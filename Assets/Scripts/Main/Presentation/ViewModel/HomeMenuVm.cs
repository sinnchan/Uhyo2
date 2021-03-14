using Scripts.Main.Domain.Entities;

namespace Scripts.Main.Presentation.ViewModel
{
    public class HomeMenuVm
    {
        private readonly SceneController _sceneController = SceneController.GetInstance();

        public void OnClickPlayButton()
        {
            _sceneController.ShowPlaySetting();
        }

        public void OnClick2PlayerButton()
        {
            _sceneController.ShowPlayScene(PlayMode.TwoPlayer);
        }

        public void OnClickOnlinePlayButton()
        {
            _sceneController.ShowMatching();
        }
    }
}