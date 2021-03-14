using UnityEditor;

namespace Scripts.Main.Presentation.View
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