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
            if (_gameManager.ActualPlayer.ConId == Context.ConnectionId)
            {
                await Clients.Others.ReceiveLine(line)
                    .ConfigureAwait(false);

            }
        }

        public async Task ClearCanvas()
        {
            if (_gameManager.ActualPlayer.ConId == Context.ConnectionId)
            {
                await Clients.Others.ReceiveClearCanvas().ConfigureAwait(false);
            }
        }

        public async Task SendFillCanvas(string color)
        {
            if (_gameManager.ActualPlayer.ConId == Context.ConnectionId)
            {
                await Clients.Others.ReceiveFillCanvas(color).ConfigureAwait(false);
            }
        }

        public async Task<bool> SendChatMessage(string message)
        {
            if (!_gameManager.PredictWord(message, GetName()))
            {
                await Clients.Others.ReceiveChatMessage(GetName(), message)
                    .ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<string>> JoinGame(string name)
        {
            Context.Items["name"] = name;
            _gameManager.PlayerJoinedTheGame(name, Context.ConnectionId);
            await Clients.Others.ReceivePlayerJoined(name)
                .ConfigureAwait(false);
            return _gameManager.Users.Where(n => n != name).ToArray();
        }

        public void ChooseWord(string word)
        {
            if (_gameManager.ActualPlayer.ConId == Context.ConnectionId)
            {
                _wordManager.SetActualWord(word);
                _gameManager.StartRound();
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _gameManager.PlayerLeftTheGame(Context.ConnectionId);
            await Clients.Others.ReceivePlayerLeft(GetName())
               .ConfigureAwait(false);
            //Todo: stop if the last player left the game

        }


        private string GetName()
        {
            return (string)Context.Items["name"];
        }
    }
}
