using LiveKritzel.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LiveKritzel.Server.Services
{
    public class GameManager
    {
        private readonly IHubContext<GameHub> _context;
        private readonly PlayerManager _playerManager;
        private readonly WordManager _wordManager;

        private CancellationTokenSource _cts;
        private Task _actualDispatcher;

        private (string Name, string ConId) _actualPlayer;


        public int RoundDuration { get; set; } = 80;

        public bool IsRunning { get; private set; }
        public GameManager(IHubContext<GameHub> context, PlayerManager playerManager, WordManager wordManager)
        {
            _context = context;
            _playerManager = playerManager;
            _wordManager = wordManager;
            IsRunning = false;
        }

        public void PredictWord(string word, string conId)
        {
            if (_wordManager.ActualWord == word)
            {
                _context.Clients.All.SendAsync("GetTheWord", _playerManager.GetNameById(conId));
            }
        }

        public void StartGame()
        {
            if (!IsRunning)
            {
                _cts = new CancellationTokenSource();
                _actualDispatcher = new Task(GameLoop, _cts.Token, TaskCreationOptions.LongRunning);
                //Start Game
                IsRunning = true;

            }
        }

        public void StopGame()
        {
            if (IsRunning && _playerManager.PlayerCount == 1)
            {
                _cts.Cancel();
                //Stop Gamne
                IsRunning = false;
            }
        }



        private async void GameLoop()
        {
            var token = _cts.Token;
            while (token.IsCancellationRequested)
            {
                var i = RoundDuration;
                for (int d = 0; d < i; d++)
                {
                    if (d% (RoundDuration / 4) == 0)
                    {
                        //Send Tipp
                    }
                    await Task.Delay(1000)
                        .ConfigureAwait(false);
                    //Update Time
                }

                await _context.Clients.All.SendAsync("RoundOver")
                    .ConfigureAwait(false);
                var player = _playerManager.NexPlayer();
                _actualPlayer = player;
            }

        }
      


        //public IEnumerable<(int Sore, string ConId, string Name)> GetScores()
        //{

        //}

    }

}
