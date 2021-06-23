using System;
using System.Threading.Tasks;
using Serilog;
using Simple.BotUtils.DI;
using Simple.BotUtils.Jobs;
using Telegram.Bot;
using RafaelEstevam.Simple.Spider.Helper;

namespace WatchmanBot.App
{
    internal class SiteReturnTest : JobBase
    {
        private readonly ILogger logger;
        private readonly TelegramBotClient bot;
        private readonly Config cfg;
        public SiteReturnTest()
        {
            logger = Injector.Get<ILogger>();
            bot = Injector.Get<TelegramBotClient>();
            cfg = Injector.Get<Config>();

            RunOnStartUp = true;
            CanBeScheduled = true;
            StartEvery = TimeSpan.FromMinutes(1);

            logger.Information("Iniciando o teste de request.");
        }

        public override async Task ExecuteAsync(ExecutionTrigger trigger, object parameter)
        {
            logger.Information("SiteReturnTest: {trigger}", trigger);
            Uri uri = new Uri("https://portalsharp.com.br");
            try
            {
                var chat = new Telegram.Bot.Types.ChatId(cfg.TelegramAdmin);
                Telegram.Bot.Types.Message message;
                if (trigger == ExecutionTrigger.Startup)
                {
                    var response = RequestInfo.SendRequest(uri);
                    var status = response.StatusCode;
                    var phrase = response.ReasonPhrase;
                    var duration = response.RequestDuration;

                    if(status.ToString() != "OK")
                    {
                        message = await bot.SendTextMessageAsync(chat, "Deu merda");
                    }
                    else
                    {
                        message = await bot.SendTextMessageAsync(chat, $"Status: {status}\n" +
                                               $"Phrase: {phrase}\n" +
                                               $"Duration: {duration}");
                    }
                }
                else if (trigger == ExecutionTrigger.Scheduled)
                {
                    var response = RequestInfo.SendRequest(uri);
                    var status = response.StatusCode;
                    var phrase = response.ReasonPhrase;
                    var duration = response.RequestDuration;

                    if (status.ToString() != "OK")
                    {
                        message = await bot.SendTextMessageAsync(chat, "Deu merda");
                    }
                    else
                    {
                        message = await bot.SendTextMessageAsync(chat, $"Status: {status}\n" +
                           $"Phrase: {phrase}\n" +
                           $"Duration: {duration}");
                    }
                }
                else return;

                logger.Information("sent: {@message}", message);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "SiteReturnTest: ExecuteAsync error");
            }
        }
    }
}