using LiveKritzel.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace LiveKritzel.Server.Services
{
    public class DrawingService
    {
        private event EventHandler<Point> _newPoint;

        public DrawingService()
        {

        }


        public void PublishPoint(Point point)
        {
            _newPoint?.Invoke(this, point);
        }

        public void GetDrawingStream([EnumeratorCancellation]CancellationToken token)
        {
            Point? actualpoint = null;
            _newPoint += (s, e) =>
            {
                actualpoint = e;
            };

            while (!token.IsCancellationRequested)
            {

                while (actualpoint is null)
                {
                    await Task.Delay(10)
                        .ConfigureAwait(false);
                }

                yield return actualpoint;
                actualpoint = null;
            }
        }
    }


}
