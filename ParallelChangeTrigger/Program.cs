using System;
using System.Threading.Tasks;
using Client;

namespace ParallelChangeTrigger
{
    internal class Program
    {
        /// <summary>
        ///     Number of characters to create per type of character (e.g. X, Y, Z).
        /// </summary>
        private const int AmountOfCharactersPerType = 5;

        /// <summary>
        ///     Number of updates that each type of character group (e.g. X, Y, Z)
        ///     will undergo after all characters have been updated.
        /// </summary>
        private const int NumberOfUpdates = 1000;

        private static void Main()
        {
            Console.WriteLine("Press any key to start triggering changes. Abort using ctrl+c.");
            Console.ReadKey();

            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            var tasks = new[]
            {
                InvokeX(),
                InvokeY(),
                InvokeZ()
            };

            await Task.WhenAll(tasks);
        }

        private static async Task InvokeX()
        {
            var client = new XApiClient(new DefaultHttpClientFactory());

            for (var i = 0; i < AmountOfCharactersPerType; i++) await client.AddRandomCharacter();

            for (var i = 0; i < NumberOfUpdates; i++) await client.UpdateRandomCharacter();
        }

        private static async Task InvokeY()
        {
            var client = new YApiClient(new DefaultHttpClientFactory());

            for (var i = 0; i < AmountOfCharactersPerType; i++) await client.AddRandomCharacter();

            for (var i = 0; i < NumberOfUpdates; i++) await client.UpdateRandomCharacter();
        }

        private static async Task InvokeZ()
        {
            var client = new ZApiClient(new DefaultHttpClientFactory());

            for (var i = 0; i < AmountOfCharactersPerType; i++) await client.AddRandomCharacter();

            for (var i = 0; i < NumberOfUpdates; i++) await client.UpdateRandomCharacter();
        }
    }
}