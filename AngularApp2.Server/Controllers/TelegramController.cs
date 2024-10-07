using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WTelegram;
using TL;
using AngularApp2.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularApp2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly TelegramService _telegramService;
        private readonly ApiTeleContext _apiTeleContext;


        public TelegramController(TelegramService telegramService, ApiTeleContext apiTeleContext)
        {
            _telegramService = telegramService;
            _apiTeleContext = apiTeleContext;

        }

        // Endpoint to log in with a phone number
        [HttpGet("login")]
        public async Task<IActionResult> Login(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return BadRequest("Phone number is required.");
            }

            try
            {
                var result = await _telegramService.DoLogin(phoneNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Endpoint để lấy danh sách các cuộc hội thoại của người dùng
        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            try
            {
                var result = await _telegramService.GetUserConversations();
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Failed to get conversations: {ex.Message}");
            }
        }

        // Endpoint to find chat with specific user ID and get chat history
        [HttpGet("getChatHistory")]
        public async Task<IActionResult> GetChatHistory(long userId)
        {
            try
            {
                var client = _telegramService.GetClient();
                var dialogs = await client.Messages_GetAllDialogs();

                foreach (var dialog in dialogs.dialogs)
                {
                    var peer = dialogs.UserOrChat(dialog);
                    if (peer is User user && user.ID == userId)
                    {
                        // Call the updated helper method to get detailed chat history
                        var chatHistory = await GetChatHistoryAsync(client, new InputPeerUser(user.id, user.access_hash));
                        return Ok(chatHistory);


                    }
                }

                return NotFound($"Chat with User ID: {userId} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Helper method to get chat history
        // Helper method to get detailed chat history
        private async Task<List<MessageTable>> GetChatHistoryAsync(WTelegram.Client client, InputPeerUser peerInput)
        {
            int offsetId = 0;
            var chatHistory = new List<MessageTable>(); // Danh sách để lưu các tin nhắn

            while (true)
            {
                // Lấy lịch sử tin nhắn cho người dùng đã chỉ định
                var messages = await client.Messages_GetHistory(peerInput, offsetId);
                if (messages.Messages.Length == 0) break; // Nếu không còn tin nhắn, thoát khỏi vòng lặp

                foreach (var msgBase in messages.Messages)
                {
                    var messageModel = new MessageTable();

                    // Lấy thông tin người gửi (User hoặc Chat)
                    var from = messages.UserOrChat(msgBase.From ?? msgBase.Peer);

                    // Nếu là tin nhắn bình thường
                    if (msgBase is Message msg)
                    {
                        // Lấy loại media nếu có
                        var mediaType = msg.media != null ? msg.media.GetType().Name : "None";
                        var mediaPath = msg.media != null ? ExtractMediaPath(msg.media) : null; // Hàm ExtractMediaPath để trích xuất đường dẫn media (nếu có)

                        // Ánh xạ thông tin tin nhắn vào model
                        messageModel.message_id = msg.ID.ToString();
                        messageModel.message_content = msg.message;
                        messageModel.message_type = "Text";
                        messageModel.media_type = mediaType;
                        messageModel.media_path = mediaPath;
                        messageModel.timestamp = msg.date;
                        messageModel.sender_id = from.ID.ToString();
                        messageModel.sender_name = from.ToString(); // Tên người gửi

                    }
                    else if (msgBase is MessageService ms)
                    {
                        // Nếu là tin nhắn hệ thống
                        messageModel.message_content = ms.action.ToString();
                        messageModel.message_type = "Service";
                        messageModel.timestamp = ms.date;
                        messageModel.sender_id = from.ID.ToString();
                        messageModel.sender_name = from.ToString(); // Tên người gửi
                    }

                    _apiTeleContext.MessageTables.Add(messageModel);
                    try
                    {
                        // Code cập nhật cơ sở dữ liệu
                        await _apiTeleContext.SaveChangesAsync();
                        // Thêm model vào danh sách chat history
                        chatHistory.Add(messageModel);
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine(ex.InnerException?.Message);  // Xem thông tin chi tiết
                    }
                }
                // Thiết lập offsetId để lấy loạt tin nhắn tiếp theo
                offsetId = messages.Messages[^1].ID;
            }
            return chatHistory; // Trả về danh sách tin nhắn
        }


        private string? ExtractMediaPath(MessageMedia media)
        {
            // Giả sử media có thể chứa đường dẫn hoặc thông tin file
            if (media is MessageMediaDocument mediaDocument)
            {
                return mediaDocument.document.ToString(); // Trích xuất đường dẫn tài liệu hoặc tệp media
            }
            return null;
        }
    }
}

