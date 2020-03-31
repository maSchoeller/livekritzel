using LiveKritzel.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveKritzel.Server.Hubs
{
    public interface IGameHubClient
    {
        Task ReceiveLine(Line line);
        Task ReceiveChatMessage(string name, string message);

        Task ReceivePlayerJoined(string name);
        Task ReceivePlayerLeft(string name);

        Task PlayerGetTheWord(string name);
        Task StartChoosing(string[] words);
        Task RoundIsFinshed(string word);
        Task NewRoundIsStarted(int wordcount, int duration);
    }
}
