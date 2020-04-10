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
        Task ReceiveClearCanvas();
        Task ReceiveFillCanvas(string color);
        Task ReceiveChatMessage(string name, string message);

        Task ReceivePlayerJoined(string name);
        Task ReceivePlayerLeft(string name);

        Task PlayerGotTheWord(string name, int score);
        Task StartChoosing(string[] words);
        Task RoundIsFinshed(string word);
        Task NewRoundIsStarted(string word, int duration);

    }
}
