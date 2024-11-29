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
using Word = Microsoft.Office.Interop.Word;

namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для sessionCashier.xaml
    /// </summary>
    public partial class sessionCashier : Window
    {
        cinemaEntities db = new cinemaEntities();
        public sessionCashier()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var list = db.sessions.ToList();
            var films = db.films.ToList();
            var sessions = from l in list join f in films on l.id_film equals f.id select new { id = l.id, date = l.date, time_start = l.time_start, time_end = l.time_end, cost = l.cost, name = f.name, photo = f.photo };
            var a = db.sessions.FirstOrDefault(x => x.id == idClass.id_session);

            /*string savePath = System.IO.Path.GetFullPath(@"..\..\res\films");
            savePath = savePath + "\\" + a.photo;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(savePath);
            bitmap.EndInit();
            filmImg.Source = bitmap;*/

            hollLabel.Content = "Зал: " + idClass.id_holl;

            switch (idClass.id_holl)
            {
                case 1:
                    firstHoll.Visibility = Visibility.Visible;
                    secondHoll.Visibility = Visibility.Hidden;
                    thirdHoll.Visibility = Visibility.Hidden;
                break;

                case 2:
                    firstHoll.Visibility = Visibility.Hidden;
                    secondHoll.Visibility = Visibility.Visible;
                    thirdHoll.Visibility = Visibility.Hidden;
                break;

                case 3:
                    firstHoll.Visibility = Visibility.Hidden;
                    secondHoll.Visibility = Visibility.Hidden;
                    thirdHoll.Visibility = Visibility.Visible;
                break;
            }

                
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            var list = db.sessions.ToList();
            var films = db.films.ToList();
            var sessions = from l in list join f in films on l.id_film equals f.id select new { id = l.id, date = l.date, time_start = l.time_start, time_end = l.time_end, cost = l.cost, name = f.name, photo = f.photo };
            var a = sessions.FirstOrDefault(x => x.id == idClass.id_session);

            var WordApp = new Word.Application();
            WordApp.Visible = false;
            DateTime date = (DateTime)a.date;
            var Worddoc = WordApp.Documents.Open(Environment.CurrentDirectory +
            @"\ticket.docx");
            //Замена слов из документа на необходимые данные
            Repwo("{номер билета}", 1.ToString() /*тут должно быть айди заказа*/, Worddoc);
            Repwo("{название}", a.name, Worddoc);
            Repwo("{дата}", date.ToShortDateString(), Worddoc);
            //Repwo("{время}", a.time_start, Worddoc);
            Repwo("{ряд}", 1.ToString(), Worddoc);
            Repwo("{место}", 1.ToString(), Worddoc);
            Repwo("{стоимость}", a.cost.ToString(), Worddoc);
            //Worddoc.SaveAs2(Environment.CurrentDirectory + @"\тестБилет" + ".docx");
            MessageBox.Show("билет сохранен");
            /*switch (idClass.id_holl)
            {
                case 1:
                    switch(combo)
                break;

                case 2:

                break;

                case 3:

                break;
            }*/
        }

        private void Repwo(string subToReplace, string text, Word.Document worddoc)
        {
            var range = worddoc.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: subToReplace, ReplaceWith: text);
        }
    }
}
