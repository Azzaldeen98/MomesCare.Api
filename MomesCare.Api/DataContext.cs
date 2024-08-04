using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Entities.Models;
using Firebase.Auth;
using System.Reflection.Emit;

namespace MomesCare.Api
{
    public class DataContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Profile> Profile { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<FavoritePosts> FavoritePosts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<FavoriteComments> FavoriteComments { get; set; }
        public DbSet<Baby> Babies { get; set; }
        public DbSet<CloudMessagingToken> CloudMessagingTokens { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseItem> CourseItems { get; set; }
        public DbSet<BroadcastLive> BroadcastLives { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<JoinBroadcastLive> JoinBroadcastLives { get; set; }
        public DbSet<BabyCareScheduled> BabyCareScheduleds { get; set; }
        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<CareType> CareTypes { get; set; }
        public DbSet<DailyCareTimes> DailyCareTimes { get; set; }
        public DbSet<BabyHealthCareNotificationsSent> DailyBabyCareNotificationsSents { get; set; }
   





        protected override void OnModelCreating(ModelBuilder builder)
        {

            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Cascade;

            builder.Entity<ApplicationUser>()
             .HasOne(a => a.profile)
             .WithOne(b => b.user)
             .HasForeignKey<Profile>(b => b.UserId);

            builder.Entity<ApplicationUser>()
              .HasOne(a => a.doctor)
              .WithOne(b => b.user)
              .HasForeignKey<Doctor>(b => b.UserId);
         
            builder.Entity<ApplicationUser>()
              .HasOne(a => a.cloudMessagingToken)
              .WithOne(b => b.user)
              .HasForeignKey<CloudMessagingToken>(b => b.UserId);


            //builder.Entity<Baby>()
            //.HasIndex(u => u.Name)
            //.IsUnique();


            base.OnModelCreating(builder);

         
        }





    }
}
