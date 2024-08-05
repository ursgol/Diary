using Diary.Commands;
using Diary.Models;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Properties;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Diary.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Repository _repository = new Repository();


        public MainViewModel()
        {
          
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            SettingsCommand = new RelayCommand(SettingsCom);
            AddStudentCommand = new RelayCommand(AddEditStudent);
            EditStudentCommand = new RelayCommand(AddEditStudent, CanEditDeleteStudent);
            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent, CanEditDeleteStudent);
            LoadedWindowCommand = new RelayCommand(LoadedWindow);

            LoadedWindow(null);
           

           
        }

        private async void LoadedWindow(object obj)
        {
            if (!IsValidConnectionToDatabase())
            {
                var metroWindow = Application.Current.MainWindow as MetroWindow;
                var dialog = await metroWindow.ShowMessageAsync("Błąd połączenia", $"Nie można połączyć się" +
                    $"z bazą danych. Czy chcesz zmienić ustawienia? ", MessageDialogStyle.AffirmativeAndNegative);

                if (dialog == MessageDialogResult.Negative)
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    var settingsWindow = new Views.Settings(false);
                    settingsWindow.ShowDialog();
                }
            }
            else
            {
                RefreshDiary();
                InitGroups();
            }
        }

        public ICommand RefreshStudentsCommand { get; set; }
        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand LoadedWindowCommand { get; set; }


        private StudentWrapper _selectedStudent;

        public StudentWrapper SelectedStudent
        {
            get { return _selectedStudent; }
            set { 
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StudentWrapper> _students;

        public ObservableCollection<StudentWrapper> Students
        {
            get { return _students; }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        private int _selectedGroupId;
        public int SelectedGroupId
        {
            get { return _selectedGroupId; }
            set
            {
                _selectedGroupId = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Group> _group;
        private int settingsWindow_Closed;

        public ObservableCollection<Group> Groups
        {
            get { return _group; }
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }

        private void RefreshDiary()
        {
            Students = new ObservableCollection<StudentWrapper>(_repository.GetStudents(SelectedGroupId));
           
        }



        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 0, Name = "Wszystkie" });

            Groups = new ObservableCollection<Group>(groups);
            
            SelectedGroupId = 0;
        }

        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }

        private async Task DeleteStudent(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;
            var dialog = await metroWindow.ShowMessageAsync("Usuwanie ucznia", 
                $"Czy na pewno chcesz usunąć ucznia " +
                $"{SelectedStudent.FirstName} {SelectedStudent.LastName}?", 
                MessageDialogStyle.AffirmativeAndNegative);

            if(dialog != MessageDialogResult.Affirmative)
            {
                return;
            }

            //usuwanie z bazy
            _repository.DeleteStudent(SelectedStudent.Id);
            RefreshDiary();
        }

        private void AddEditStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }

        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void RefreshStudents(object obj)
        {
            RefreshDiary();
        }

        private void SettingsCom(object obj)
        {
            var settingsWindow = new Views.Settings(true);
            settingsWindow.ShowDialog();
        }

        private bool IsValidConnectionToDatabase()
        {
            try
            {
                using(var context = new ApplicationDbContext())
                {
                    context.Database.Connection.Open();
                    context.Database.Connection.Close();

                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
