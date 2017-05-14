using System;
using VkNet.Enums.Filters;
using VkNet;
using VkNet.Model.RequestParams;
using System.Net;
using System.Text;
using VkNet.Exception;

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

                var createAlbum = vkapi.Photo.CreateAlbum(new PhotoCreateAlbumParams
                {
                    Title = "Моя деревня"
                });

                //альбом
                var albumId = createAlbum.Id;
                var uploadServer = vkapi.Photo.GetUploadServer(albumId);

                //загрузка
                var wc = new WebClient();
                var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, @"screen.jpg"));

                // Сохранить загруженный файл
                var photos = vkapi.Photo.Save(new PhotoSaveParams
                {
                    SaveFileResponse = responseFile,
                    AlbumId = albumId
                });

                string messageText = "Я поиграл в игру «Моя деревня» и набрал " + args[2] + " баллов";
                //пост
                var post = vkapi.Wall.Post(new WallPostParams
                {
                    Message = messageText,
                    Attachments = photos
                });
            }
            catch(VkApiAuthorizationException)
            {
                Console.WriteLine("invalid password");
            }

            Console.WriteLine("кек");
        }
    }
}
