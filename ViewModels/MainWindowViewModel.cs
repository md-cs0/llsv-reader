// TODO - learn MVVM better (particularly DI/IoC)?

using System.Threading.Tasks;
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
    public async Task CommandOpen(MainWindow window)
    {
        // If a save is already open, request the user as to whether they would like to
        // save it.
        if (Save != null && !await SavePrompt(window))
            return;

        // Locate the save file to open.
        var files = await window.StorageProvider.OpenFilePickerAsync(
            new Avalonia.Platform.Storage.FilePickerOpenOptions
            {
                Title = "Open LLSV file",
                AllowMultiple = false
            });
        if (files.Count == 0)
            return;

        // Open the given save file.
        Save = LLSV.Open(files[0].Path.AbsolutePath);
    }

    // Close a loaded save file, if loaded.
    public async Task CommandClose(MainWindow window)
    {
        // If a save is already open, request the user as to whether they would like to
        // save it.
        if (Save != null && !await SavePrompt(window))
            return;

        // Close the loaded save file.
        Save?.Close();
        Save = null;
    }

    // Exit.
    public async Task CommandExit(MainWindow window)
    {
        // If a save is already open, request the user as to whether they would like to
        // save it.
        if (Save != null && !await SavePrompt(window))
            return;

        // Exit.
        Environment.Exit(0);
    }

    // Launch the about dialogue.
    public void CommandAbout(MainWindow window)
    {
        var dialogue = new About
        {
            DataContext = new AboutViewModel()
        };
        dialogue.ShowDialog(window);
    }

    // If a save file is open, ask whether to save it before closing it.
    private async Task<bool> SavePrompt(Window window)
    {
        var result = await MessageBox.ShowDialog(window,
                                                 "Should the currently-open save file be saved?",
                                                 "LLSV file prompt",
                                                 MessageBoxButtons.YesNoCancel,
                                                 MessageBoxIcon.Question);
        if (result == MessageBoxResult.Cancel)
            return false;
        if (result == MessageBoxResult.Yes)
            Save?.Write();
        return true;
    }
}
