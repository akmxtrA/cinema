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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cinema
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        cinemaEntities db = new cinemaEntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = db.users.FirstOrDefault(x => x.login == loginBox.Text);
            if (user != null)
            {
                if (user.password == passwordBox.Text)
                {
                    idClass.id_user = user.id;
                    switch (user.id_role)
                    {
                        case 1:
                            admin admin = new admin();
                            admin.Show();
                            this.Close();
                            break;
                        case 2:
                            cashier cashier = new cashier();
                            cashier.Show();
                            this.Close();
                            break;
                        default:
                            MessageBox.Show("Роль данного пользователя ложная");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Неверный пароль");

                }
            }
            else
            {
                MessageBox.Show("Введённого пользователя не существует");
            }

        }
    }
}
