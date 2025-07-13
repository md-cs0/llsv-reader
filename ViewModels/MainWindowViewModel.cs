using LLSVReader.Views;

namespace LLSVReader.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Is a file currently open?
    public bool FileOpen { get; private set; } = false;

    // Open a new save file.
    public void CommandOpen()
    {
    }

    // Close a loaded save file, if loaded.
    public void CommandClose()
    {
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
