using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace HamLoggerGateway.MessageProcessors.N1MM.Schema;

/// <summary>
///     Represents radio information sent via UDP messages, detailing specific radio settings and states.
///     This includes information such as radio frequency, mode, and other relevant settings.
/// </summary>
[XmlRoot("RadioInfo")]
public class RadioInfo
{
    /// <summary>
    ///     Short name of the logger sending the packet. Examples: “N1MM”, “DXLab”, “Logger32”, “UcxLog”, “Log4OM”
    /// </summary>
    [XmlElement("app")]
    public string App { get; set; } = string.Empty;

    /// <summary>
    ///     The NetBios name of the computer that is sending these messages.
    /// </summary>
    /// <remarks>
    ///     It is the name used in Multi-Computer networking. Windows limits it to 15 characters. If the computer name is
    ///     greater than 15 characters long, the first 15 characters will be used.
    /// </remarks>
    [XmlElement("StationName")]
    public string StationName { get; set; } = string.Empty;

    /// <summary>
    ///     The radio number associated with a specific XML packet – in other words, the source of the information in that
    ///     packet. When in SO2V or SO2R mode, N1MM+ sends two packets every ten seconds – one packet each from RadioNr1 and
    ///     RadioNr2
    /// </summary>
    [XmlElement("RadioNr")]
    public int RadioNr { get; set; }

    /// <summary>
    ///     The receive frequency represented as values to the tens digit with no delimiter
    /// </summary>
    [XmlElement("Freq")]
    public long Frequency { get; set; }

    /// <summary>
    ///     The transmit frequency represented as values to the tens digit with no delimiter.
    /// </summary>
    [XmlElement("TXFreq")]
    public long TXFreq { get; set; }

    /// <summary>
    ///     The mode of operation (e.g., CW, SSB).
    /// </summary>
    /// <remarks>
    ///     Could be any one of the following: CW, USB, LSB, RTTY, PSK31, PSK63, PSK125, PSK250, QPSK31, QPSK63, QPSK125,
    ///     QPSK250, FSK, GMSK, DOMINO, HELL, FMHELL, HELL80, MT63, THOR, THRB, THRBX, OLIVIA, MFSK8, MFSK16
    /// </remarks>
    [XmlElement("Mode")]
    public string Mode { get; set; } = string.Empty;

    /// <summary>
    ///     The callsign entered by the operator
    /// </summary>
    [XmlElement("OpCall")]
    public string OpCall { get; set; } = string.Empty;

    /// <summary>
    ///     A value indicating whether the radio is in run mode ("True" or "False").
    /// </summary>
    [XmlElement("IsRunning")]
    [JsonIgnore] // Exclude from JSON serialization
    public string IsRunningString
    {
        get => IsRunning.ToString();
        set => IsRunning = Convert.ToBoolean(value);
    }

    /// <summary>
    ///     A value indicating whether the radio is in run mode (true or false).
    /// </summary>
    /// <remarks>
    ///     This property works in addition to <see cref="IsRunningString" /> for supporting inconsistent boolean types
    /// </remarks>
    [XmlIgnore] // Exclude from XML serialization
    public bool IsRunning { get; set; }

    /// <summary>
    ///     The Windows assigned handle of the Entry Window with program focus.
    /// </summary>
    [XmlElement("FocusEntry")]
    public int FocusEntry { get; set; }

    /// <summary>
    ///     Allows external software to send commands to a specific EntryWindow
    /// </summary>
    [XmlElement("EntryWindowHwnd")]
    public int EntryWindowHwnd { get; set; }

    /// <summary>
    ///     The currently selected antenna for this radio. (0-63)
    /// </summary>
    [XmlElement("Antenna")]
    public int Antenna { get; set; }

    /// <summary>
    ///     The currently selected rotor from the Antenna table in the Configurer.
    /// </summary>
    [XmlElement("Rotors")]
    public string Rotors { get; set; } = string.Empty;

