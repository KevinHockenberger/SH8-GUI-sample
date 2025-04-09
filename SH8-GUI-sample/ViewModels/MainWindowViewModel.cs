using ipeenginectrlLib;
using SH8_Sample.Models;
using SH8_Sample.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using SH8Res = ipeenginectrlLib.Result;

namespace SH8_Sample.ViewModels
{
  // must implement INotifyPropertyChanged to update the view when properties change
  public class MainWindowViewModel : INotifyPropertyChanged
  {
    #region COMMANDS ##################################################################################################################################

    ICommand? connectCommand;
    public ICommand ConnectCommand => connectCommand ??= new Command(ConnectSherlock, p => IsNotConnected); // the return true is whether the button is enabled or not. code or property can be used here



    //ICommand? testCommand;
    //public ICommand TestCommand => testCommand ??= new Command(() => Task.Run(GetData), p => true); 

    private void GetData()
    {
      if (hSh8 == null) { return; }
      try
      {
        Attribute = ((double)hSh8.value("numberA.value"));
      }
      catch (Exception)
      {
      }
    }

    ICommand? stopCommand;
    public ICommand StopCommand => stopCommand ??= new Command(async () => { await Task.Run(StopMachine); CommandManager.InvalidateRequerySuggested(); }, p => IsConnected && IsInAuto);

    ICommand? startCommand;
    public ICommand StartCommand => startCommand ??= new Command(async () => { await Task.Run(RunMachine); CommandManager.InvalidateRequerySuggested(); }, p => IsConnected && IsNotInAuto);
    private void RunMachine()
    {
      if (hSh8?.run(-1) != SH8Res.Ok) { return; }
      hSh8.programLoopCompleted += SherlockLoopComplete;
      IsInAuto = true;
    }

    private void SherlockLoopComplete()
    {
      GetData();
    }

    private void StopMachine()
    {
      if (hSh8 == null) { return; }
      if (hSh8.requestStop() != SH8Res.Ok) { return; }
      int tries = 10;
      while (hSh8.isRunning())
      {
        Thread.Sleep(100); // wait for the machine to stop
        tries--;
        if (tries < 0) { break; } // give up after 10 tries
      }
      if (hSh8.isRunning())
      {
        if (hSh8.requestAbort() != SH8Res.Ok) { return; }
        while (hSh8.isRunning())
        {
          Thread.Sleep(100); // wait for the machine to abort
          tries--;
          if (tries < 0) { return; } // cancel
        }
      }
      IsInAuto = false;
    }

    ipeenginectrlLib.AxIpeEngine? hSh8;
    private void ConnectSherlock()
    {
      try
      {
        hSh8 = new();
        var nResult = hSh8.initialize("default");
        if (nResult != SH8Res.Ok) { IsConnected = false; return; }
        //AxipedisplayctrlLib.AxAxIpeDisplay imgWindow = new AxipedisplayctrlLib.AxAxIpeDisplay();

        //System.Windows.Forms.Integration.WindowsFormsHost host = new System.Windows.Forms.Integration.WindowsFormsHost();
        //host.Child = imgWindow;
        //Grid grid = new Grid();
        //grid.Children.Add(host);
        //this.ImageArea.Child = grid;

        nResult = hSh8.initialize("default");
        if (nResult != SH8Res.Ok) { IsConnected = false; return; }
        nResult = hSh8.load(Filename);
        if (nResult != SH8Res.Ok) { IsConnected = false; return; }
        //nResult = hSh8.connectDisplay(imgWindow.dspHandle, "workspace_mono8A");

        IsConnected = true;
      }
      catch (Exception)
      {
        IsConnected = false;
      }
    }
    #endregion
    #region PROPERTIES ##################################################################################################################################
    // required for INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public string IntFormat { get => StaticGlobals.intformat; }
    public string DecimalFormat { get => StaticGlobals.decimalformat; }

    private bool isConnected;
    public bool IsConnected
    {
      get => isConnected;
      set
      {
        if (isConnected != value)
        {
          isConnected = value;
          OnPropertyChanged(); // trigger the ui to update
          OnPropertyChanged(nameof(IsNotConnected)); // trigger the ui to also update this property
        }
      }
    }

    // a readonly property. no way to trigger OnPropertyChanged
    public bool IsNotConnected { get => !IsConnected; }
    private bool isInAuto;
    public bool IsInAuto
    {
      get => isInAuto;
      set
      {
        if (isInAuto != value)
        {
          isInAuto = value;
          OnPropertyChanged(); // trigger the ui to update
          OnPropertyChanged(nameof(IsNotInAuto)); // trigger the ui to also update this property
        }
      }
    }

    // a readonly property. no way to trigger OnPropertyChanged
    public bool IsNotInAuto { get => !IsInAuto; }
    public string Filename { get => Settings.Default.LastFilename; set { if (Settings.Default.LastFilename != value) { Settings.Default.LastFilename = value; OnPropertyChanged(); } } }
    private int total = 0;
    public int Total
    {
      get => total;
      // only settable from within the class
      private set { if (total != value) { total = value; OnPropertyChanged(); } }
    }
    private int pass = 0;
    public int Pass
    {
      get => pass;
      private set { if (pass != value) { pass = value; OnPropertyChanged(); } }
    }
    private int fail = 0;
    public int Fail
    {
      get => fail;
      private set { if (fail != value) { fail = value; OnPropertyChanged(); } }
    }
    private double attribute = 0;
    public double Attribute
    {
      get => attribute;
      private set { if (attribute != value) { attribute = value; OnPropertyChanged(); } }
    }
    private string file = string.Empty;
    public string File { get => file; set { if (file != value) { file = value; OnPropertyChanged(); } } }
    #endregion
    public bool Dispose() { /* code that should run to clean up resources */ return true; }
  }
}
