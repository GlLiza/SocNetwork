﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NetworkContext : DbContext
    {
        public NetworkContext()
            : base("name=NetworkContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AlbAndPhot> AlbAndPhot { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Conversation> Conversation { get; set; }
        public virtual DbSet<FamilyStatus> FamilyStatus { get; set; }
        public virtual DbSet<Friendship> Friendship { get; set; }
        public virtual DbSet<FriendStatuses> FriendStatuses { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Participants> Participants { get; set; }
        public virtual DbSet<Photoalbum> Photoalbum { get; set; }
        public virtual DbSet<Requests> Requests { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }
        public virtual DbSet<WorkPlace> WorkPlace { get; set; }
    }
}