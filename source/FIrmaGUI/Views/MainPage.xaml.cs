using FirmaGUI;
using FirmaGUI.Helpers;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FirmaGUI.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        StorageHandler FileHandler = new StorageHandler();
        public MainPage()
        {
            InitializeComponent();
            if (FileHandler.ReadFile("TeamDataFile.json") == null || JsonConvert.DeserializeObject<Team>(FileHandler.ReadFile("TeamDataFile.json")).Manager == null)
            {
                ShowTeamSetupDialog();
            }

        }


        private ScreenCaptureHandler ScreenCaptureHandler = new ScreenCaptureHandler();
        private async void ShowTeamSetupDialog()
        {
            var Dialog = new TeamSetupDialog();
            ScreenCaptureHandler.SetScreenCapture(false);
            ContentDialogResult response = ContentDialogResult.None;
            do
            {
                response = await Dialog.ShowAsync();
                if(response==ContentDialogResult.Primary)
                {
                    string ReadXml = await FileHandler.UserReadFile(".xml");
                    if (!string.IsNullOrEmpty(ReadXml))
                    {
                        string ConvertedJson = FileHandler.XmlToJson(ReadXml);
                        FileHandler.SaveFile("TeamDataFile.json", ConvertedJson);
                    }
                }
            } while (FileHandler.ReadFile("TeamDataFile.json") == null || JsonConvert.DeserializeObject<Team>(FileHandler.ReadFile("TeamDataFile.json")).Manager == null);
            ScreenCaptureHandler.SetScreenCapture(true);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private async void LaunchTeamSetupButton_Click(object sender, RoutedEventArgs e)
        {
            var Dialog = new TeamSetupDialog();
            ScreenCaptureHandler.SetScreenCapture(false);
                var response = await Dialog.ShowAsync();
                if (response == ContentDialogResult.Primary)
                {
                    string ReadXml = await FileHandler.UserReadFile(".xml");
                    string ConvertedJson = FileHandler.XmlToJson(ReadXml);
                    FileHandler.SaveFile("TeamDataFile.json", ConvertedJson);
                }
            ScreenCaptureHandler.SetScreenCapture(true);
        }
        private async void GenerateTemplateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            var Dialog = new ContentDialog()
            {
                Title = FileHandler.GetStringFromReswFile("Main_GenerateTemplateTeamDialogTitle"),
                Content = FileHandler.GetStringFromReswFile("Main_GenerateTemplateTeamDialogContent"),
                CloseButtonText = FileHandler.GetStringFromReswFile("Main_GenerateTemplateTeamDialogCloseButton")
            };
            TeamManager manager = new TeamManager("Adam", "Kowalski", "01.08.1990", "90070142412", genders.male, 5);

            TeamMember m1 = new TeamMember("Witold", "Adamski", "22.10.1992", "92102266738", genders.male, "sekretarz", "01.01.2020");
            TeamMember m2 = new TeamMember("Jan", "Janowski", "15.03.1992", "92031532652", genders.male, "programista", "01.01.2020");
            TeamMember m3 = new TeamMember("Jan", "But", "16.05.1992", "92051613915", genders.male, "programista", "01.06.2019");
            TeamMember m4 = new TeamMember("Beata", "Nowak", "22.11.1993", "93112225023", genders.female, "projektant", "01.01.2020");
            TeamMember m5 = new TeamMember("Anna", "Mysza", "22.07.1991", "91072235964", genders.female, "projektant", "31.07.2019");

            Team t = new Team("Grupa IT", manager);
            t.AddMember(m1);
            t.AddMember(m2);
            t.AddMember(m3);
            t.AddMember(m4);
            t.AddMember(m5);
            StorageHandler handler = new StorageHandler();
            handler.SaveFile("TeamDataFile.json", JsonConvert.SerializeObject(t));

            await Dialog.ShowAsync();
        }
        private void GoToSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
