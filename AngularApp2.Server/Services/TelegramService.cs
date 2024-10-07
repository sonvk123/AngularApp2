using AngularApp2.Server.Models;
using TL;

public class TelegramService
{
    private readonly ApiTeleContext _context;
    private static WTelegram.Client _client; // Singleton Client instance

    public TelegramService(ApiTeleContext context)
    {
        _context = context;
    }

    private async Task<WTelegram.Client> GetTelegramClient()
    {
        if (_client == null)
        {
            _client = new WTelegram.Client(26305124, "86d244b9c442478e71cf77a8188df692");
        }
        return _client;
    }

    public async Task<string> SaveUserConversationsToDatabase(string userName, long? userId, string Phone)
    {
        try
        {
            await _context.SaveConversationToDatabase(userName, userId, Phone);
            return "Conversation saved successfully";
        }
        catch (Exception ex)
        {
            return $"Failed to save conversation: {ex.Message}";
        }
    }

    public async Task<string> DoLogin(string loginInfo)
    {
        _client = await GetTelegramClient(); // Đảm bảo _client không bị null
        while (_client.User == null)
        {
            var config = await _client.Login(loginInfo);

            switch (config)
            {
                case "verification_code":
                    Console.Write("Code: ");
                    loginInfo = Console.ReadLine();
                    break;
                default:
                    loginInfo = null;
                    break;
            }
        }
        return $"We are logged-in as {_client.User} (id {_client.User.id})";
    }

    // Các phương thức để lấy cuộc hội thoại hoặc lịch sử tin nhắn vẫn giữ nguyên
    public async Task<string> GetUserConversations()
    {
        _client = await GetTelegramClient(); // Đảm bảo _client không bị null
        if (_client.User == null)
        {
            throw new Exception("Please log in first.");
        }

        var dialogs = await _client.Messages_GetAllDialogs();
        string result = "User Conversations:\n";

        foreach (var dialog in dialogs.dialogs)
        {
            var peer = dialogs.UserOrChat(dialog);

            switch (peer)
            {
                case User user when user.IsActive:
                    result += $"User: {user.last_name + user.first_name},Phone: {user.phone}, ID: {user.id}\n";
                    await SaveUserConversationsToDatabase(user.last_name + user.first_name, user.id, user.phone);
                    break;

                case ChatBase chat when chat.IsActive:
                    result += $"Chat: {chat.Title}, ID: {chat.ID}\n";
                    break;

                default:
                    result += $"Unknown peer type for dialog ID: {dialog.Peer.ID}\n";
                    break;
            }
        }

        return result;
    }
    // Hàm để lấy đối tượng WTelegram.Client (nếu cần sử dụng thêm)
    // Method to get the singleton client
    public WTelegram.Client GetClient()
    {
        return GetTelegramClient().Result;
    }
}
