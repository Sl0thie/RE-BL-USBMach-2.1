namespace RE_BL_USBMach_2._1
{
    public enum ProgramState
    {
        Unknown,
        Idle,
        Jogging,

        CmdStop,
        CmdXPlus,
        CmdXMinus,
        CmdYPlus,
        CmdYMinus,
        CmdZPlus,
        CmdZMinus,
        CmdAPlus,
        CmdAMinus,
        CmdBPlus,
        CmdBMinus,

        InitializeStep1,
        InitializeStep2,
        InitializeStep3,
        InitializeStep4,

        SetEmergencyStopStep1,
        SetEmergencyStopStep2,
        SetEmergencyStopStep3,
        SetEmergencyStopStep4,

        ClearEmergencyStopStep1,
        ClearEmergencyStopStep2,
        ClearEmergencyStopStep3,
        ClearEmergencyStopStep4,
    }
}