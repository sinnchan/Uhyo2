using Scripts.Main.Presentation.View;
using UnityEditor;

namespace Main.Presentation.View
{
    [InitializeOnLoad]
    public class Main
    {
        static Main()
        {
            ScenePresenter.GetInstance().Init();
        }
    }
}
