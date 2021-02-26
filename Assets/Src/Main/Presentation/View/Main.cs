using Src.Presentation.View;
using UnityEditor;

namespace Src.Main.Presentation.View
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
