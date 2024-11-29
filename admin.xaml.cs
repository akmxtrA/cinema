using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для admin.xaml
    /// </summary>
    public partial class admin : Window
    {
        string elementName = "";
        cinemaEntities db = new cinemaEntities();
        public admin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var user = db.users.FirstOrDefault(x => x.id == idClass.id_user);
            nameLabel.Content = user.login;
            var Users = db.users.ToList();
            usersGrid.ItemsSource = Users;

            sessionsGrid.ItemsSource = db.sessions.ToList();;

            userBox.ItemsSource = db.users.ToList();
            userBox.SelectedValuePath = "id";
            userBox.DisplayMemberPath = "login";

            showFilmView();

        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow autorization = new MainWindow();
            autorization.Show();
            this.Close();
        }

        private void filmsView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point mousePosition = Mouse.GetPosition(this);
            IInputElement element = InputHitTest(mousePosition);
            elementName = (element as FrameworkElement)?.Uid;
            int id = Convert.ToInt32(elementName);
            var films = db.films.FirstOrDefault(x => x.id == id);

            idClass.id_film = films.id;
        }

        private void addFilm_Click(object sender, RoutedEventArgs e)
        {
            idClass.id_film = 0;

            addFilm addFilm = new addFilm();
            addFilm.Show();
        }

        private void redactFilm_Click(object sender, RoutedEventArgs e)
        {
            addFilm addFilm = new addFilm();
            addFilm.Show();
        }

        private void deleteFilm_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(elementName);
            var film = db.films.FirstOrDefault(x => x.id == id);
            db.films.Remove(film);
            db.SaveChanges();
            
            MessageBox.Show("Успешно удалено!");
            showFilmView();
        }

        private void reloadFilmView_Click(object sender, RoutedEventArgs e)
        {
            showFilmView();
        }

        public void showFilmView()
        {
            filmsView.Items.Clear();
            var lists = db.films.ToList();
            for (int i = 0; i < db.films.Count(); i++)
            {
                WrapPanel wp = new WrapPanel();
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                TextBlock name = new TextBlock();
                TextBlock description = new TextBlock();

                name.TextWrapping = TextWrapping.Wrap;
                description.TextWrapping = TextWrapping.Wrap;

                wp.Height = 350;
                wp.Width = 230;

                name.Text = "Название: \n" + lists[i].name + "\t \t \t \t";
                description.Text = "Описание: \n" + lists[i].description;

                string savePath = System.IO.Path.GetFullPath(@"..\..\res\films");
                savePath = savePath + "\\" + lists[i].photo;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(savePath);
                bitmap.EndInit();
                img.Source = bitmap;

                img.MouseDown += new MouseButtonEventHandler(filmsView_MouseDown);

                img.Height = 125;
                img.Width = 100;

                img.Uid = lists[i].id.ToString();

                wp.Children.Add(name);
                wp.Children.Add(img);
                wp.Children.Add(description);

                filmsView.Items.Add(wp);
            }
        }

        private void addSessionsBtn_Click(object sender, RoutedEventArgs e)
        {
            addSessions addSessions = new addSessions();
            addSessions.Show();
        }

        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            users users = new users
            {
                login = loginTxt.Text,
                password = passwordTxt.Text,
                id_role = roleBox.SelectedIndex + 1
            };
            db.users.Add(users);
            db.SaveChanges();
            MessageBox.Show("Пользователь добавлен!");
        }

        private void deleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = db.users.FirstOrDefault(x => x.id == (int)userBox.SelectedValue);
            db.users.Remove(user);
            db.SaveChanges();
            MessageBox.Show("Пользователь удалён!");
        }

        private void changeUserBtn_Click(object sender, RoutedEventArgs e)
        {
            changeUserBtn.Visibility = Visibility.Hidden;
            deleteUserBtn.Visibility = Visibility.Hidden;
            addUserBtn.Visibility = Visibility.Hidden;
            confirmChangeUserBtn.Visibility = Visibility.Visible;
            var user = db.users.FirstOrDefault(x => x.id == (int)userBox.SelectedValue);
            loginTxt.Text = user.login;
            passwordTxt.Text = user.password;
            roleBox.SelectedIndex = (int)user.id_role;
        }

        private void confirmChangeUserBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = db.users.FirstOrDefault(x => x.id == (int)userBox.SelectedValue);
            user.login = loginTxt.Text;
            user.password = passwordTxt.Text;
            user.id_role = roleBox.SelectedIndex + 1;
            db.SaveChanges();
            MessageBox.Show("Пользователь изменён!");


            confirmChangeUserBtn.Visibility = Visibility.Hidden;
            changeUserBtn.Visibility = Visibility.Visible;
            deleteUserBtn.Visibility = Visibility.Visible;
            addUserBtn.Visibility = Visibility.Visible;
        }
    }
}
