using Main.Presentation.ViewModel;
using UnityEngine;

namespace Main.Presentation.View
{
    public class HomeMenu : MonoBehaviour
    {
        private readonly HomeMenuVm _vm = new HomeMenuVm();

        public void OnClickPlayButton()
        {
            _vm.OnClickPlayButton();
        }

        public void OnClick2PlayerButton()
        {
            _vm.OnClick2PlayerButton();
        }

        public void OnClickOnlinePlayButton()
        {
            _vm.OnClickOnlinePlayButton();
        }
    }
}
