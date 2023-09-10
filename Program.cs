﻿using OpenTelemetry;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;

class Program
{
    static Meter s_meter = new("HatCo.HatStore", "1.0.0");
    static Counter<int> s_hatsSold = s_meter.CreateCounter<int>(
        name: "hats-sold",
        unit: "Hats",
        description: "The number of hats sold in our store");

    static void Main(string[] args)
    {
        using MeterProvider meterProvider = Sdk.CreateMeterProviderBuilder()
            .AddMeter("HatCo.HatStore")
            .AddPrometheusHttpListener(opt =>
            {
                opt.UriPrefixes = new string[] { $"http://localhost:9184/" };
            })
            .Build();

        var rand = Random.Shared;
        Console.WriteLine("Press any key to exit");
        while (true)
        {
            //// Simulate hat selling transactions.
            Thread.Sleep(rand.Next(100, 2500));
            s_hatsSold.Add(rand.Next(0, 1000));
        }
    }
}