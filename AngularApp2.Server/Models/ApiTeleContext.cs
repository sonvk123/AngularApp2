using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AngularApp2.Server.Models;

public partial class ApiTeleContext : DbContext
{

    public ApiTeleContext(DbContextOptions<ApiTeleContext> options)
        : base(options)
    {
    }
    public async Task SaveConversationToDatabase(string userName, long? userId, string Phone)
    {
        var existingMessages = ListMessageTables
        .Where(m => m.UserId == userId)
        .ToList();

        // If any entries exist, remove them from the database
        if (existingMessages.Any())
        {
            ListMessageTables.RemoveRange(existingMessages);
        }

        ListMessageTable messageEntry = new ListMessageTable
        {
            UserId = userId,
            UserName = userName,
            Phone = Phone
        };

        ListMessageTables.Add(messageEntry);
        await SaveChangesAsync();

    }
   
    public async Task SaveConversationToDatabase()
    {
        return;
    }

    public virtual DbSet<ListMessageTable> ListMessageTables { get; set; }
    public virtual DbSet<MessageTable> MessageTables { get; set; }
}
