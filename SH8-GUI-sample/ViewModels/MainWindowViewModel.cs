using SH8_Sample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SH8_Sample.ViewModels
{
  // must implement INotifyPropertyChanged to update the view when properties change
  public class MainWindowViewModel : INotifyPropertyChanged
  {
    #region COMMANDS ##################################################################################################################################

    ICommand? startCommand;
    public ICommand StartCommand => startCommand ??= new Command(() => { IsInAuto = true; }, p => IsNotInAuto); // the return true is whether the button is enabled or not. code or property can be used here
    ICommand? stopCommand;
    public ICommand StopCommand => stopCommand ??= new Command(() => { IsInAuto = false; }, p => IsInAuto); // the return true is whether the button is enabled or not. code or property can be used here

    ICommand? runMachineCommand;
    public ICommand RunMachineCommand => runMachineCommand ??= new Command(() => Task.Run(RunMachine), p => IsNotInAuto); // the return true is whether the button is enabled or not. code or property can be used here

    private void RunMachine()
    {
      IsInAuto = true;
      while (IsInAuto)
      {
        // do something
        Total++;
        if (Total % 2 == 0) { Pass++; }
        else { Fail++; }
        Attribute = (double)Pass / Total;
        System.Threading.Thread.Sleep(100);
      }
    }

    RelayCommand<string?>? doSomethingWithStringCommand;
    public RelayCommand<string?> DoSomethingWithStringCommand => doSomethingWithStringCommand ??= new RelayCommand<string?>((p) =>
    {
      if (p == null) { return; }
      DoSomethingWithString(p);
    }, p => true); // the return true is whether the button is enabled or not. code or property can be used here

    private void DoSomethingWithString(string? s)
    {
    }
    #endregion
    #region PROPERTIES ##################################################################################################################################
    // required for INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

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
    public bool IsNotInAuto { get => !isInAuto; }

    private int total = 0;
    public int Total
    {
      get => total;
      // only settable from within the class
      private set { if (total != value) { total = value; OnPropertyChanged(); OnPropertyChanged(nameof(TotalString)); } }
    }
    public string TotalString { get => total.ToString(StaticGlobals.intformat); }
    private int pass = 0;
    public int Pass
    {
      get => pass;
      private set { if (pass != value) { pass = value; OnPropertyChanged(); OnPropertyChanged(nameof(PassString)); } }
    }
    public string PassString { get => pass.ToString(StaticGlobals.intformat); }
    private int fail = 0;
    public int Fail
    {
      get => fail;
      private set { if (fail != value) { fail = value; OnPropertyChanged(); OnPropertyChanged(nameof(FailString)); } }
    }
    public string FailString { get => fail.ToString(StaticGlobals.intformat); }
    private double attribute = 0;
    public double Attribute
    {
      get => attribute;
      private set { if (attribute != value) { attribute = value; OnPropertyChanged();OnPropertyChanged(nameof(AttributeString)); } }
    }
    public string AttributeString { get => attribute.ToString(StaticGlobals.decimalformat); }
    private string file = string.Empty;
    public string File { get => file; set { if (file != value) { file = value; OnPropertyChanged(); } } }
    #endregion
    public bool Dispose() { /* code that should run to clean up resources */ return true; }
  }
}
