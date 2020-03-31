using LiveKritzel.Server.Models;
using LiveKritzel.Server.Services;
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
    public class GameHub : Hub
    {
        private readonly GameManager _scoreManager;
        private readonly DrawingService _drawingService;
        private readonly PlayerManager _playerManager;

        public GameHub(GameManager scoreManager, DrawingService drawingService, PlayerManager playerManager)
        {
            _scoreManager = scoreManager;
            _drawingService = drawingService;
            _playerManager = playerManager;
        }

        public IAsyncEnumerable<Point> GetDrawingStream()
        {
            return _drawingService.GetDrawingStream(cancellationToken);
        }

        public async Task SetDrawingStream(IAsyncEnumerable<Point> pointStream)
        {
            if (pointStream is null)
            {
                throw new ArgumentNullException(nameof(pointStream));
            }

            await foreach (var item in pointStream)
            {
                _drawingService.PublishPoint(item);
            }
        }

        public Task GetChatMessage(string message)
        {

        }

        public Task JoinGame(string name)
        {
            _playerManager.JoinGame(name, Context.ConnectionId);
            return Task.CompletedTask;
        }



        public override Task OnDisconnectedAsync(Exception exception)
        {
            _playerManager.LeafGame(Context.ConnectionId);
            return Task.CompletedTask;
        }

    }
}
