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
    
    public partial class UserDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public Nullable<System.DateTime> DateOfBirthday { get; set; }
        public string FamilyStatusId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ImagesId { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual FamilyStatus FamilyStatus { get; set; }
        public virtual Images Images { get; set; }
    }
}
