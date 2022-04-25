namespace RE_BL_USBMach_2._1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Device.Net;

    using Hid.Net.Windows;

    internal class CNCDevice
    {
        private IDevice CardDevice;
        private Thread bufferInThread;
        private readonly bool DoLoop = true;

        #region Delegates

        // Delegate to handle incoming frames.
        public event EventHandler<FrameInEventArgs> FrameInEvent;

        protected virtual void RaiseFrameInEvent(byte[] data)
        {
            EventHandler<FrameInEventArgs> handler = FrameInEvent;

            if (handler != null)
            {
                FrameInEventArgs e = new FrameInEventArgs(data);
                handler(this, e);
            }
        }

        // Delegate to handle outgoing frames.
        public event EventHandler<FrameOutEventArgs> FrameOutEvent;

        protected virtual void RaiseFrameOutEvent(byte[] data)
        {
            EventHandler<FrameOutEventArgs> handler = FrameOutEvent;

            if (handler != null)
            {
                FrameOutEventArgs e = new FrameOutEventArgs(data);
                handler(this, e);
            }
        }

        #endregion

        public CNCDevice()
        {
            InitializeDeviceAsync();        
        }

        // Initialize the card and start the read buffer loop.
        public async void InitializeDeviceAsync()
        {
            try
            {
                WindowsHidDeviceFactory.Register();
                List<FilterDeviceDefinition> deviceDefinitions = new List<FilterDeviceDefinition> { new FilterDeviceDefinition { DeviceType = DeviceType.Hid, VendorId = 0x3698, ProductId = 0x0107, Label = "Model 1" } };
                List<IDevice> devices = await DeviceManager.Current.GetDevicesAsync(deviceDefinitions);
                CardDevice = devices.FirstOrDefault();
                await CardDevice.InitializeAsync();

                Console.WriteLine("Done");

                bufferInThread = new Thread(new ThreadStart(ReadBufferLoop));
                bufferInThread.Start();

                _ = WriteBufferAsync(new byte[64] { 0x80, 0x07, 0x00, 0x00, 0x2b,
                0x00, 0x80, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6f, 0x12, 0x83, 0x3a, 0x00, 0x6f,
                0x12, 0x83, 0x3a, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, });

            }
            catch
            {
            }
        }

        // Continuously read the input buffer on a separate thread.
        private async void ReadBufferLoop()
        {
            while (DoLoop)
            {
                byte[] buffer = await CardDevice.ReadAsync();

                if (buffer != null)
                {
                    RaiseFrameInEvent(buffer);
                }

                Thread.Sleep(100);
            }
        }

        public void SendFrame(byte[] buffer)
        {
            _ = WriteBufferAsync(buffer);
        }

        // Write the data to the buffer and then call back to the UI.
        private async Task WriteBufferAsync(byte[] data)
        {
            await CardDevice.WriteAsync(data);
            RaiseFrameOutEvent(data);
        }
    }
}