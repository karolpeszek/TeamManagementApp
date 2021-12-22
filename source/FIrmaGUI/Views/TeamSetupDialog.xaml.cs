using FirmaGUI;
using FirmaGUI.Helpers;
using Microsoft.UI.Xaml.Controls;
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

namespace FirmaGUI.Views
{
    public sealed partial class TeamSetupDialog : ContentDialog
    {
        Team Team = new Team();
        StorageHandler FileHandler = new StorageHandler();
        public TeamSetupDialog()
        {
            this.InitializeComponent();
            string ReadTeamJson = FileHandler.ReadFile("TeamDataFile.json");
            if (ReadTeamJson != null)
            {
                Team = JsonConvert.DeserializeObject<Team>(ReadTeamJson);
                TeamManagerName.Text = Team.Manager.Name;
                TeamManagerSurname.Text = Team.Manager.Surname;
                TeamManagerBirthdate.SelectedDate = Team.Manager.Birthday;
                TeamManagerPesel.Text = Team.Manager.PESEL;
                TeamManagerSex.SelectedIndex = (int)Team.Manager.Gender - 1;
                TeamManagerExperience.Value = Team.Manager.Experience;
                TeamName.Text = Team.TeamName;
            }
            else TeamManagerBirthdate.SelectedDate= new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            UpdateSaveButton();
        }
        private DateTime DateTimeOffsetToDateTime(DateTimeOffset SourceTime)
        {
            return new DateTime(SourceTime.Year, SourceTime.Month, SourceTime.Day);
        }
        private void UpdateSaveButton()
        {
            this.IsSecondaryButtonEnabled = !(TeamManagerPesel.Text.Length < 11 || TeamManagerName.Text == "" || TeamManagerSurname.Text == "" || TeamManagerPesel.Text == "" || TeamManagerSex.SelectedIndex == -1 || TeamName.Text == "" || TeamManagerBirthdate == null);
        }

        private void SaveButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            TeamManager Manager = new TeamManager(
                TeamManagerName.Text,
                TeamManagerSurname.Text,
                DateTimeOffsetToDateTime((DateTimeOffset)TeamManagerBirthdate.SelectedDate).ToString("yyyy-MM-dd"),
                TeamManagerPesel.Text,
                (genders)TeamManagerSex.SelectedIndex + 1,
                (int)TeamManagerExperience.Value
            );
            Team.TeamName = TeamName.Text;
            Team.Manager = Manager;
            string SerialisedJson = JsonConvert.SerializeObject(Team);
            FileHandler.SaveFile("TeamDataFile.json", SerialisedJson);
        }

        private void TeamManagerPesel_Changed(object sender, TextChangedEventArgs e)
        {
            while (System.Text.RegularExpressions.Regex.IsMatch(TeamManagerPesel.Text, "[^0-9]")||TeamManagerPesel.Text.Length>11)
            {
                
                TeamManagerPesel.Text = TeamManagerPesel.Text.Remove(TeamManagerPesel.Text.Length - 1);
                TeamManagerPesel.Select(TeamManagerPesel.Text.Length, 0);
            }
            UpdateSaveButton();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSaveButton();
        }
        private void NumberBox_ValueChanged(object sender, NumberBoxValueChangedEventArgs e)
        {
            UpdateSaveButton();
        }
        private void MemberSex_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateSaveButton();
        }
    }
}
