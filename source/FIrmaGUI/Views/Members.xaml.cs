using FirmaGUI;
using FirmaGUI.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FirmaGUI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Members : Page
    {
        StorageHandler FileHandler = new StorageHandler();
        Team Team = new Team();
        public Members()
        {
            this.InitializeComponent();
            RefreshMemberListView();
        }

        private ScreenCaptureHandler ScreenCaptureHandler = new ScreenCaptureHandler();
        private void RefreshMemberListView()
        {
            string ReadFileContent = FileHandler.ReadFile("TeamDataFile.json");
            Team = JsonConvert.DeserializeObject<Team>(ReadFileContent);
            MembersList.ItemsSource = Team.Members;
            if (MembersList.SelectedIndex >= 0)
            {
                EditMemberButton.Visibility = Visibility.Visible;
                DeleteMemberButton.Visibility = Visibility.Collapsed;
            }
        }
        private async void ImportFromXml_Click(object sender, RoutedEventArgs e)
        {
            string ReadXml = await FileHandler.UserReadFile(".xml");
            if (string.IsNullOrEmpty(ReadXml)) return;
            string ConvertedJson = FileHandler.XmlToJson(ReadXml);
            var T = JsonConvert.DeserializeObject<Team>(ConvertedJson);
            if (T.Manager != null)
            {
                FileHandler.SaveFile("TeamDataFile.json", ConvertedJson);
                RefreshMemberListView();
            }
            else
            {
                var Dialog = new ContentDialog()
                {
                    Title = FileHandler.GetStringFromReswFile("Members_ImportWrongDataDialogTitle"),
                    Content = FileHandler.GetStringFromReswFile("Members_ImportWrongDataDialogContent"),
                    CloseButtonText = FileHandler.GetStringFromReswFile("Members_ImportWrongDataDialogCloseButtonText")
                };
                await Dialog.ShowAsync();
            }
        }
        private void ExportToXml_Click(object sender, RoutedEventArgs e)
        {
            string SerializedJson = JsonConvert.SerializeObject(Team);
            string SerializedXml = FileHandler.JsonToXml(SerializedJson);
            FileHandler.UserSaveFile(".xml", SerializedXml);
        }
        private async void ImportFromJson_Click(object sender, RoutedEventArgs e)
        {
            string ReadJson = await FileHandler.UserReadFile(".json");
            if(string.IsNullOrEmpty(ReadJson)) return;
            var T = JsonConvert.DeserializeObject<Team>(ReadJson);
            if (T.Manager != null)
            {
                FileHandler.SaveFile("TeamDataFile.json", ReadJson);
                RefreshMemberListView();
            }
            else
            {
                var Dialog = new ContentDialog()
                {
                    Title = FileHandler.GetStringFromReswFile("Members_ImportWrongDataDialogTitle"),
                    Content = FileHandler.GetStringFromReswFile("Members_ImportWrongDataDialogContent"),
                    CloseButtonText = FileHandler.GetStringFromReswFile("Members_ImportWrongDataDialogCloseButtonText")
                };
                await Dialog.ShowAsync();
            }
            
        }
        private void ExportToJson_Click(object sender, RoutedEventArgs e)
        {
            string SerializedJson = JsonConvert.SerializeObject(Team);
            FileHandler.UserSaveFile(".json", SerializedJson);
        }
        private void Fire_EveryoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.FireEveryoneButton.Flyout is Flyout f)
            {
                f.Hide();
            }
            Team.FireEveryone();
            FileHandler.SaveFile("TeamDataFile.json", JsonConvert.SerializeObject(Team));
            RefreshMemberListView();
        }
        private void DeletetMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DeleteMemberButton.Flyout is Flyout f)
            {
                f.Hide();
            }
            Team.FireTeamMember(MembersList.SelectedIndex);
            FileHandler.SaveFile("TeamDataFile.json", JsonConvert.SerializeObject(Team));
            RefreshMemberListView();
        }
        private async void AddMemberButton_Click(object sender, RoutedEventArgs e)
        {
            ScreenCaptureHandler.SetScreenCapture(false);
            TeamMember Member = new TeamMember("", "", "01.01.1970", "", genders.unset, "", "01.01.1970");
            Team.AddMember(Member);
            FileHandler.SaveFile("TeamDataFile.json", JsonConvert.SerializeObject(Team));
            var Dialog = new EditMember(Team.TeamMemberCount - 1);
            var Result = await Dialog.ShowAsync();
            if (Result == ContentDialogResult.Secondary) RefreshMemberListView();
            else
            {
                Team.FireTeamMember(Team.TeamMemberCount - 1);
                FileHandler.SaveFile("TeamDataFile.json", JsonConvert.SerializeObject(Team));
            }
            ScreenCaptureHandler.SetScreenCapture(true);
        }
        private async void EditMemberButton_Click(object sender, RoutedEventArgs e)
        {
            ScreenCaptureHandler.SetScreenCapture(false);
            var Dialog = new EditMember(MembersList.SelectedIndex);
            var Result = await Dialog.ShowAsync();
            if (Result == ContentDialogResult.Secondary) RefreshMemberListView();
            ScreenCaptureHandler.SetScreenCapture(true);
        }
        private void MemberList_SelectionChange(object sender, RoutedEventArgs e)
        {
            if(MembersList.SelectedIndex >= 0)
            {
                EditMemberButton.Visibility = Visibility.Visible;
                DeleteMemberButton.Visibility = Visibility.Visible;
            }
        }
    }
}
