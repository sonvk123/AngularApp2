using Azure.Messaging;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;

namespace AngularApp2.Server.Models;

public partial class MessageTable
{
    public int Id { get; set; }
    public long? user_id { get; set; }
    public string? user_name { get; set; }
    public string? message_id { get; set; }
    public string? sender_id { get; set; }
    public string? sender_name { get; set; }
    public string? message_content { get; set; }
    public string? message_type { get; set; }
    public string? media_type { get; set; }
    public string? media_path { get; set; }
    public DateTime timestamp { get; set; }
}
