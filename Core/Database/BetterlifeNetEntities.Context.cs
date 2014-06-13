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
    
        /// <summary>
        /// 系统管理员
        /// </summary>
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Functions> Functions { get; set; }
        public DbSet<Msg> Msg { get; set; }
        public DbSet<Notice> Notice { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Rolefunctions> Rolefunctions { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Userdetail> Userdetail { get; set; }
        public DbSet<Usernotice> Usernotice { get; set; }
        public DbSet<Userrole> Userrole { get; set; }

        /// <summary>
        /// 系统日志
        /// </summary>
        public DbSet<Logsystem> Logsystem { get; set; }
        public DbSet<Loguser> Loguser { get; set; }
    }
}
