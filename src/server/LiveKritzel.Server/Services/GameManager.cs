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
        private int _playerGuesses;
        private int _actualPlayerNumber;

        private CancellationTokenSource _cts;
        private Task _actualDispatcher;


        private bool playerIsChoosing = false;
        public int RoundDuration { get; set; } = 80;
        public (string Name, string ConId) ActualPlayer { get; private set; }
        public IEnumerable<string> Users => _users.Select(n => n.Name);
        public GameManager(IHubContext<GameHub> context, WordManager wordManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _wordManager = wordManager ?? throw new ArgumentNullException(nameof(wordManager));
            _users = new List<(string Name, string ConId)>();
            _playerGuesses = 0;
            _actualPlayerNumber = 0;
        }

        public bool PredictWord(string word, string name)
        {
            if (_wordManager.ActualWord == word)
            {
                _context.Clients.All.SendAsync("PlayerGotTheWord", name);
                _playerGuesses++;
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
            if (Users.Count() == 2)
            //Todo: maybe later solve concurrency problem.
            {
                StartGame();
            }
        }

        public void PlayerLeftTheGame(string conId)
        {
            _users.Remove(_users.FirstOrDefault(u => u.ConId == conId));
            if (_users.Count <= 1)
            {
                StopGame();
            }
        }


        public void StartGame()
        {
            _cts = new CancellationTokenSource();
            _actualPlayerNumber = 0;
            _actualDispatcher = new Task(GameLoop, _cts.Token, TaskCreationOptions.LongRunning);
            _actualDispatcher.Start();
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
            while (!token.IsCancellationRequested)
            {
                playerIsChoosing = true;
                _playerGuesses = 0;
                if (_actualPlayerNumber > _users.Count-1)
                {
                    _actualPlayerNumber = 0;
                }
                ActualPlayer = _users[_actualPlayerNumber];
                _actualPlayerNumber++;
                await _context.Clients.Client(ActualPlayer.ConId).SendAsync("StartChoosing", _wordManager.GetWordsToChoose(3))
                    .ConfigureAwait(false);
                while (playerIsChoosing)
                {
                    await Task.Delay(10)
                        .ConfigureAwait(false);
                }
                await _context.Clients.All.SendAsync("NewRoundIsStarted", _wordManager.ActualWord, RoundDuration)
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
                    if (_playerGuesses == _users.Count-1)
                        break;
                }
                await _context.Clients.All.SendAsync("RoundIsFinshed", _wordManager.ActualWord)
                    .ConfigureAwait(false);
            }

        }

        public void Dispose()
        {
            _cts?.Dispose();
            _actualDispatcher?.Dispose();
        }
    }

}
