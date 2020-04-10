using LiveKritzel.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LiveKritzel.Server.Services
{
    public class GameManager : IDisposable
    {
        private readonly IHubContext<GameHub> _context;
        private readonly WordManager _wordManager;
        private readonly List<(string Name, string ConId)> _users;


        private CancellationTokenSource _cts;
        private Task _actualDispatcher;


        private bool playerIsChoosing = false;
        public int RoundDuration { get; set; } = 80;
        public (string Name, string ConId) ActualPlayer { get; private set; }
        public IEnumerable<string> Users => _users.Select(n => n.Name).ToArray();
        public GameManager(IHubContext<GameHub> context, WordManager wordManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _wordManager = wordManager ?? throw new ArgumentNullException(nameof(wordManager));
            _users = new List<(string Name, string ConId)>();
        }

        public bool PredictWord(string word, string name)
        {
            if (_wordManager.ActualWord == word)
            {
                _context.Clients.All.SendAsync("PlayerGetTheWord", name);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PlayerJoinedTheGame(string name, string conId)
        {
            _users.Add((name, conId));
            if (_users.Count == 1)
            {
                StartGame();
            }
        }

        public void PlayerLeftTheGame(string conId)
        {
            _users.Remove(_users.FirstOrDefault(u => u.ConId == conId));
            if (_users.Count == 0)
            {
                StopGame();
            }
        }


        public void StartGame()
        {
            _cts = new CancellationTokenSource();
            _actualDispatcher = new Task(GameLoop, _cts.Token, TaskCreationOptions.LongRunning);
            //Start Game
        }

        public void StopGame()
        {
            _cts.Cancel();
            //Stop Gamne
        }

        public void StartRound()
        {
            playerIsChoosing = false;
        }


        private async void GameLoop()
        {
            var token = _cts.Token;
            var rnd = new Random();
            while (!token.IsCancellationRequested)
            {

                while (playerIsChoosing)
                {
                    await Task.Delay(10)
                        .ConfigureAwait(false);
                }

                await _context.Clients.All.SendAsync("NewRoundIsStarted", _wordManager.ActualWord.Length, RoundDuration)
                    .ConfigureAwait(false);
                var i = RoundDuration;
                for (int d = 0; d < i; d++)
                {
                    if (d % (RoundDuration / 4) == 0)
                    {
                        //Send Tipp to predict
                    }
                    await Task.Delay(1000)
                        .ConfigureAwait(false);
                }

                await _context.Clients.All.SendAsync("ReceiveClearCanvas")
                    .ConfigureAwait(false);
                await _context.Clients.All.SendAsync("RoundIsFinshed", _wordManager.ActualWord)
                    .ConfigureAwait(false);
                playerIsChoosing = true;
                ActualPlayer = _users[rnd.Next(0, _users.Count - 1)];
                await _context.Clients.Client(ActualPlayer.ConId).SendAsync("StartChoosing", _wordManager.GetWordsToChoose(3))
                    .ConfigureAwait(false);

            }

        }

        public void Dispose()
        {
            _cts.Dispose();
            _actualDispatcher.Dispose();
        }
    }

}
