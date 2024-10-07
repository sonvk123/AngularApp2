using System;
using System.Collections.Generic;

namespace AngularApp2.Server.Models;

public partial class ListMessageTable
{
    public int Id { get; set; }
    public long? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Phone { get; set; }

}
