namespace RE_BL_USBMach_2._1
{
    internal class FrameOutEventArgs
    {
        internal FrameOutEventArgs(byte[] data)
        {
            Data = data;
        }

        internal byte[] Data { get; private set; }
    }
}