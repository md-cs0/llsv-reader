namespace LLSVReader;

// Error states.
public enum LLSVError
{
    OK = 0,     // Save file instantiated successfully.
    NOTEXISTS,  // The save file path is invalid.
    MAGIC,      // The magic of the save file is wrong.
    CORRUPT,     // Save file data is corrupted due to missing data.
    WRONGVER    // The given save file version number does not match what the game expects.
}

// LLSV header class.
public sealed class LLSVHeader
{
    // Internal binary reader.
    private BinaryReader Reader { get; init; }

    // Loaded save file's magic number.
    public int Magic { get; set; }

    // The version number of the save file.
    public byte Version { get; set; }

    // Save file flags.
    public byte[] Flags { get; set; }

    // Lives counter.
    public short Lives { get; set; }

    // Coins counter.
    public short Coins { get; set; }

    // Score counter.
    public uint Score { get; set; }

    // Current power-up.
    public byte Powerup { get; set; }

    // Number of worlds.
    public byte NumWorlds { get; set; }

    // Wrap around a loaded binary reader.
    public LLSVHeader(BinaryReader reader)
    {
        // Load the file stream.
        Reader = reader;

        // Read header properties.
        Magic = Reader.ReadInt32();
        Version = Reader.ReadByte();
        Flags = Reader.ReadBytes(15);
        Lives = Reader.ReadInt16();
        Coins = Reader.ReadInt16();
        Score = Reader.ReadUInt32();
        Powerup = Reader.ReadByte();
        NumWorlds = Reader.ReadByte();

        // Skip padding.
        Reader.ReadInt16();
    }

    // Length of LLSV header.
    public static int SizeOfHeader { get; } = 0x20;
}

// Save file class.
public sealed class LLSV
{
    // LLSV magic.
    public static int Magic { get; } = 0x56534C4C;

    // Current LLSV save file version number.
    public static int CurrentVersion { get; } = 1;

    // Internal file stream instance.
    private FileStream? _stream;

    // LLSV header instance.
    public LLSVHeader? Header { get; private set; }

    // Current level information.
    public List<byte> CurrentLevel { get; set; } = [];

    // Error state.
    public LLSVError ErrorState { get; private set; } = LLSVError.OK;

    // Open an existing save file.
    public static LLSV? Open(string path)
    {
        // Check if the file path exists first.
        if (!File.Exists(path))
            return null;

        // Read the given save file.
        var save = new LLSV
        {
            _stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite)
        };
        using var reader = new BinaryReader(save._stream);

        // If the file size is too small, the header cannot be read fully.
        if (save._stream.Length < LLSVHeader.SizeOfHeader)
        {
            save.ErrorState = LLSVError.CORRUPT;
            return save;
        }

        // Read the header.
        save.Header = new LLSVHeader(reader);

        // Verify the magic.
        if (save.Header.Magic != Magic)
        {
            save.ErrorState = LLSVError.MAGIC;
            return save;
        }

        // Verify the version number. This won't cause the save file to exit prematurely.
        if (save.Header.Version != CurrentVersion)
            save.ErrorState = LLSVError.WRONGVER;

        // Read until the number of worlds is reached.
        for (int i = 0; i < save.Header.NumWorlds; ++i)
        {
            try
            {
                byte level = reader.ReadByte();
                save.CurrentLevel.Add(level);
            }
            catch (EndOfStreamException)
            {
                save.ErrorState = LLSVError.CORRUPT;
                return save;
            }
        }

        // Return the save file.
        return save;
    }

    // Write to the loaded save file.
    public void Write()
    {
        // Null-check the stream and header and create a new binary writer instance.
        if (_stream == null)
            return;
        if (Header == null)
            return;
        using var writer = new BinaryWriter(_stream);

        // Clear the save file.
        _stream.SetLength(0);

        // Write the LLSV header.
        writer.Write(Header.Magic);
        writer.Write(Header.Version);
        writer.Write(Header.Flags);
        writer.Write(Header.Lives);
        writer.Write(Header.Coins);
        writer.Write(Header.Score);
        writer.Write(Header.Powerup);
        writer.Write(Header.NumWorlds);

        // Write the current level array.
        writer.Write(CurrentLevel.ToArray());
    }

    // Close the save file.
    public void Close()
    {
        _stream?.Close();
    }
}