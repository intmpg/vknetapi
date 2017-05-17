using System;
using VkNet.Enums.Filters;
using VkNet;
using VkNet.Model.RequestParams;
using System.Net;
using System.Text;
using VkNet.Exception;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            VkApi vkapi = new VkApi();
            try
            {
                vkapi.Authorize(new ApiAuthParams
                {
                    ApplicationId = 6030680,
                    Login = args[0],
                    Password = args[1],
                    Settings = Settings.All
                });

                StreamWriter outputFile = new StreamWriter(@"vk_post/post/check.kek");
                outputFile.WriteLine("TRUE");
                outputFile.Close();

                string messageText = "Я поиграл в игру «Моя деревня» и набрал " + args[2] + " баллов";
                var uploadServer = vkapi.Photo.GetWallUploadServer(null);

                var wc = new WebClient();
                var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, @"vk_post/post/screen.jpg"));

                var photos = vkapi.Photo.SaveWallPhoto(responseFile);
                var post = vkapi.Wall.Post(new WallPostParams
                {
                    OwnerId = vkapi.UserId.Value,
                    FriendsOnly = false,
                    FromGroup = false,
                    Message = messageText,
                    Attachments = photos                
                });
            }
            catch(VkApiAuthorizationException)
            {
                Console.WriteLine("invalid password");
                StreamWriter outputFile = new StreamWriter(@"vk_post/post/check.kek");
                outputFile.WriteLine("FALSE");
                outputFile.Close();
            }

            Console.WriteLine("кек");
        }
    }
}
