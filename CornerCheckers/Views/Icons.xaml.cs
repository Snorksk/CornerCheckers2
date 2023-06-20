using CornerCheckers.Models;
using System.Windows;
using System.Windows.Controls;

namespace CornerCheckers.Views
{
    /// <summary>
    /// Логика взаимодействия для Icons.xaml
    /// </summary>
    public partial class Icons : UserControl
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(CellValueEnum), typeof(Icons));

        public CellValueEnum Icon
        {
            get => (CellValueEnum)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public Icons()
        {
            InitializeComponent();
        }
    }
}
