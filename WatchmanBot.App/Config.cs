using Simple.BotUtils.Data;
using Simple.BotUtils.Startup;

namespace WatchmanBot.App
{
    public class Config : ConfigBase
    {
        [ArgumentKey("-token")]
        public string TelegramToken { get; set; }
        [ArgumentKey("-admin")]
        public long TelegramAdmin { get; set; }

    }
}