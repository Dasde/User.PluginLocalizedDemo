﻿using GameReaderCommon;
using SimHub.Plugins;
using SimHub.Plugins.Devices;
using System;
using System.Globalization;
using System.Windows.Media;
using WoteverLocalization;

namespace User.PluginLocalizedDemo
{
    [PluginDescription("Sample localized plugin")]
    [PluginAuthor("Author")]
    [PluginName("Localized Demo plugin")]
    public class DataPluginDemo : IPlugin, IDataPlugin, IWPFSettingsV2
    {
        public DataPluginDemoSettings Settings;
        private bool SimhubIsClosing;

        /// <summary>
        /// Instance of the current plugin manager
        /// </summary>
        public PluginManager PluginManager { get; set; }

        /// <summary>
        /// Gets the left menu icon. Icon must be 24x24 and compatible with black and white display.
        /// </summary>
        public ImageSource PictureIcon => this.ToIcon(Properties.Resources.sdkmenuicon);

        /// <summary>
        /// Gets a short plugin title to show in left menu. Return null if you want to use the title as defined in PluginName attribute.
        /// </summary>
        public string LeftMenuTitle => "Localized Demo plugin";
        private string lastLog;
        /// <summary>
        /// Called one time per game data update, contains all normalized game data,
        /// raw data are intentionally "hidden" under a generic object type (A plugin SHOULD NOT USE IT)
        ///
        /// This method is on the critical path, it must execute as fast as possible and avoid throwing any error
        ///
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <param name="data">Current game data, including current and previous data frame.</param>
        public void DataUpdate(PluginManager pluginManager, ref GameData data)
        {
            // Define the value of our property (declared in init)
            if (data.GameRunning)
            {
                if (data.OldData != null && data.NewData != null)
                {
                    if (data.OldData.SpeedKmh < Settings.SpeedWarningLevel && data.OldData.SpeedKmh >= Settings.SpeedWarningLevel)
                    {
                        // Trigger an event
                        this.TriggerEvent("SpeedWarning");
                    }
                }
            }
            String newLog = pluginManager.GetPropertyValue("DataCorePlugin.LoggingLastMessage").ToString();
            if (newLog != lastLog)
            {
                lastLog = newLog;
                if (newLog == "Closing simhub." || newLog == "Application exit")
                    SimhubIsClosingHandler();
            }         
        }

        private void SimhubIsClosingHandler()
        {
            this.PluginManager.TriggerEvent("SimhubIsClosing", this.GetType());
            this.PluginManager.SetPropertyValue("SimhubIsClosing", this.GetType(), true);
            SimhubIsClosing = true;
        }
        private void PluginManager_PreApplicationExit(object sender, EventArgs e)
        {
            SimhubIsClosingHandler();
        }

        private void PluginManager_ApplicationExit(object sender, EventArgs e)
        {
            SimhubIsClosingHandler();
        }

        /// <summary>
        /// Called at plugin manager stop, close/dispose anything needed here !
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void End(PluginManager pluginManager)
        {
            SimhubIsClosingHandler();
            // Save settings
            this.SaveCommonSettings("GeneralSettings", Settings);
        }

        /// <summary>
        /// Returns the settings control, return null if no settings control is required
        /// </summary>
        /// <param name="pluginManager"></param>
        /// <returns></returns>
        public System.Windows.Controls.Control GetWPFSettingsControl(PluginManager pluginManager)
        {
            return new SettingsControlDemo(this);
        }

        /// <summary>
        /// Called once after plugins startup
        /// Plugins are rebuilt at game change
        /// </summary>
        /// <param name="pluginManager"></param>
        public void Init(PluginManager pluginManager)
        {
            SimHub.Logging.Current.Info("Starting plugin");

            // Load settings
            Settings = this.ReadCommonSettings<DataPluginDemoSettings>("GeneralSettings", () => new DataPluginDemoSettings());

            /* 
             * // If you want to localize your plugin with the current language used by Simhub just do this :
            */
            //String LanguageName = LocalizationProvider.Instance.CurrentLanguage.DisplayName;
            String cultureCode = LocalizationProvider.Instance.CurrentLanguage.Code;
            CultureInfo ci = new CultureInfo(cultureCode);
            Properties.Resources.Culture = ci;
            SimHub.Logging.Current.Info(Properties.Resources.Hello);

            // Declare a property available in the property list, this gets evaluated "on demand" (when shown or used in formulas)
            this.AttachDelegate(name: "CurrentDateTime", valueProvider: () => DateTime.Now);

            // Declare an event
            this.AddEvent(eventName: "SpeedWarning");

            // Declare an action which can be called
            this.AddAction(
                actionName: "IncrementSpeedWarning",
                actionStart: (a, b) =>
                {
                    Settings.SpeedWarningLevel++;
                    SimHub.Logging.Current.Info("Speed warning changed");
                });

            // Declare an action which can be called, actions are meant to be "triggered" and does not reflect an input status (pressed/released ...)
            this.AddAction(
                actionName: "DecrementSpeedWarning",
                actionStart: (a, b) =>
                {
                    Settings.SpeedWarningLevel--;
                });

            // Declare an input which can be mapped, inputs are meant to be keeping state of the source inputs,
            // they won't trigger on inputs not capable of "holding" their state.
            // Internally they work similarly to AddAction, but are restricted to a "during" behavior
            this.AddInputMapping(
                inputName: "InputPressed",
                inputPressed: (a, b) => {/* One of the mapped input has been pressed   */},
                inputReleased: (a, b) => {/* One of the mapped input has been released */}
            );

            SimhubIsClosing = false;
            this.AddEvent(eventName: "SimhubIsClosing");
            this.AttachDelegate(name: "SimhubIsClosing", valueProvider: () => SimhubIsClosing);
            pluginManager.ApplicationExit += PluginManager_ApplicationExit;
            pluginManager.PreApplicationExit += PluginManager_PreApplicationExit;
        }

    }
}
