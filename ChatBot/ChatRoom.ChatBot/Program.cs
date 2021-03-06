﻿using ChatRoom.ChatBot.Domain;
using ChatRoom.ChatBot.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ChatRoom.ChatBot
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        static void Main(string[] args)
        {
            RegisterServices();

            _serviceProvider.GetService<ChatCommandReceiver>()
                            .Register();

            Console.WriteLine(" ChatBot Microservice");
            Console.WriteLine(" Press ctrl+c to exit.");
            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<ChatCommandReceiver>();
            collection.AddTransient<BotResponseSender>();
            collection.AddSingleton<BotService>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.Local.json", optional: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(AspNetCoreEnvironment)}.json", optional: true)
                .AddEnvironmentVariables()
                            .Build();

            collection.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

            collection.Configure<ChatBotSettings>(configuration.GetSection("ChatBots"));
            
            collection.AddLogging();

            _serviceProvider = collection.BuildServiceProvider();
        }
    }
}
