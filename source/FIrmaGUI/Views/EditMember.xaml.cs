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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FirmaGUI.Views
{
    public sealed partial class EditMember : ContentDialog
    {
        private StorageHandler FileHandler = new StorageHandler();
        private int MemberIndex = -1;
        private readonly StorageHandler StorageHandler = new StorageHandler();
        private TeamMember Member = null;
        public EditMember(int Index)
        {
            this.InitializeComponent();
            MemberIndex = Index;
            var Team = JsonConvert.DeserializeObject<Team>(StorageHandler.ReadFile("TeamDataFile.json"));
            Member = Team.Members[MemberIndex];
            if (Member != null)
            {
                MemberName.Text = Member.Name;
                MemberSurname.Text = Member.Surname;
                MemberBirthdate.SelectedDate = Member.Birthday;
                MemberPesel.Text = Member.PESEL;
                MemberSex.SelectedIndex = (int)Member.Gender - 1;
                MemberFunction.Text = Member.Function;
                MemberJoinDate.SelectedDate= Member.JoinDate;
            }
            UpdateSaveButton();
        }

        private void UpdateSaveButton()
        {
            this.IsSecondaryButtonEnabled = !(MemberPesel.Text.Length < 11 || MemberName.Text == "" || MemberSurname.Text == "" || MemberPesel.Text == "" || MemberSex.SelectedIndex == -1);
        }

        private void MemberPesel_Changed(object sender, TextChangedEventArgs e)
        {
            while (System.Text.RegularExpressions.Regex.IsMatch(MemberPesel.Text, "[^0-9]") || MemberPesel.Text.Length > 11)
            {

                MemberPesel.Text = MemberPesel.Text.Remove(MemberPesel.Text.Length - 1);
                MemberPesel.Select(MemberPesel.Text.Length, 0);
            }
            UpdateSaveButton();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSaveButton();
        }
        private DateTime DateTimeOffsetToDateTime(DateTimeOffset SourceTime)
        {
            return new DateTime(SourceTime.Year, SourceTime.Month, SourceTime.Day);
        }
        private void SaveButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            TeamMember teamMember = new TeamMember();
            teamMember.Name = MemberName.Text;
            teamMember.Surname = MemberSurname.Text;
            teamMember.PESEL = MemberPesel.Text;
            teamMember.Gender = (genders)(MemberSex.SelectedIndex + 1);
            teamMember.Function = MemberFunction.Text;
            teamMember.Birthday = DateTimeOffsetToDateTime((DateTimeOffset)MemberBirthdate.SelectedDate);
            teamMember.JoinDate = DateTimeOffsetToDateTime((DateTimeOffset)MemberJoinDate.SelectedDate);
            Team Team = JsonConvert.DeserializeObject<Team>(StorageHandler.ReadFile("TeamDataFile.json"));
            Team.Members[MemberIndex] = teamMember;
            StorageHandler.SaveFile("TeamDataFile.json", JsonConvert.SerializeObject(Team));
        }

        private void MemberSex_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateSaveButton();
        }
    }
}
