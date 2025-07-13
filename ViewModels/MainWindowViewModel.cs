using Classic.CommonControls.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using LLSVReader.Views;

namespace LLSVReader.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Is a save currently open?
    [ObservableProperty] // The WPF in me is pleading in joy.
    private LLSV? _save;

    // Open a new save file.
    public void CommandOpen()
    {
        Window? window = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow; // Yeah.
        Save = LLSV.Open("dev/newdev.sav");
        MessageBox.ShowDialog(window, $"{Save != null} {((Save != null) ? Save.ErrorState : -1)}", "test", MessageBoxButtons.Ok, MessageBoxIcon.Information);
    }

    // Close a loaded save file, if loaded.
    public void CommandClose()
    {
        Save?.Close();
        Save = null;
    }

    // Exit.
    public void CommandExit()
    {
        Environment.Exit(0);
    }

    // Launch the about dialogue.
    public void CommandAbout(MainWindow window)
    {
        var dialogue = new About();
        dialogue.DataContext = new AboutViewModel();
        dialogue.ShowDialog(window);
    }
}
