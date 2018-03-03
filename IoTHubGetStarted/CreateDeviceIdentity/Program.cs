using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using System;
using System.Threading.Tasks;

namespace CreateDeviceIdentity
{
    class Program
    {
        // PVPGJJyvZHwJiznDD+K/TxmaXNk5npGAdpnQh86AUm8=
        static RegistryManager registryManager;
        static string connectionString = "HostName=marstest.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=6nF0ZikGJ0lXBDswcwiuTwghrU6Z8crH62jWfdyJxBw=";

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
            Console.ReadLine();
        }

        private static async Task AddDeviceAsync()
        {
            string deviceId = "myFirstDevice";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}
