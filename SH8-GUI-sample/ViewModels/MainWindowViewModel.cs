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

    ICommand? doSomethingCommand;
    public ICommand DoSomethingCommand => doSomethingCommand ??= new Command(() => { Task.Run(DoSomething); }, p => true); // the return true is whether the button is enabled or not. code or property can be used here

    private void DoSomething()
    {
      IsSomethingBool = !IsSomethingBool;
      switch (SomethingString)
      {
        case "A":
          SomethingString = "B";
          break;
        case "B":
          SomethingString = "C";
          break;
        case "C":
          SomethingString = string.Empty;
          break;
        default:
          SomethingString = "A";
          break;
      }
      myDictionaryValue++;
      if (myDictionaryValue > 5 || myDictionaryValue < 0) { myDictionaryValue = 0; }
      OnPropertyChanged(nameof(MyDictionaryValue)); // since this property does not self trigger (readonly) trigger the ui to update
    }

    RelayCommand<string?>? doSomethingWithStringCommand;
    public RelayCommand<string?> DoSomethingWithStringCommand => doSomethingWithStringCommand ??= new RelayCommand<string?>((p) =>
    {
      if (p == null) { return; }
      DoSomethingWithString(p);
    }, p => true); // the return true is whether the button is enabled or not. code or property can be used here

    private void DoSomethingWithString(string? s)
    {
      if (MyString == s) { s = "send a different"; }
      MyString = s ?? "no param sent";
    }
    #endregion
    #region PROPERTIES ##################################################################################################################################
    // required for INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool isSomethingBool;
    public bool IsSomethingBool
    {
      get => isSomethingBool;
      set
      {
        if (isSomethingBool != value)
        {
          isSomethingBool = value;
          OnPropertyChanged(); // trigger the ui to update
          OnPropertyChanged(nameof(IsNotSomethingBool)); // trigger the ui to also update this property
        }
      }
    }

    // a readonly property. no way to trigger OnPropertyChanged
    public bool IsNotSomethingBool { get => !IsSomethingBool; }

    private string somethingString = string.Empty;
    public string SomethingString
    {
      get => somethingString;
      // only settable from within the class
      private set { if (somethingString != value) { somethingString = value; OnPropertyChanged(); } }
    }
    private string myString = string.Empty;
    public string MyString { get => myString; set { if (myString != value) { myString = value; OnPropertyChanged(); } } }

    private int myDictionaryValue = 0;
    public string MyDictionaryValue
    {
      get
      {
        try { return StaticGlobals.MyDictionary.Where(p => p.Key == myDictionaryValue).First().Value; }
        catch (Exception) { return string.Empty; }
      }
    }
    #endregion
    public bool Dispose() { /* code that should run to clean up resources */ return true; }
  }
}
