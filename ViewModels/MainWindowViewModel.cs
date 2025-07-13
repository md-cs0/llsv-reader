using System.Runtime.InteropServices;

namespace LLSVReader.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Used for testing.
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(IntPtr hWnd, string lpText, string lpCaption, uint uType);

    // Is a file currently open?
    public bool FileOpen { get; set; } = false;

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
    public void CommandAbout()
    {
        MessageBoxW(IntPtr.Zero, "Lost Levels Save File (LLSV) reader", "About LLSVReader", 0);
    }
}
