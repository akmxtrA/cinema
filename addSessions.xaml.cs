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
    /// Логика взаимодействия для addSessions.xaml
    /// </summary>
    public partial class addSessions : Window
    {
        string elementName;
        cinemaEntities db = new cinemaEntities();
        public addSessions()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            hollBox.ItemsSource = db.holls.ToList();
            hollBox.DisplayMemberPath = "id";
            hollBox.SelectedValuePath = "id";

            var lists = db.films.ToList();
            for (int i = 0; i < db.films.Count(); i++)
            {
                WrapPanel wp = new WrapPanel();
                System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                TextBlock name = new TextBlock();

                name.TextWrapping = TextWrapping.Wrap;

                wp.Height = 200;
                wp.Width = 150;

                name.Text = "Название: \n" + lists[i].name + "\t \t \t \t";

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

                filmsView.Items.Add(wp);
            }
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

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var films = db.films.FirstOrDefault(x => x.id == idClass.id_film);
            sessions sessions = new sessions
            {
                id_film = idClass.id_film,
                id_hall = (int)hollBox.SelectedValue,
                //time_start = timeHourseBox + ":" + timeMinutesBox,
                //time_end = Convert.,
                date = dateBox.SelectedDate,
                cost = Convert.ToDecimal(costBox.Text),
            };
            db.sessions.Add(sessions);
            db.SaveChanges();
            MessageBox.Show("Сеанс добавлен!");
        }
    }
}
