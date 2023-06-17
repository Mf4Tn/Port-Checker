namespace System.Net.NetworkInformation
{
    internal class IPEndPoint
    {
        public static int MinPort { get; internal set; }
        public static int MaxPort { get; internal set; }

        internal static string GetServiceName(int port)
        {
            throw new NotImplementedException();
        }
    }
}