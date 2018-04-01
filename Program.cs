using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace BanBoardBot
{
    class Program
    {
        static void Main(string[] args) =>
            new Program().StartAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 100
            });

            _client.ReactionAdded += async (msg, ch, rea) =>
            {
                if (rea.Emote.Name != "❌") return;
                var realMsg = await msg.GetOrDownloadAsync();
                var reacts = await realMsg.GetReactionUsersAsync("❌");
                if (reacts.Count() > 19)
                {
                    var modChannel = _client.GetChannel(271419503257190400) as ISocketMessageChannel;
                    await realMsg.Channel.SendMessageAsync($":hammer: {realMsg.Author.Mention} was voted off the server.");
                    await modChannel.SendMessageAsync($"!!tempban {realMsg.Author.Mention} 600 \"Voted off the server via BanBoard:tm:\"");
                }
            };

            _client.Log += (msg) =>
            {
                Console.WriteLine(msg.ToString());
                return Task.CompletedTask;
            };

            await _client.LoginAsync(TokenType.Bot, "");
            await _client.StartAsync();
            await Task.Delay(-1);
        }
    }
}
