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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL.MVVM.View
{
    /// <summary>
    /// Interaction logic for AnalyzerView.xaml
    /// </summary>
    public partial class AnalyzerView : UserControl
    {
        public AnalyzerVM viewM { get; set; }
        public AnalyzerView()
        {
            InitializeComponent();
            viewM = new AnalyzerVM();
            this.DataContext = viewM;
        }
    }
}
