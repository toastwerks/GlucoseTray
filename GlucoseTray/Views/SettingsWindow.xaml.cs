﻿using GlucoseTray.Enums;
using GlucoseTray.Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace GlucoseTray.Views.Settings
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public GlucoseTraySettings Settings { get; set; } = new GlucoseTraySettings
        {
            HighBg = 250,
            WarningHighBg = 200,
            WarningLowBg = 80,
            LowBg = 70,
            CriticalLowBg = 55,
            PollingThreshold = 15,
            StaleResultsThreshold = 15,
            DexcomUsername = "",
            NightscoutUrl = "",
            IsServerDataUnitTypeMmol = false,
            IsDebugMode = false,
            // Appears we will need to manually bind radios, dropdowns, and password fields for now.
        };

        public SettingsWindow()
        {
            InitializeComponent();

            combobox_dexcom_server.ItemsSource = typeof(DexcomServerLocation).GetFields().Select(x => (DescriptionAttribute[])x.GetCustomAttributes(typeof(DescriptionAttribute), false)).SelectMany(x => x).Select(x => x.Description);

            if (File.Exists(Program.SettingsFile))
            {
                try
                {
                    var model = FileService<GlucoseTraySettings>.ReadModelFromFile(Program.SettingsFile);

                    if (model is null)
                    {
                        MessageBox.Show("Unable to load existing settings due to a bad file.");
                    }
                    else
                    {
                        txt_dexcom_password.Password = model.DexcomPassword;
                        txt_nightscout_token.Password = model.AccessToken;
                        if (model.FetchMethod == FetchMethod.DexcomShare)
                            radio_source_dexcom.IsChecked = true;
                        else
                            radio_source_nightscout.IsChecked = true;

                        if (model.GlucoseUnit == GlucoseUnitType.MG)
                            radio_unit_mg.IsChecked = true;
                        else
                            radio_unit_mmol.IsChecked = true;

                        combobox_dexcom_server.SelectedIndex = (int)model.DexcomServer;
                        Settings = model;
                    }
                }
                catch (Exception e) // Catch serialization errors due to a bad file
                {
                    MessageBox.Show("Unable to load existing settings due to a bad file.  " + e.Message + e.InnerException?.Message);
                }
            }

            DataContext = Settings;
        }

        private bool HaveBypassedInitialModification;
        private void UpdateValuesFromMMoLToMG(object sender, RoutedEventArgs e)
        {
            if (!HaveBypassedInitialModification)
            {
                HaveBypassedInitialModification = true;
                return;
            }
            Settings.HighBg = Math.Round(Settings.HighBg *= 18);
            Settings.WarningHighBg = Math.Round(Settings.WarningHighBg *= 18);
            Settings.WarningLowBg = Math.Round(Settings.WarningLowBg *= 18);
            Settings.LowBg = Math.Round(Settings.LowBg *= 18);
            Settings.CriticalLowBg = Math.Round(Settings.CriticalLowBg *= 18);
        }

        private void UpdateValuesFromMGToMMoL(object sender, RoutedEventArgs e)
        {
            if (!HaveBypassedInitialModification)
            {
                HaveBypassedInitialModification = true;
                return;
            }
            Settings.HighBg = Math.Round(Settings.HighBg /= 18, 1);
            Settings.WarningHighBg = Math.Round(Settings.WarningHighBg /= 18, 1);
            Settings.WarningLowBg = Math.Round(Settings.WarningLowBg /= 18, 1);
            Settings.LowBg = Math.Round(Settings.LowBg /= 18, 1);
            Settings.CriticalLowBg = Math.Round(Settings.CriticalLowBg /= 18, 1);
        }

        private void ShowNightscoutBlock(object sender, RoutedEventArgs e)
        {
            if (label_dexcom_username == null)
                return;
            label_dexcom_username.Visibility = Visibility.Hidden;
            label_dexcom_password.Visibility = Visibility.Hidden;
            txt_dexcom_password.Visibility = Visibility.Hidden;
            txt_dexcom_username.Visibility = Visibility.Hidden;
            label_dexcom_server.Visibility = Visibility.Hidden;
            combobox_dexcom_server.Visibility = Visibility.Hidden;

            label_nightscoutUrl.Visibility = Visibility.Visible;
            label_nightscout_token.Visibility = Visibility.Visible;
            txt_nightscout_token.Visibility = Visibility.Visible;
            txt_nightscoutUrl.Visibility = Visibility.Visible;
        }

        private void ShowDexcomBlock(object sender, RoutedEventArgs e)
        {
            if (label_nightscoutUrl == null)
                return;
            label_nightscoutUrl.Visibility = Visibility.Hidden;
            label_nightscout_token.Visibility = Visibility.Hidden;
            txt_nightscout_token.Visibility = Visibility.Hidden;
            txt_nightscoutUrl.Visibility = Visibility.Hidden;

            label_dexcom_username.Visibility = Visibility.Visible;
            label_dexcom_password.Visibility = Visibility.Visible;
            txt_dexcom_password.Visibility = Visibility.Visible;
            txt_dexcom_username.Visibility = Visibility.Visible;
            label_dexcom_server.Visibility = Visibility.Visible;
            combobox_dexcom_server.Visibility = Visibility.Visible;
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.FetchMethod = radio_source_dexcom.IsChecked == true ? FetchMethod.DexcomShare : FetchMethod.NightscoutApi;
            Settings.GlucoseUnit = radio_unit_mg.IsChecked == true ? GlucoseUnitType.MG : GlucoseUnitType.MMOL;
            Settings.DexcomServer = (DexcomServerLocation)combobox_dexcom_server.SelectedIndex;
            Settings.DexcomUsername = txt_dexcom_username.Text;
            Settings.DexcomPassword = txt_dexcom_password.Password;
            Settings.AccessToken = txt_nightscout_token.Password;

            var errors = SettingsService.ValidateSettings(Settings);
            if (errors.Any())
            {
                MessageBox.Show("Settings are not valid.  Please fix before continuing.\r\n\r\n" + string.Join("\r\n", errors));
                return;
            }

            FileService<GlucoseTraySettings>.WriteModelToJsonFile(Settings, Program.SettingsFile);

            DialogResult = true;
            Close();
        }
    }
}
