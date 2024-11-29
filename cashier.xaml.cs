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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для cashier.xaml
    /// </summary>
    public partial class cashier : Window
    {
        cinemaEntities db = new cinemaEntities();
        public cashier()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var user = db.users.FirstOrDefault(x => x.id == idClass.id_user);
            nameLabel.Content = user.login;

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
                description.Text = "Описание: \n" + lists[i].description + "\n";

                string savePath = System.IO.Path.GetFullPath(@"..\..\res\films");
                savePath = savePath + "\\" + lists[i].photo;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(savePath);
                bitmap.EndInit();
                img.Source = bitmap;

                

                img.Height = 125;
                img.Width = 100;

                img.Uid = lists[i].id.ToString();

                wp.Children.Add(name);
                wp.Children.Add(img);
                wp.Children.Add(description);

                filmsView.Items.Add(wp);
            }

            var list = db.sessions.ToList();
            var films = db.films.ToList();
            var sessions = from l in list join f in films on l.id_film equals f.id select new {id = l.id, date = l.date, time_start = l.time_start, time_end = l.time_end, cost = l.cost, name = f.name, photo = f.photo  };
            var a = sessions.ToList();
            for (int i = 0; i < db.sessions.Count(); i++)
            {
                WrapPanel wp = new WrapPanel();
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                TextBlock name = new TextBlock();
                TextBlock cost = new TextBlock();
                Button button = new Button();

                name.TextWrapping = TextWrapping.Wrap;
                cost.TextWrapping = TextWrapping.Wrap;

                button.Width = 50;
                button.Height = 50;
                button.Content = a[i].time_start;

                wp.Height = 350;
                wp.Width = 230;

                name.Text = $"Название: \n" + a[i].name + "\t \t \t \t";
                cost.Text = "Цена: \n" + a[i].cost + "Руб.\n";

                string savePath = System.IO.Path.GetFullPath(@"..\..\res\films");
                savePath = savePath + "\\" + a[i].photo;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(savePath);
                bitmap.EndInit();
                img.Source = bitmap;

                button.Click += (object sender1, RoutedEventArgs r) =>
                {
                    idClass.id_session = Convert.ToInt32(button.Uid);
                    sessionCashier sessionCashier = new sessionCashier();
                    sessionCashier.Show();
                };

                img.Height = 125;
                img.Width = 100;

                button.Uid = a[i].id.ToString();
                img.Uid = a[i].id.ToString();

                wp.Children.Add(name);
                wp.Children.Add(img);
                wp.Children.Add(cost);
                wp.Children.Add(button);

                sessionsView.Items.Add(wp);
            }
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow autorization = new MainWindow();
            autorization.Show();
            this.Close();
        }
    }
}
