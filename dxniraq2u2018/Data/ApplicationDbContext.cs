using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using dxniraq2u2018.Models;

namespace dxniraq2u2018.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<dxniraq2u2018.Models.MemberType> MemberTypes { get; set; }
        public DbSet<dxniraq2u2018.Models.Address> Addresses { get; set; }
        public DbSet<dxniraq2u2018.Models.Tracking> Trackings { get; set; }
        public DbSet<dxniraq2u2018.Models.Statement> Statements { get; set; }
        public DbSet<dxniraq2u2018.Models.ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<dxniraq2u2018.Models.Product> Products { get; set; }
        public DbSet<dxniraq2u2018.Models.Page> Pages { get; set; }
        public DbSet<dxniraq2u2018.Models.Invoice> Invoices { get; set; }
        public DbSet<dxniraq2u2018.Models.City> Cities { get; set; }
        public DbSet<dxniraq2u2018.Models.BlogPost> BlogPosts { get; set; }
        public DbSet<dxniraq2u2018.Models.BlogCategory> BlogCategories { get; set; }
        public DbSet<dxniraq2u2018.Models.CategoryProduct> CategoryProducts { get; set; }
        public DbSet<dxniraq2u2018.Models.InvoiceItem> InvoiceItems { get; set; }
        public DbSet<dxniraq2u2018.Models.PaymentMethod> PaymentMethods { get; set; }
        public IEnumerable<object> ApplicationUsers { get; internal set; }
        public DbSet<dxniraq2u2018.Models.ApplicationUser> ApplicationUser { get; set; }
        public DbSet<dxniraq2u2018.Models.Branch> Branchs { get; set; }
        public DbSet<dxniraq2u2018.Models.Lecture> Lectures { get; set; }
        public DbSet<dxniraq2u2018.Models.LectureRegistration> LectureRegistrations { get; set; }
        public DbSet<dxniraq2u2018.Models.StoreLog> StoreLogs { get; set; }
        public DbSet<dxniraq2u2018.Models.Attendance> Attendances { get; set; }
        public DbSet<dxniraq2u2018.Models.Comment> Comments { get; set; }
        public DbSet<dxniraq2u2018.Models.BlogAlbum> BlogAlbum { get; set; }
        public DbSet<dxniraq2u2018.Models.BlogSection> BlogSection { get; set; }
        public DbSet<dxniraq2u2018.Models.BranchAdvertismentScreenk> BranchAdvertismentScreenk { get; set; }
        public DbSet<dxniraq2u2018.Models.Library> Library { get; set; }
        public DbSet<dxniraq2u2018.Models.RegisterIntention> RegisterIntention { get; set; }
        public DbSet<dxniraq2u2018.Models.TicketReply> TicketReply { get; set; }
        public DbSet<dxniraq2u2018.Models.Ticket> Ticket { get; set; }
        public DbSet<dxniraq2u2018.Models.Faq> Faq { get; set; }
        public DbSet<dxniraq2u2018.Models.Gallery> Gallery { get; set; }
        public DbSet<dxniraq2u2018.Models.GalleryImage> GalleryImage { get; set; }
        public DbSet<dxniraq2u2018.Models.FaqCategory> FaqCategory { get; set; }

        //Community (Blog, communiy, Group)
        public DbSet<dxniraq2u2018.Models.Community> Communities { get; set; }
        public DbSet<dxniraq2u2018.Models.Post> Posts { get; set; }
        public DbSet<dxniraq2u2018.Models.PostComment> PostComments { get; set; }
        public DbSet<dxniraq2u2018.Models.CommunityFollower> CommunityFollowers { get; set; }
        public DbSet<dxniraq2u2018.Models.CommentMetric> CommentMetrics { get; set; }
        public DbSet<dxniraq2u2018.Models.PostMetric> PostMetrics { get; set; }
        public DbSet<dxniraq2u2018.Models.PostRevision> PostRevisions { get; set; }
    }
}
