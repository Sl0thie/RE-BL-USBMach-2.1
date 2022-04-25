namespace RE_BL_USBMach_2._1
{
    internal class FrameInEventArgs
    {
        internal FrameInEventArgs(byte[] data)
        {
            Data = data;
        }

        internal byte[] Data { get; private set; }
    }
}
