using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using InvoiceMakerPro.Models;

namespace InvoiceMakerPro.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160603135709_CustomerVatMigration")]
    partial class CustomerVatMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InvoiceMakerPro.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasAnnotation("Relational:Name", "EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .HasAnnotation("Relational:Name", "UserNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetUsers");
                });

            modelBuilder.Entity("InvoiceMakerPro.Models.Article", b =>
                {
                    b.Property<Guid>("ArticleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Notes");

                    b.Property<decimal>("Price");

                    b.Property<decimal>("VAT");

                    b.HasKey("ArticleId");
                });

            modelBuilder.Entity("InvoiceMakerPro.Models.Customer", b =>
                {
                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("CP")
                        .IsRequired();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("CompanyNumber")
                        .IsRequired();

                    b.Property<string>("ContactPerson")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Fax");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Notes");

                    b.Property<string>("Phone1")
                        .IsRequired();

                    b.Property<string>("Phone2");

                    b.Property<decimal>("VAT");

                    b.HasKey("CustomerId");
                });

            modelBuilder.Entity("InvoiceMakerPro.Models.Invoice", b =>
                {
                    b.Property<Guid>("InvoiceId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AdvancePaymentTax");

                    b.Property<decimal>("AdvancePaymentTaxAmount");

                    b.Property<Guid?>("CustomerCustomerId");

                    b.Property<DateTime>("DueDate");

                    b.Property<int?>("InvoiceNumber");

                    b.Property<int>("InvoiceState");

                    b.Property<decimal>("NetTotal");

                    b.Property<string>("Notes");

                    b.Property<bool>("Paid");

                    b.Property<Guid?>("StoreStoreId");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<decimal>("TotalToPay");

                    b.Property<decimal>("TotalWithVAT");

                    b.Property<string>("User");

                    b.Property<decimal>("VATAmount");

                    b.HasKey("InvoiceId");
                });

            modelBuilder.Entity("InvoiceMakerPro.Models.InvoiceDetails", b =>
                {
                    b.Property<Guid>("InvoiceDetailsId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ArticleArticleId");

                    b.Property<Guid?>("InvoiceInvoiceId");

                    b.Property<decimal>("Price");

                    b.Property<int>("Qty");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<decimal>("Total");

                    b.Property<decimal>("TotalPlusVat");

                    b.Property<decimal>("VAT");

                    b.Property<decimal>("VatAmount");

                    b.HasKey("InvoiceDetailsId");
                });

            modelBuilder.Entity("InvoiceMakerPro.Models.Store", b =>
                {
                    b.Property<Guid>("StoreId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("CP")
                        .IsRequired();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("ContactPerson")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Fax");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Notes");

                    b.Property<string>("Phone1")
                        .IsRequired();

                    b.Property<string>("Phone2");

                    b.HasKey("StoreId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasAnnotation("Relational:Name", "RoleNameIndex");

                    b.HasAnnotation("Relational:TableName", "AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAnnotation("Relational:TableName", "AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasAnnotation("Relational:TableName", "AspNetUserRoles");
                });

            modelBuilder.Entity("InvoiceMakerPro.Models.Invoice", b =>
                {
                    b.HasOne("InvoiceMakerPro.Models.Customer")
                        .WithMany()
                        .HasForeignKey("CustomerCustomerId");

                    b.HasOne("InvoiceMakerPro.Models.Store")
                        .WithMany()
                        .HasForeignKey("StoreStoreId");
                });

            modelBuilder.Entity("InvoiceMakerPro.Models.InvoiceDetails", b =>
                {
                    b.HasOne("InvoiceMakerPro.Models.Article")
                        .WithMany()
                        .HasForeignKey("ArticleArticleId");

                    b.HasOne("InvoiceMakerPro.Models.Invoice")
                        .WithMany()
                        .HasForeignKey("InvoiceInvoiceId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("InvoiceMakerPro.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("InvoiceMakerPro.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNet.Identity.EntityFramework.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("InvoiceMakerPro.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}
