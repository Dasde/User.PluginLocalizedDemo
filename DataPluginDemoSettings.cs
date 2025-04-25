using System.ComponentModel;

namespace User.PluginLocalizedDemo
{
    /// <summary>
    /// Settings class, make sure it can be correctly serialized using JSON.net
    /// </summary>
    public class DataPluginDemoSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int SpeedWarningLevel = 100;
    }
}