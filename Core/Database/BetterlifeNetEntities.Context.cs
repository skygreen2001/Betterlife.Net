﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BetterlifeNetEntities : DbContext
    {
        public BetterlifeNetEntities()
            : base("name=BetterlifeNetEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Logsystem> Logsystems { get; set; }
        public DbSet<Loguser> Logusers { get; set; }
        public DbSet<Msg> Msgs { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Rolefunction> Rolefunctions { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Userdetail> Userdetails { get; set; }
        public DbSet<Usernotice> Usernotices { get; set; }
        public DbSet<Userrole> Userroles { get; set; }
    }
}
