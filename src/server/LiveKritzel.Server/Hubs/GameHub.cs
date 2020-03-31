using LiveKritzel.Server.Models;
using LiveKritzel.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;

namespace LiveKritzel.Server.Hubs
{
    public class GameHub : Hub<IGameHubClient>
    {
        private readonly GameManager _gameManager;
        private readonly WordManager _wordManager;

        public GameHub(GameManager gameManager, WordManager wordManager)
        {
            _gameManager = gameManager;
            _wordManager = wordManager;
        }


        public async Task SendLine(Line line)
        {
            await Clients.Others.ReceiveLine(line)
                .ConfigureAwait(false);
        }

        public async Task ClearCanvas()
        {
            await Clients.Others.ReceiveClearCanvas().ConfigureAwait(false);
        }

        public async Task SendFillCanvas(string color)
        {
            await Clients.Others.ReceiveFillCanvas(color).ConfigureAwait(false);
        }

        public async Task SendChatMessage(string message)
        {
            if (!_gameManager.PredictWord(message, GetName()))
            {
                await Clients.Others.ReceiveChatMessage(GetName(), message)
                    .ConfigureAwait(false);
            }
        }

        public async Task JoinGame(string name)
        {
            Context.Items["name"] = name;
            await Clients.Others.ReceivePlayerJoined(name)
                .ConfigureAwait(false);
        }

        public void ChooseWord(string word)
        {
            _wordManager.SetActualWord(word);
            _gameManager.StartRound();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        { 
            await Clients.Others.ReceivePlayerLeft(GetName())
               .ConfigureAwait(false);

        }


        private string GetName()
        {
            return (string)Context.Items["name"];
        }
    }
}
