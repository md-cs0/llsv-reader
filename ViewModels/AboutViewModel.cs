using LLSVReader.Views;

namespace LLSVReader.ViewModels;

public partial class AboutViewModel : ViewModelBase
{
    // Version text for the about dialogue.
    public static string VersionText { get; } = $"""
                                                Lost Levels SaVe file (LLSV) reader for reading and modifying save files
                                                
                                                v{Version.Major}.{Version.Minor}.{Version.Revision}
                                                """;

    // Close the about dialogue.
    public void CommandOK(About window)
    {
        window.Close();
    }
}
