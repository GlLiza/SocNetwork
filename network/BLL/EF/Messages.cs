//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace network.BLL.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Messages
    {
        public int Id { get; set; }
        public int Conversation_id { get; set; }
        public int Sender_id { get; set; }
        public string Message { get; set; }
        public Nullable<System.DateTime> Created_at { get; set; }
        public Nullable<bool> Visibility { get; set; }
        public Nullable<bool> IsNotReading { get; set; }
    
        public virtual Conversation Conversation { get; set; }
        public virtual UserDetails UserDetails { get; set; }
    }
}
