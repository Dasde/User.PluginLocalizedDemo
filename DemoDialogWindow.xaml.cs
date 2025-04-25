using SimHub.Plugins.UI;

namespace User.PluginLocalizedDemo
{
    /// <summary>
    /// Logique d'interaction pour DemoDialogWindow.xaml
    /// </summary>
    public partial class DemoDialogWindow : SHDialogContentBase
    {
        public DemoDialogWindow()
        {
            InitializeComponent();
            ShowOk = true;
            ShowCancel = true;
        }
    }
}
