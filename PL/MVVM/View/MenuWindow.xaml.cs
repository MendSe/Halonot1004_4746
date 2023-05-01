using MaterialDesignThemes.Wpf;
using PL.MVVM.ViewModel;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private bool menu;
        MenuVM menuVM;
        public MenuWindow()
        {
            InitializeComponent();
            menuVM = new MenuVM();
            this.DataContext = menuVM;
        }
        private void Border_MouseHold(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            if (menu)
            {
                var sb = Resources["OpenMenu"] as Storyboard;
                sb?.Begin(Bar);
                menu = false;
                OpenCloseButtonIcon.Kind = PackIconKind.MenuUp;
            }
            else
            {
                var sb = Resources["CloseMenu"] as Storyboard;
                sb?.Begin(Bar);
                menu = true;
                OpenCloseButtonIcon.Kind = PackIconKind.MenuDown;
            }

        }
        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            if (!menu)
            {
                var sb = Resources["CloseMenu"] as Storyboard;
                sb?.Begin(Bar);
                menu = true;
                OpenCloseButtonIcon.Kind = PackIconKind.MenuDown;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
