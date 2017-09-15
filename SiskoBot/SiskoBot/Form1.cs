using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;


namespace SiskoBot
{
    public partial class Form1 : Form
    {
        BackgroundWorker bw;
        public Form1()
        {
            InitializeComponent();

            this.bw = new BackgroundWorker();
            this.bw.DoWork += this.bw_DoWork;
            
        }

 

        async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as String;
            Help help = new Help();
            Dict dict = new Dict();
            try
            {
            
                Uri collectionuri = new Uri("https://tfs.bss.nvision-group.com");
                System.Net.NetworkCredential credent = new System.Net.NetworkCredential(@"bss\rygrishi", "Parol");
                Microsoft.TeamFoundation.Client.TfsTeamProjectCollection tfs = new Microsoft.TeamFoundation.Client.TfsTeamProjectCollection(collectionuri, credent);
                tfs.EnsureAuthenticated();

                var workitemstore = tfs.GetService<Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItemStore>();
                bool isproj = false;
                string projName = null;
                foreach(Microsoft.TeamFoundation.WorkItemTracking.Client.Project proj in workitemstore.Projects)
                {
                    if(proj.Name.ToLower() == "foris_mobile")
                    {
                        isproj = true;
                        projName = proj.Name;
                    }
                }

                var itemsQuery = workitemstore.Query( @"select System.Title, System.State, System.Id, System.CreatedBy  from WorkItems 
                                                        where [System.TeamProject] = '" + projName +"'"
                                                        + "and [System.WorkItemType] = 'Bug'"
                                                        + "and [System.State] <> 'Closed'"
                                                        + "and [System.Title] Contains '[AT]'"
                                                        + "and [Found On Stand] = 'MSK-FORIS-60'"
                                                        + "and [System.AssignedTo] = 'g Test Automation Team'");
            
                var Bot = new Telegram.Bot.TelegramBotClient(key);
                await Bot.SetWebhookAsync("");
                int offset = 0;
                var GlobalUpdates = await Bot.GetUpdatesAsync(offset);
                
                foreach(var update in GlobalUpdates)
                {
                    offset = update.Id + 1;
                }
                string bb = "";
                string query = "";
                while (true)
                {
                    var updates = await Bot.GetUpdatesAsync(offset);
                    help.LoadMessages(updates);
                    foreach (var update in updates)
                    {
                        var message = update.Message;
                        if(message.Type == Telegram.Bot.Types.Enums.MessageType.LocationMessage)
                        {
                            help.SendWeather(Bot, message);
                        }
                        else if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage && message.Text.Trim().Contains("Бот"))
                        {

                            if (help.Hi(message) == true)
                            {
                                await Bot.SendChatActionAsync(message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
                                var url = @"C:\Users\Рамазан\Documents\Visual Studio 2015\Projects\SiskoBot\Pics\sticker_954919964618261059.webp";
                                using (var fstream = System.IO.File.Open(url, System.IO.FileMode.Open))
                                {
                                    Telegram.Bot.Types.FileToSend fts = new Telegram.Bot.Types.FileToSend();
                                    fts.Content = fstream;
                                    fts.Filename = url.Split('\\').Last();
                                    await Bot.SendStickerAsync(message.Chat.Id, fts);
                                    fstream.Close();
                                }
                            }

                            else if(help.Develops(message) == true)
                            {
                                await Bot.SendChatActionAsync(message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Своим существованием я обязан Роману Юрьевичу", replyToMessageId: message.MessageId);
                            }
                            else if(help.Pnhs(message) == true)
                            {
                                string response = help.GetResponseNah();
                                await Bot.SendTextMessageAsync(message.Chat.Id, response, replyToMessageId: message.MessageId);
                            }
                            else if(help.Agrees(message) == true)
                            {
                                await Bot.SendChatActionAsync(message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
                                if(message.Text.Trim().ToLower().Contains("б-52") || message.Text.Trim().ToLower().Contains("б52"))
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "А вот хуй!", replyToMessageId: message.MessageId);
                                else
                                    await Bot.SendTextMessageAsync(message.Chat.Id, "Согласовано", replyToMessageId: message.MessageId);
                            }
                            else if(message.Text.Trim().ToLower().Contains("спасибо"))
                            {
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Да говно вoпрос)", replyToMessageId: message.MessageId);
                            }
                            else if (message.Text.Trim().ToLower().Contains( "температура") && message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Private)
                            {
                                help.KeyTemp(Bot, message);
                            }
                            else if(message.Text.Trim().ToLower().Contains("температура") && message.Chat.Type != Telegram.Bot.Types.Enums.ChatType.Private)
                            {
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Извини, но я могу показать погоду по текущему местоположению только в приватном чате", replyToMessageId: message.MessageId);
                            }
                            else if (message.Text.Trim().ToLower().Contains("погода") && message.Text.Substring(0, 1) != "*")
                            {
                                await Bot.SendChatActionAsync(message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
                                help.WeatherApi(message, Bot);
                            }
                           
                            else if(message.Text != bb)
                            {
                                await Bot.SendChatActionAsync(message.Chat.Id, Telegram.Bot.Types.Enums.ChatAction.Typing);
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Ишь чего хочешь! Эта функция в разработке!", replyToMessageId: message.MessageId);
                            }                           
                        }
                        else if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage && message.Text.Trim().ToLower().Contains ("/search"))
                        {
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Чё надо?");
                            bool ya = true;
                            query = "";
                            while (ya)
                            {
                                offset = update.Id + 1;
                                var goo = await Bot.GetUpdatesAsync(offset);
                                foreach (var i in goo)
                                {

                                    var soopshenie = i.Message;
                                    if (soopshenie.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage && soopshenie.Text.Substring(0, 1) == "*")
                                    {
                                        query = soopshenie.Text;
                                        ya = false;
                                        bb = soopshenie.Text;
                                    }
                                    else
                                    {
                                        await Bot.SendTextMessageAsync(soopshenie.Chat.Id, "Нет доступа!");
                                        ya = false;
                                        bb = soopshenie.Text;
                                    }
                                }
                            }
                            ya = false;
                            if (query != "")
                            {
                                help.ImageGoogle(Bot, message, query);
                            }
                        }
                        else if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage && message.Text.Trim().ToLower().Contains("/imagesearch"))
                        {
                            await Bot.SendTextMessageAsync(message.Chat.Id, "Какую картинку тебе найти?");
                            bool ya = true;
                            query = "";
                            while (ya)
                            {
                                offset = update.Id + 1;
                                var goo = await Bot.GetUpdatesAsync(offset);
                                foreach (var i in goo)
                                {

                                    var soopshenie = i.Message;
                                    if (soopshenie.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage && soopshenie.Text.Substring(0, 1) == "*")
                                    {
                                        query = soopshenie.Text;
                                        ya = false;
                                        bb = soopshenie.Text;
                                    }
                                    else
                                    {
                                        await Bot.SendTextMessageAsync(soopshenie.Chat.Id, "Нет доступа!");
                                        ya = false;
                                        bb = soopshenie.Text;
                                    }
                                }
                            }
                            ya = false;
                            if (query != "")
                            {
                                help.SearchImage(Bot, message, query);
                            }
                        }
                        offset = update.Id + 1;
                    }
                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка бота", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void BtnRunClick(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            if (text != null && this.bw.IsBusy != true)
            {
                this.bw.RunWorkerAsync(text);
            }
        }
        private void BtnStopClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
