using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KostalApiClient.Api;
using KostalApiClient.Model;

namespace KostalApiClient.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task t = MainAsync(args);
            t.Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            try
            {
                Session session = new Session(your_host);
                await Test(session);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static async Task Test(Session session)
        {
            // Get device infos
            InfoVersion infos = await session.Info.GetVersion();
            Console.WriteLine($"Session to : {infos.Hostname} - {infos.Name}. Api version : {infos.ApiVersion}. Software version : {infos.SoftwareVersion}");

            // Log into device
            await session.Login(your_password);
            Me me = await session.Auth.GetMe();
            Console.WriteLine($"User : {me.Role}. Authenticated : {me.Authenticated}.");
            await session.System.Reboot();
            // Get log datas
            LogDatas logDatas = await session.LogData.GetLogData(DateTime.Today, DateTime.Today);
            Console.WriteLine("Datas : " + logDatas.Datas.Count);
            Console.WriteLine("Summaries : " + logDatas.Summaries.Count);
            Console.WriteLine("Errors : " + logDatas.Errors.Count);

            // Get last events 
            Console.WriteLine("Events :");
            List<Event> events = await session.Events.GetLastest("fr-fr", 10);
            foreach (Event @event in events)
            {
                Console.WriteLine($"> {@event.StartTime} : {@event.Category}\t{@event.Category}\t{@event.Code}\t{@event.Description}");
            }

            // Get last values for AC
            Console.WriteLine("AC Values :");
            for (int i = 0; i < 5; i++)
            {
                Console.Write("> ");
                List<ProcessModuleData> processModuleDatas = await session.ProcessData.GetProcessDataIdentifiers("devices:local:ac", new List<string> { "CosPhi","Frequency","L1_I","L1_P","L1_U","L2_I","L2_P","L2_U","L3_I","L3_P","L3_U","P","Q","S"});
                foreach (ProcessModuleData processModuleData in processModuleDatas)
                {
                    foreach (ProcessData processData in processModuleData.ProcessDatas)
                    {
                        Console.Write($"{processData.Id} : {processData.Value}\t");
                    }
                }
                Console.WriteLine("");
                await Task.Delay(1000);
            }

            // Logout
            await session.Auth.Logout();
        }
    }
}