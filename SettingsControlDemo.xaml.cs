using SimHub.Plugins.Styles;
using System.Windows.Controls;

namespace User.PluginLocalizedDemo
{
    /// <summary>
    /// Logique d'interaction pour SettingsControlDemo.xaml
    /// </summary>
    public partial class SettingsControlDemo : UserControl
    {
        public DataPluginDemo Plugin { get; }

        public SettingsControlDemo()
        {
            InitializeComponent();
        }

        public SettingsControlDemo(DataPluginDemo plugin) : this()
        {
            this.Plugin = plugin;
            this.DataContext = plugin.Settings;
        }

        private async void StyledMessageBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var res = await SHMessageBox.Show("Message box",Properties.Resources.Hello, System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Question);

            await SHMessageBox.Show(res.ToString());
        }

        private void DemoWindow_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = new DemoWindow();

            window.Show();
        }

        private async void DemodialogWindow_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialogWindow = new DemoDialogWindow();

            var res = await dialogWindow.ShowDialogWindowAsync(this);

            await SHMessageBox.Show(res.ToString());
        }
    }
}