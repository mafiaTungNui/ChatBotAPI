using Microsoft.EntityFrameworkCore;
using ChatBotAPI.Models;

namespace ChatBotAPI.Data
{
    public class ChatBotDbContext : DbContext
    {
        public ChatBotDbContext(DbContextOptions<ChatBotDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Intent> Intents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập các quy tắc và cấu hình cho các bảng nếu cần
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Conversation>().ToTable("Conversations");
            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<Response>().ToTable("Responses");
            modelBuilder.Entity<Intent>().ToTable("Intents");

            base.OnModelCreating(modelBuilder);
        }
    }
}
