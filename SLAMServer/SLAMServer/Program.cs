using Microsoft.AspNetCore.Builder;
using System.Net;
using System.Net.Sockets;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// -----------------------------
// 1. Minimal API (HTTP)
// -----------------------------
app.MapGet("/health", () => "OK");
app.MapGet("/status", () => new
{
    Time = DateTime.UtcNow,
    Server = "SLAM Game Server"
});

// Start Kestrel **without blocking**
var httpTask = app.RunAsync();   // Important: RunAsync()

// -----------------------------
// 2. Your TCP/WebSocket server
// -----------------------------
var ipEndPoint = new IPEndPoint(IPAddress.Any, 13);
TcpListener listener = new(ipEndPoint);
listener.Start();

Console.WriteLine("TCP/WebSocket server started on port 13.");

_ = Task.Run(async () =>
{
    while (true)
    {
        using TcpClient client = await listener.AcceptTcpClientAsync();
        await using NetworkStream stream = client.GetStream();

        var message = $"📅 {DateTime.Now} 🕛";
        var dateBytes = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(dateBytes);

        Console.WriteLine($"Sent message: \"{message}\"");
    }
});

// -----------------------------
// 3. Wait for shutdown
// -----------------------------
await httpTask;