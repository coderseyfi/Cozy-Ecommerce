using Cozy.Domain.Models.Entites;
using Cozy.Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cozy.Domain.Models.DataContexts
{
    public class CozyDbContext: IdentityDbContext<CozyUser, CozyRole, int, CozyUserClaim,
        CozyUserRole,
        CozyUserLogin, CozyRoleClaim, CozyUserToken>
    {
        public CozyDbContext(DbContextOptions options)
           : base(options)
        {

        }

        public DbSet<ContactPost> ContactPosts { get; set; }

        public DbSet<ContactInfo> ContactInfos { get; set; }

        public DbSet<Faq> Faqs { get; set; }

        public DbSet<Subscribe> Subscribes { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<BlogPostComment> BlogPostComments { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<BlogPostTagItem> BlogPostTagCloud { get; set; }
        public DbSet<ProductColor> Colors { get; set; }






        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CozyDbContext).Assembly);
        }
    }
}
