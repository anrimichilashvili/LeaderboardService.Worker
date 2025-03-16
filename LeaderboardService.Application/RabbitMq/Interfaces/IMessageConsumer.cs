using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LeaderboardService.Application.RabbitMq.Interfaces
{
    public interface IMessageConsumer
    {
        void StartConsumingWithChannel<T>(Action<T> onMessage);
    }
}
