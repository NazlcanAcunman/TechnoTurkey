using System;
using System.Collections.Generic;
using System.Text;

namespace EventTicket.Core.Common;

public interface IEntityTimestamps
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    bool IsDeleted { get; set; }
}