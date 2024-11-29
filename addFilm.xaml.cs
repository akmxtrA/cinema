using Microsoft.Win32;
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
using System.Xml;

namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для addFilm.xaml
    /// </summary>
    public partial class addFilm : Window
    {
        cinemaEntities db = new cinemaEntities();
        public addFilm()
        {
            InitializeComponent();
        }

        private void changePhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog(this);
            imgNameTxt.Text = System.IO.Path.GetFileName(openFileDialog.FileName);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(openFileDialog.FileName);
            bitmap.EndInit();
            img.Source = bitmap;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (idClass.id_film != 0)
            {
                var film = db.films.FirstOrDefault(x => x.id == idClass.id_film);
                nameTxt.Text = film.name;
                durationTxt.Text = film.duration;
                releaseDate.SelectedDate = film.release;
                descriptionTxt.Text = film.description;

                string savePath = System.IO.Path.GetFullPath(@"..\..\res\films");
                savePath = savePath + "\\" + film.photo;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(savePath);
                bitmap.EndInit();
                img.Source = bitmap;
                imgNameTxt.Text = film.photo;

            }
            else
            {
                nameTxt.Text = "";
            }
        }

        private void addFilmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (idClass.id_film == 0)
            {
                films films = new films
                {
                    name = nameTxt.Text,
                    duration = durationTxt.Text,
                    description = descriptionTxt.Text,
                    photo = imgNameTxt.Text,
                    release = releaseDate.SelectedDate
                };
                db.films.Add(films);
                db.SaveChanges();
                MessageBox.Show("Фильм добавлен!");
                this.Close();
            }
            else
            {
                var film = db.films.FirstOrDefault(x => x.id == idClass.id_film);
                film.name = nameTxt.Text;
                film.description = descriptionTxt.Text;
                film.duration = durationTxt.Text;
                film.photo = imgNameTxt.Text;
                film.release = releaseDate.SelectedDate;
                db.SaveChanges();
                MessageBox.Show("Изменения сохранены!");
                this.Close();
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
