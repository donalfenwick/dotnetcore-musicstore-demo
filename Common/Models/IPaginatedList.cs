using System;
using System.Collections;
using System.Collections.Generic;

namespace MusicStoreDemo.Common.Models
{
    public interface IPaginatedList<TItem>
    {
        ICollection<TItem> Items { get; set; }

        int TotalItems { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
    }
}