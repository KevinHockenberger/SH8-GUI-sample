using MVVM_template.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM_template
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    // assigned this way allows backend methods to reference the viewmodel
    static MainWindowViewModel vm = new();

    public MainWindow()
    {
      // this is what links the view to the viewmodel at runtime
      DataContext = vm;
      // or you could simply do this if you didn't need the reference
      //DataContext = new MainWindowViewModel();
      InitializeComponent();
    }
  }
}