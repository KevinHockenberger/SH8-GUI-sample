using SH8_Sample.Models;
using SH8_Sample.Properties;
using SH8_Sample.ViewModels;
using System;
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
using AxipedisplayctrlLib;

namespace SH8_Sample
{  /// <summary>
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

    private void Window_Initialized(object sender, EventArgs e)
    {
      ApplyAppSettings();
      ApplyUserSettings();
      vm.AssignStatusText("Load a Sherlock program at the top.", 0);
    }
    private void ApplyAppSettings()
    {
      // assign the last used settings from the application settings file
      if (Settings.Default.UpgradeRequired)
      {
        Settings.Default.Upgrade();
        Settings.Default.UpgradeRequired = false;
        Settings.Default.Save();
      }
      Width = Settings.Default.LastSavedWidth;
      Height = Settings.Default.LastSavedHeight; if (Height < 0) { Height = 0; }
      Top = Settings.Default.LastSavedTop;
      Left = Settings.Default.LastSavedLeft;
      WindowState = Settings.Default.LastWindowState == "Maximized" ? WindowState.Maximized : WindowState.Normal;
      // vaidate the window is somewhat on screen and large enough to view
      if (Top < 0) { Top = 0; }
      else if (Top > SystemParameters.VirtualScreenHeight) { Top = 0; }
      if (Left < 0) { Left = 0; }
      if (Width < 325) { Width = 325; }
      else if (Width > SystemParameters.VirtualScreenWidth) { Width = SystemParameters.VirtualScreenWidth; }
      if (Height < 200) { Height = 200; }
      else if (Height > SystemParameters.VirtualScreenHeight) { Height = SystemParameters.VirtualScreenHeight; }
    }
    private void SaveAppSettings()
    {
      Settings.Default.LastWindowState = this.WindowState.ToString();
      Settings.Default.LastSavedWidth = Width;
      Settings.Default.LastSavedHeight = Height;
      Settings.Default.LastSavedTop = Top;
      Settings.Default.LastSavedLeft = Left;
      Settings.Default.Save();
    }
    private void ApplyUserSettings()
    {
      // any code that should load user/file settings from another config or database
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      SaveAppSettings();
      if (!vm.Dispose())
      {
        Thread.Sleep(1000);
        if (!vm.Dispose())
        {
          Thread.Sleep(1000);
          if (!vm.Dispose())
          {
            // could not stop ...
            e.Cancel = true;
            vm.AssignStatusText("Could not close application", 4);
            return;
          }
        }
      }
    }

    private void InitSherlock(object sender, RoutedEventArgs e)
    {
      try
      {
        vm.ConnectSherlock();
        AxAxIpeDisplay imgWindow = new AxAxIpeDisplay();
        System.Windows.Forms.Integration.WindowsFormsHost host = new();
        host.Child = imgWindow;
        ImageArea.Child = host;
        ImageArea.Background = System.Windows.Media.Brushes.Transparent;
        vm.HSh8?.connectDisplay(imgWindow.dspHandle, "workspace_mono8A");
        vm.AssignStatusText("Sherlock ready.", 1);
      }
      catch (Exception ex)
      {
        vm.AssignStatusText(ex.Message, 2);
      }
    }
  }
}
