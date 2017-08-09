using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace SendCloudToDevice
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=marsiotdfw.azure-devices.net;SharedAccessKeyName=coffeeclient;SharedAccessKey=rwKwYhCLhsuB1L6Hs8STUVf/EuSPBPQWmN5SjKGWvkg=";

        static void Main(string[] args)
        {
            Console.WriteLine("Send Cloud-to-Device message\n");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            ReceiveFeedbackAsync();

            Console.WriteLine("Press any key to send a C2D message.");
            Console.ReadLine();
            SendCloudToDeviceMessageAsync().Wait();
            Console.ReadLine();
        }

        private async static Task SendCloudToDeviceMessageAsync()
        {
            var commandMessage = new Message(Encoding.ASCII.GetBytes("{\"Command\":\"Brew\",\"Team\":\"Team01\",\"Parameters\":\"YO YO YO\"}"));
            commandMessage.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("coffeepot", commandMessage);
        }

        private async static void ReceiveFeedbackAsync()
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            Console.WriteLine("\nReceiving c2d feedback from service");
            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received feedback: {0}", string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode)));
                Console.ResetColor();

                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }
    }
}
