using System;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace Alarm.Model
{
    using Abstraction;

    /// <summary>
    /// Класс таймера реализующий способ получения времени с сервиса по типу ntp0.ntp-servers.net или time.windows.com.
    /// </summary>
    public class TimerNTP : Timer
    {
        private readonly string url = "ntp0.ntp-servers.net";

        protected override DateTime GetTime()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            byte[] data = new byte[48];
            data[0] = 0x1B;

            var addresses = Dns.GetHostEntry(url).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);

            using (Socket socket = new Socket(addresses[0].AddressFamily, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);
                socket.ReceiveTimeout = 3000;
                socket.Send(data);
                socket.Receive(data);
                socket.Close();
            }

            var intPart = (ulong)data[40] << 24 | (ulong)data[41] << 16 | (ulong)data[42] << 8 | data[43];
            var fractPart = (ulong)data[44] << 24 | (ulong)data[45] << 16 | (ulong)data[46] << 8 | data[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            DateTime networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);
            
            stopwatch.Stop();
            return networkDateTime.ToLocalTime().AddMilliseconds(-stopwatch.ElapsedMilliseconds);
        }
    }
}
