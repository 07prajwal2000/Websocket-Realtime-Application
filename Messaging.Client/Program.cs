using ImGuiNET;
using Messaging.Client;
using Messaging.Shared;
using Raylib_cs;
using rlImGui_cs;
using Websocket.Client;

var client = new Client();

client.Start();

Console.ReadKey();

client.Stop();

return;

//bool connected = false;
//var messages = new List<string>();

//using var client = new WebsocketClient(new Uri("wss://localhost:5001/ws"));

//client.MessageReceived.Subscribe(x => OnMessageReceived(x));
//client.DisconnectionHappened.Subscribe(x => { connected = false; });


//Raylib.InitWindow(640, 420, "Socket - client");
//rlImGui.Setup();
//while (!Raylib.WindowShouldClose())
//{
//    Raylib.BeginDrawing();
//    Raylib.ClearBackground(Color.BLANK);
//    rlImGui.Begin();
//    DrawGui();
//    rlImGui.End();

//    Raylib.EndDrawing();
//}
//rlImGui.Shutdown();
//Raylib.CloseWindow();
//client.Dispose();

//void DrawGui()
//{
//    if (!connected && ImGui.Button("Connect"))
//    {
//        Connect();
//        connected = true;
//    }
//    else if (connected && ImGui.Button("Disconnect"))
//    {
//        Disconnect();
//    }
//    ImGui.BeginChild(100);
//    foreach (var msg in messages)
//    {
//        ImGui.Text(msg);
//    }
//    ImGui.EndChild();
//}

//void Disconnect()
//{
//    if (!connected) return;
//    client.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "").GetAwaiter().GetResult();
//    connected = false;
//}
//void Connect()
//{
//    if (connected) return;
//    client.Start();
//    connected = true;
//}

//void OnMessageReceived(ResponseMessage message)
//{
//    Console.WriteLine("Message received");
//    using var packet = NetworkPacket.CreateReadPacket(message.Binary!);
//    packet.ReadInt();
//    var msg = packet.ReadString();

//    messages.Add(msg);
//}