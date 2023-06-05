using Casualkit.Toolkit.UI;


namespace CasualKit.Toolkit.Loader
{
    public class TkLoadingIcon : TkCanvasBase
    {
        public void Show()
        {
            Active = true;
        }

        public void Hide()
        {
            Active = false;
        }
    }

}