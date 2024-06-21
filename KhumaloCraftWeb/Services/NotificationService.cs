using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using KhumaloCraftWeb.Data;

namespace KhumaloCraftWeb.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;
        private static FirebaseApp _firebaseApp;

        public NotificationService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;

            if (_firebaseApp == null)
            {
                var googleCredential = GoogleCredential.FromFile(configuration["Firebase:ServiceAccountKeyPath"]);
                _firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = googleCredential
                });
            }
        }

        public async Task SendNotificationAsync(string title, string body, string imageUrl)
        {
            var tokens = _context.Users.Select(u => u.FcmToken).ToList();

            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body,
                    ImageUrl = imageUrl
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);

            // Log the number of successful and failed sends
            Console.WriteLine($"{response.SuccessCount} messages were sent successfully");
            Console.WriteLine($"{response.FailureCount} messages failed to send");
        }

        public async Task SendNotificationToAllClientsAsync(string title, string body)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Topic = "all" // Subscribe all clients to this topic
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine("Successfully sent message: " + response);
        }

        public async Task SendNotificationToSpecificClientAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Token = token
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine("Successfully sent message: " + response);
        }

    }
}