    /// <summary>
    ///     Receive Radio Focus
    /// </summary>
    [XmlElement("FocusRadioNr")]
    public int FocusRadioNr { get; set; }

    /// <summary>
    ///     Audio switching for SO2R operation
    /// </summary>
    [XmlElement("IsStereo")]
    [JsonIgnore] // Exclude from JSON serialization
    public string IsStereoString
    {
        get => IsStereo.ToString();
        set => IsStereo = Convert.ToBoolean(value);
    }

    /// <summary>
    ///     Gets or sets a value indicating whether the radio is in run mode (true or false).
    /// </summary>
    /// <remarks>
    ///     This property works in addition to <see cref="IsRunningString" /> for supporting inconsistent boolean types
    /// </remarks>
    [XmlIgnore] // Exclude from XML serialization
    public bool IsStereo { get; set; }


    /// <summary>
    ///     A value indicating whether the transceiver is in Split VFO mode.
    /// </summary>
    [XmlElement("IsSplit")]
    [JsonIgnore] // Exclude from JSON serialization
    public string IsSplitString
    {
        get => IsSplit.ToString();
        set => IsSplit = Convert.ToBoolean(value);
    }

    /// <summary>
    ///     A value indicating whether the transceiver is in Split VFO mode.
    /// </summary>
    /// <remarks>
    ///     This property works in addition to <see cref="IsSplitString" /> for supporting inconsistent boolean types
    /// </remarks>
    [XmlIgnore] // Exclude from XML serialization
    public bool IsSplit { get; set; }

    /// <summary>
    ///     The Transmit Radio Focus
    /// </summary>
    [XmlElement("ActiveRadioNr")]
    public int ActiveRadioNr { get; set; }

    /// <summary>
    ///     A value indicating if the program is in transmit (True) or receive (False) mode
    /// </summary>
    [XmlElement("IsTransmitting")]
    [JsonIgnore] // Exclude from JSON serialization
    public string IsTransmittingString
    {
        get => IsTransmitting.ToString();
        set => IsTransmitting = Convert.ToBoolean(value);
    }

    /// <summary>
    ///     A value indicating if the program is in transmit (True) or receive (False) mode
    /// </summary>
    /// <remarks>
    ///     This property works in addition to <see cref="IsTransmitting" /> for supporting inconsistent boolean types
    /// </remarks>
    [XmlIgnore] // Exclude from XML serialization
    public bool IsTransmitting { get; set; }

    /// <summary>
    ///     The label of the Function Key that was pressed to initiate this transmission
    /// </summary>
    [XmlElement("FunctionKeyCaption")]
    public string FunctionKeyCaption { get; set; } = string.Empty;

    /// <summary>
    ///     The name of the radio, as shown in the Entry Window
    /// </summary>
    [XmlElement("RadioName")]
    public string RadioName { get; set; } = string.Empty;

    /// <summary>
    ///     The auxiliary antenna selected number.
    /// </summary>
    [XmlElement("AuxAntSelected")]
    public int AuxAntSelected { get; set; } = -1;

    /// <summary>
    ///     The auxiliary antenna selected name.
    /// </summary>
    [XmlElement("AuxAntSelectedName")]
    public string AuxAntSelectedName { get; set; } = string.Empty;

    /// <summary>
    ///     A value indicating whether the radio is connected.
    /// </summary>
    /// <remarks>It may take some time for a disconnection (False) to be registered</remarks>
    [XmlElement("IsConnected")]
    [JsonIgnore]
    public string IsConnectedString
    {
        get => IsConnected.ToString();
        set => IsConnected = Convert.ToBoolean(value);
    }

    /// <summary>
    ///     A value indicating whether the radio is connected.
    /// </summary>
    /// <remarks>It may take some time for a disconnection (False) to be registered</remarks>
    [XmlIgnore]
    public bool IsConnected { get; set; }
}