
using TribalWarsBot;

Console.WriteLine("Hello, World!");
ScavengeModule scavengeModule = new();
do
{
    Thread.Sleep(30000);
    scavengeModule.Refresh();
} while (true);

