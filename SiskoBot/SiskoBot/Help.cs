using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xNet;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SiskoBot
{
    public class Help
    {
        Dict dict = new Dict();
        /// <summary>
        /// Ведём логирование
        /// </summary>
        /// <param name="updates"></param>
        public void LoadMessages(Telegram.Bot.Types.Update[] updates)
        {
            using (var fstream = new System.IO.StreamWriter(@"C:\Users\Рамазан\Documents\Visual Studio 2015\Projects\SiskoBot\Logs\log.txt", true, System.Text.Encoding.Default))
            {
                var updates_ = updates;
                foreach(var update in updates_)
                {

                        fstream.Write("{" + update.Message.Date.ToString() + "  ");
                        if(update.Message.Chat.FirstName != null)
                        fstream.Write(update.Message.Chat.FirstName.ToString() + " " /*+ update.Message.Chat.LastName.ToString() + "  "*/);

                    fstream.Write(update.Message.Chat.Id.ToString() + " } ");
                    if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                        fstream.WriteLine("Сообщение: " + update.Message.Text.ToString());
                    else
                        fstream.WriteLine();
                }                
                fstream.Close();
            }
        }

        /// <summary>
        /// Логируем отправленные изображения
        /// </summary>
        /// <param name="link"></param>
        public void LogImages(string link, string user)
        {
            using (var fstream = new System.IO.StreamWriter(@"C:\Users\Рамазан\Documents\Visual Studio 2015\Projects\SiskoBot\Logs\images.txt", true, System.Text.Encoding.Default))
            {
                fstream.WriteLine("{" + System.DateTime.Now.ToString() + "} [" + user + "] " + link);
                fstream.Close();
            }
        }

        /// <summary>
        /// Возвращает параметры RequestParams для запроса погоды
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public RequestParams Request(string city)
        {
            var par = new RequestParams();
            par["q"] = city;
            par["type"] = "like";
            par["units"] = "metric";
            par["lang"] = "ru";
            par["APPID"] = "11056b9e636720452bb0d13dfcab33a3";
            return par;
        }

        /// <summary>
        /// Возвращает название городя для запроса погоды
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string City(string message)
        {
            List<string> MegaCity = new List<string>();
            List<string> City = new List<string>();
            City.Add("Peterburg");
            City.Add("Vyborg");
            City.Add("Moscow");
            City.Add("Sochi");
            City.Add("London");

            MegaCity.Add("петербург");
            MegaCity.Add("выборг");
            MegaCity.Add("москв");
            MegaCity.Add("сочи");
            MegaCity.Add("лондон");
            string gorod = "";
            for(int i = 0; i < MegaCity.Count(); i++ )
            {
                if(message.Trim().ToLower().Contains(MegaCity[i]))
                {
                    gorod = City[i];
                }
            }

            return gorod;
        }

        /// <summary>
        /// Узнаем погоду в определенном месте
        /// </summary>
        /// <param name="message"></param>
        /// <param name="bot"></param>
       async public void WeatherApi(Telegram.Bot.Types.Message message, Telegram.Bot.TelegramBotClient bot)
        {
            string city = City(message.Text);

            if (city == "")
                await bot.SendTextMessageAsync(message.Chat.Id, "Воу воу, полегче. Я тебе не гугл!", replyToMessageId: message.MessageId);
            else
            {
                var parameters = Request(city);
                HttpRequest getInfo = new HttpRequest();

                var res = getInfo.Get("http://api.openweathermap.org/data/2.5/find", parameters).ToString();

                SiskoBot.WeatherApi.Message api = JsonConvert.DeserializeObject<SiskoBot.WeatherApi.Message>(res);

                if (api.list.Count() > 1)
                {
                    bool send = false;
                    foreach (var item in api.list)
                    {
                        if (item.sys.country.Trim().ToLower() == "ru")
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, "Температура: " + item.main.temp + ", " + item.weather[0].description);
                            send = true;
                        }
                        else if (item.sys.country.Trim().ToLower() == "gb")
                        {
                            await bot.SendTextMessageAsync(message.Chat.Id, "Температура: " + item.main.temp + ", " + item.weather[0].description);
                            send = true;
                        }
                    }
                    if (!send)
                        await bot.SendTextMessageAsync(message.Chat.Id, "Какие то говнари не хотят давать мне инфу по погоде. Сорян, братан");
                }
                else
                    await bot.SendTextMessageAsync(message.Chat.Id, "Температура: " + api.list[0].main.temp + ", " + api.list[0].weather[0].description);
            }
        }
        
        /// <summary>
        /// Делаем поисковый запрос в Google
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="message"></param>
        /// <param name="query"></param>
        async public void ImageGoogle(Telegram.Bot.TelegramBotClient bot, Telegram.Bot.Types.Message message, string query)
        {

            string apiKey = "AIzaSyDlSceSgZdV1rMzq9LR5gcOMJpDfqjID7I";
            string cx = "004298067883753127426:rxfuckscscy";
            
            var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer { ApiKey = apiKey });
            var listrequest = svc.Cse.List(query.Substring(1));
            listrequest.Cx = cx;
            Random ran = new Random();
            int co = ran.Next(0, 9);
            var search = listrequest.Execute();
            var links = new Telegram.Bot.Types.MessageEntity();
            links.Url = search.Items[0].Link;

            await bot.SendTextMessageAsync(message.Chat.Id, search.Items[0].Link, Telegram.Bot.Types.Enums.ParseMode.Html);
            LogImages(search.Items[0].Link, message.Chat.FirstName);
            query = "";
        }

        /// <summary>
        /// Поиск картинок в Google и оправка в чат
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="message"></param>
        /// <param name="query"></param>
        async public void SearchImage(Telegram.Bot.TelegramBotClient bot, Telegram.Bot.Types.Message message, string query)
        {

            string apiKey = "AIzaSyDlSceSgZdV1rMzq9LR5gcOMJpDfqjID7I";
            string cx = "004298067883753127426:rxfuckscscy";

            var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new Google.Apis.Services.BaseClientService.Initializer { ApiKey = apiKey });
            var listrequest = svc.Cse.List(query.Substring(1));
            listrequest.Cx = cx;
            Random ran = new Random();
            int co = ran.Next(0, 9);
            listrequest.SearchType = Google.Apis.Customsearch.v1.CseResource.ListRequest.SearchTypeEnum.Image;
            var search = listrequest.Execute();
            var links = new Telegram.Bot.Types.MessageEntity();
            links.Url = search.Items[co].Link;

            await bot.SendPhotoAsync(message.Chat.Id, new Telegram.Bot.Types.FileToSend(links.Url));
            LogImages(search.Items[co].Link, message.Chat.FirstName);
            query = "";
        }

        /// <summary>
        /// Узнаем погоду по местоположению пользователя
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="message"></param>
        async public void SendWeather(Telegram.Bot.TelegramBotClient bot, Telegram.Bot.Types.Message message)
        {
            HttpRequest getInfo = new HttpRequest();
            var par = new RequestParams();
            par["lat"] = message.Location.Latitude.ToString();
            par["lon"] = message.Location.Longitude.ToString();
            par["units"] = "metric";
            par["lang"] = "ru";
            par["APPID"] = "11056b9e636720452bb0d13dfcab33a3";
            var res = getInfo.Get("http://api.openweathermap.org/data/2.5/weather", par).ToString();

            SiskoBot.WeatherInLocation.Message api = JsonConvert.DeserializeObject<SiskoBot.WeatherInLocation.Message>(res);
            await bot.SendTextMessageAsync(message.Chat.Id, "Температура: " + api.main.temp + ", " + api.weather[0].description);

        }

        /// <summary>
        /// Запрашиваем местополежение пользователя
        /// </summary>
        /// <param name="bot"></param>
        /// <param name="message"></param>
        async public void KeyTemp(Telegram.Bot.TelegramBotClient bot, Telegram.Bot.Types.Message message)
        {
            Telegram.Bot.Types.KeyboardButton[] keyboard = new Telegram.Bot.Types.KeyboardButton[] { "Отправить локацию" };
            keyboard[0].RequestLocation = true;
            var mark = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(keyboard, true, false);
            await bot.SendTextMessageAsync(message.Chat.Id, "Пришли мне свое местоположение по кнопке ниже", Telegram.Bot.Types.Enums.ParseMode.Default, false, false, message.MessageId, mark);
        }
        
        public bool Hi(Telegram.Bot.Types.Message message)
        {
            bool key = false;
            var greet = dict.Greetings();
            foreach(var item in greet)
            {
                if (message.Text.Trim().ToLower().Contains(item.Value))
                    key = true;
            }
            return key;
        }

        public bool Develops(Telegram.Bot.Types.Message message)
        {
            bool key = false;
            var develop = dict.Develop();
            foreach (var item in develop)
            {
                if (message.Text.Trim().ToLower().Contains(item.Value))
                    key = true;
            }
            return key;
        }
        public bool Agrees(Telegram.Bot.Types.Message message)
        {
            bool key = false;
            var agree = dict.Agree();
            foreach (var item in agree)
            {
                if (message.Text.Trim().ToLower().Contains(item.Value))
                    key = true;
            }
            return key;
        }
        public bool Pnhs(Telegram.Bot.Types.Message message)
        {
            bool key = false;
            var pnh = dict.Pnh();
            foreach (var item in pnh)
            {
                if (message.Text.Trim().ToLower().Contains(item.Value))
                    key = true;
            }
            return key;
        }

        public string GetResponseNah()
        {
            List<string> response = new List<string>();
            response.Add("Э бля. По ебальнику съездить?!");
            response.Add("Слышь, чмо. Следи за базаром!");
            response.Add("Ща допиздишься нахер!");
            response.Add("У кого-то дохуя жизней?!");
            response.Add("Тебе за жизнь обосновать?!");
            response.Add("Да иди в пизду блять!");

            Random ranResponse = new Random();
            int count = ranResponse.Next(0, 5);

            string otvet = response[count];
            return otvet;
        }
    }
}
