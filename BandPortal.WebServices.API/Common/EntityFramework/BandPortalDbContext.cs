using BandPortal.WebServices.API.Common.EntityFramework.EntityModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BandPortal.WebServices.API.Common.EntityFramework
{
    public class BandPortalDbContext : DbContext
    {
        public BandPortalDbContext(DbContextOptions<BandPortalDbContext> options)
            : base(options)
        {
        }





        public DbSet<AddressEntityModel> Addresses { get; set; }
        public DbSet<BandEntityModel> Bands { get; set; }
        public DbSet<BandMembershipEntityModel> BandMemberships { get; set; }
        public DbSet<ContactEntityModel> Contacts { get; set; }
        public DbSet<DividendEntityModel> Dividends { get; set; }
        public DbSet<GigAvailabilityEntityModel> GigAvailabilities { get; set; }
        public DbSet<GigEntityModel> Gigs { get; set; }
        public DbSet<InvoiceEntityModel> Invoices { get; set; }
        public DbSet<NoteEntityModel> Notes { get; set; }
        public DbSet<PasswordResetTokenEntitymodel> PasswordResetTokens { get; set; }
        public DbSet<QuoteEntityModel> Quotes { get; set; }
        public DbSet<RefreshTokenEntityModel> RefreshTokens { get; set; }
        public DbSet<SetListEntityModel> SetLists { get; set; }
        public DbSet<SetListItemEntityModel> SetListItems { get; set; }
        public DbSet<TrackEntityModel> Tracks { get; set; }
        public DbSet<UserEntityModel> Users { get; set; }
        public DbSet<VenueEntityModel> Venues { get; set; }
        public DbSet<VenueEquipmentEntityModel> VenueEquipment { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);





            // addresses
            modelBuilder.Entity<AddressEntityModel>(entity =>
            {
                entity.ToTable("addresses");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");

                entity.Property(e => e.Name)                            .HasColumnName("name")                                      .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.AddressLine1)                    .HasColumnName("address_line_1")                            .HasColumnType("text");
                entity.Property(e => e.AddressLine2)                    .HasColumnName("address_line_2")                            .HasColumnType("text");
                entity.Property(e => e.City)                            .HasColumnName("city")                                      .HasColumnType("text");
                entity.Property(e => e.County)                          .HasColumnName("county")                                    .HasColumnType("text");
                entity.Property(e => e.Country)                         .HasColumnName("country")                                   .HasColumnType("text");
                entity.Property(e => e.Postcode)                        .HasColumnName("postcode")                                  .HasColumnType("text");
                entity.Property(e => e.Latitude)                        .HasColumnName("latitude")                                  .HasColumnType("float");
                entity.Property(e => e.Longitude)                       .HasColumnName("longitude")                                 .HasColumnType("float");
                entity.Property(e => e.PrimaryContactId)                .HasColumnName("primary_contact_id")                        .HasColumnType("char(36)");



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.PrimaryContact)                    .WithMany()                                                 .HasForeignKey(e => e.PrimaryContactId) .OnDelete(DeleteBehavior.SetNull);
            });





            // bands
            modelBuilder.Entity<BandEntityModel>(entity =>
            {
                entity.ToTable("bands");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.Name)                            .HasColumnName("name")                                      .HasColumnType("text");
                entity.Property(e => e.Biography)                       .HasColumnName("biography")                                 .HasColumnType("text");
                entity.Property(e => e.Website)                         .HasColumnName("website")                                   .HasColumnType("text");

                

                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
            });





            // band_memberships
            modelBuilder.Entity<BandMembershipEntityModel>(entity =>
            {
                entity.ToTable("band_memberships");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.BandId)                          .HasColumnName("band_id")                                   .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.UserId)                          .HasColumnName("user_id")                                   .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.Role)                            .HasColumnName("role")                                      .HasColumnType("text")                                                                                                              .IsRequired();



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Band)                              .WithMany()                                                 .HasForeignKey(e => e.BandId)           .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.User)                              .WithMany()                                                 .HasForeignKey(e => e.UserId)           .OnDelete(DeleteBehavior.Cascade);
            });





            // contacts
            modelBuilder.Entity<ContactEntityModel>(entity =>
            {
                entity.ToTable("contacts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.DisplayName)                     .HasColumnName("display_name")                              .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.EmailAddress)                    .HasColumnName("email_address")                             .HasColumnType("text");
                entity.Property(e => e.PhoneNumber)                     .HasColumnName("phone_number")                              .HasColumnType("text");
                entity.Property(e => e.UserId)                          .HasColumnName("user_id")                                   .HasColumnType("char(36)");



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.User)                              .WithMany()                                                 .HasForeignKey(e => e.UserId)           .OnDelete(DeleteBehavior.SetNull);
            });





            // dividends
            modelBuilder.Entity<DividendEntityModel>(entity =>
            {
                entity.ToTable("dividends");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.InvoiceId)                       .HasColumnName("invoice_id")                                .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.UserId)                          .HasColumnName("user_id")                                   .HasColumnType("char(36)");
                entity.Property(e => e.Amount)                          .HasColumnName("amount")                                    .HasColumnType("text")                                                                                                              .IsRequired();



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Invoice)                           .WithMany()                                                 .HasForeignKey(e => e.InvoiceId)        .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.User)                              .WithMany()                                                 .HasForeignKey(e => e.UserId)           .OnDelete(DeleteBehavior.SetNull);
            });





            // gig_availabilities
            modelBuilder.Entity<GigAvailabilityEntityModel>(entity =>
            {
                entity.ToTable("gig_availabilities");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.GigId)                           .HasColumnName("gig_id")                                    .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.BandMembershipId)                .HasColumnName("band_membership_id")                        .HasColumnType("char(36)");
                entity.Property(e => e.IsAvailable)                     .HasColumnName("is_available")                              .HasColumnType("tinyint(1)")                                                                                                        .IsRequired();
                entity.Property(e => e.PrimaryInstrument)               .HasColumnName("primary_instrument")                        .HasColumnType("text");



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Gig)                               .WithMany()                                                 .HasForeignKey(e => e.GigId)            .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.BandMembership)                    .WithMany()                                                 .HasForeignKey(e => e.BandMembershipId) .OnDelete(DeleteBehavior.SetNull);
            });





            // gigs
            modelBuilder.Entity<GigEntityModel>(entity =>
            {
                entity.ToTable("gigs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.Name)                            .HasColumnName("name")                                      .HasColumnType("text")                                                                                                          .IsRequired();
                entity.Property(e => e.Description)                     .HasColumnName("description")                               .HasColumnType("text");
                entity.Property(e => e.ClientContactId)                 .HasColumnName("client_contact_id")                         .HasColumnType("char(36)");
                entity.Property(e => e.VenueId)                         .HasColumnName("venue_id")                                  .HasColumnType("char(36)");
                entity.Property(e => e.PerformanceStartDate)            .HasColumnName("performance_start_date")                    .HasColumnType("date");
                entity.Property(e => e.PerformanceStartTime)            .HasColumnName("performance_start_time")                    .HasColumnType("time");
                entity.Property(e => e.PerformanceEndDate)              .HasColumnName("performance_end_date")                      .HasColumnType("date");
                entity.Property(e => e.PerformanceEndTime)              .HasColumnName("performance_end_time")                      .HasColumnType("time");
                entity.Property(e => e.LoadInDateTime)                  .HasColumnName("load_in_date_time")                         .HasColumnType("datetime");
                entity.Property(e => e.LoadOutDateTime)                 .HasColumnName("load_out_date_time")                        .HasColumnType("datetime");
                entity.Property(e => e.SoundCheckDateTime)              .HasColumnName("sound_check_date_time")                     .HasColumnType("datetime");
                entity.Property(e => e.Status)                          .HasColumnName("status")                                    .HasColumnType("text");



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Client)                            .WithMany()                                                 .HasForeignKey(e => e.ClientContactId)  .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Venue)                             .WithMany()                                                 .HasForeignKey(e => e.VenueId)          .OnDelete(DeleteBehavior.SetNull);
            });





            // invoices
            modelBuilder.Entity<InvoiceEntityModel>(entity =>
            {
                entity.ToTable("invoices");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.GigId)                           .HasColumnName("gig_id")                                    .HasColumnType("char(36)");



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Gig)                               .WithMany()                                                 .HasForeignKey(e => e.GigId)            .OnDelete(DeleteBehavior.SetNull);
            });





            // notes
            modelBuilder.Entity<NoteEntityModel>(entity =>
            {
                entity.ToTable("notes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.TableName)                       .HasColumnName("table_name")                                .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.TableRowId)                      .HasColumnName("table_row_id")                              .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.NoteContent)                     .HasColumnName("note_content")                              .HasColumnType("text")                                                                                                              .IsRequired();



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
            });





            // password_reset_tokens
            modelBuilder.Entity<PasswordResetTokenEntitymodel>(entity =>
            {
                entity.ToTable("password_reset_tokens");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.TokenHash)                       .HasColumnName("token_hash")                                .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.ExpiresAt)                       .HasColumnName("expires_at")                                .HasColumnType("datetime")                                                                                                          .IsRequired();



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
            });





            // quotes
            modelBuilder.Entity<QuoteEntityModel>(entity =>
            {
                entity.ToTable("quotes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.GigId)                           .HasColumnName("gig_id")                                    .HasColumnType("char(36)");
                entity.Property(e => e.ClientContactId)                 .HasColumnName("client_contact_id")                         .HasColumnType("char(36)");
                entity.Property(e => e.Amount)                          .HasColumnName("amount")                                    .HasColumnType("decimal(10,2)")                                                                                                     .IsRequired();
                entity.Property(e => e.Currency)                        .HasColumnName("currency")                                  .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.Status)                          .HasColumnName("status")                                    .HasColumnType("text")                                                                                                              .IsRequired();



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Gig)                               .WithMany()                                                 .HasForeignKey(e => e.GigId)            .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Client)                            .WithMany()                                                 .HasForeignKey(e => e.ClientContactId)  .OnDelete(DeleteBehavior.SetNull);
            });





            // refresh_tokens
            modelBuilder.Entity<RefreshTokenEntityModel>(entity =>
            {
                entity.ToTable("refresh_tokens");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.TokenHash)                       .HasColumnName("token_hash")                                .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.ExpiresAt)                       .HasColumnName("expires_at")                                .HasColumnType("datetime")                                                                                                          .IsRequired();



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
            });





            // set_lists
            modelBuilder.Entity<SetListEntityModel>(entity =>
            {
                entity.ToTable("set_lists");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.Description)                     .HasColumnName("description")                               .HasColumnType("text")                                                                                                          .IsRequired();



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
            });





            // set_list_items
            modelBuilder.Entity<SetListItemEntityModel>(entity =>
            {
                entity.ToTable("set_list_items");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.SetListId)                       .HasColumnName("set_list_id")                               .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.TrackId)                         .HasColumnName("track_id")                                  .HasColumnType("char(36)");



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.SetList)                           .WithMany()                                                 .HasForeignKey(e => e.SetListId)        .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Track)                             .WithMany()                                                 .HasForeignKey(e => e.TrackId)          .OnDelete(DeleteBehavior.SetNull);
            });





            // tracks
            modelBuilder.Entity<TrackEntityModel>(entity =>
            {
                entity.ToTable("tracks");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.Title)                           .HasColumnName("title")                                     .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.ArtistContactId)                 .HasColumnName("artist_contact_id")                         .HasColumnType("char(36)");
                entity.Property(e => e.DurationInSeconds)               .HasColumnName("duration_in_seconds")                       .HasColumnType("text");
                entity.Property(e => e.TimeSignature)                   .HasColumnName("time_signature")                            .HasColumnType("text");
                entity.Property(e => e.KeySignature)                    .HasColumnName("key_signature")                             .HasColumnType("text");
                entity.Property(e => e.TempoInBpm)                      .HasColumnName("tempo_in_bpm")                              .HasColumnType("text");
                entity.Property(e => e.Genre)                           .HasColumnName("genre")                                     .HasColumnType("text");



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.ArtistContact)                     .WithMany()                                                 .HasForeignKey(e => e.ArtistContactId)  .OnDelete(DeleteBehavior.SetNull);
            });





            // users
            modelBuilder.Entity<UserEntityModel>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.EmailAddress)                    .HasColumnName("email_address")                             .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.DisplayName)                     .HasColumnName("display_name")                              .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.PasswordHash)                    .HasColumnName("password_hash")                             .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.LastLogin)                       .HasColumnName("last_login")                                .HasColumnType("datetime");
                entity.Property(e => e.HasEmailAddressBeenVerified)     .HasColumnName("has_email_address_been_verified")           .HasColumnType("tinyint(1)")                                                                                                        .IsRequired();
                entity.Property(e => e.AllowLogin)                      .HasColumnName("allow_login")                               .HasColumnType("tinyint(1)")                                                                                                        .IsRequired();



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
            });





            // venues
            modelBuilder.Entity<VenueEntityModel>(entity =>
            {
                entity.ToTable("venues");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.Name)                            .HasColumnName("name")                                      .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.AddressId)                       .HasColumnName("address_id")                                .HasColumnType("char(36)");
                entity.Property(e => e.Capacity)                        .HasColumnName("capacity")                                  .HasColumnType("text");
                entity.Property(e => e.StageDimensions)                 .HasColumnName("stage_dimensions")                          .HasColumnType("text");
                entity.Property(e => e.ParkingInstructions)             .HasColumnName("parking_instructions")                      .HasColumnType("text");
                entity.Property(e => e.LoadInInstructions)              .HasColumnName("load_in_instructions")                      .HasColumnType("text");
                entity.Property(e => e.PrimaryContactId)                .HasColumnName("primary_contact_id")                        .HasColumnType("char(36)");



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Address)                           .WithMany()                                                 .HasForeignKey(e => e.AddressId)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.PrimaryContact)                    .WithMany()                                                 .HasForeignKey(e => e.PrimaryContactId) .OnDelete(DeleteBehavior.SetNull);
            });





            // venue_equipment
            modelBuilder.Entity<VenueEquipmentEntityModel>(entity =>
            {
                entity.ToTable("venue_equipment");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)                              .HasColumnName("id")                                        .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.CreatedAt)                       .HasColumnName("created_at")                                .HasColumnType("datetime")                                                      .HasDefaultValueSql("UTC_TIMESTAMP()")              .IsRequired();
                entity.Property(e => e.CreatedBy)                       .HasColumnName("created_by")                                .HasColumnType("char(36)");
                entity.Property(e => e.LastUpdatedAt)                   .HasColumnName("last_updated_at")                           .HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy)                   .HasColumnName("last_updated_by")                           .HasColumnType("char(36)");
                entity.Property(e => e.DeletedAt)                       .HasColumnName("deleted_at")                                .HasColumnType("datetime");
                entity.Property(e => e.DeletedBy)                       .HasColumnName("deleted_by")                                .HasColumnType("char(36)");
                
                entity.Property(e => e.VenueId)                         .HasColumnName("venue_id")                                  .HasColumnType("char(36)")                                                                                                          .IsRequired();
                entity.Property(e => e.Description)                     .HasColumnName("description")                               .HasColumnType("text")                                                                                                              .IsRequired();
                entity.Property(e => e.Available)                       .HasColumnName("available")                                 .HasColumnType("text")                                                                                                              .IsRequired();



                entity.HasOne(e => e.CreatedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.CreatedBy)        .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.LastUpdatedByUser)                 .WithMany()                                                 .HasForeignKey(e => e.LastUpdatedBy)    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.DeletedByUser)                     .WithMany()                                                 .HasForeignKey(e => e.DeletedBy)        .OnDelete(DeleteBehavior.SetNull);
                
                entity.HasOne(e => e.Venue)                             .WithMany()                                                 .HasForeignKey(e => e.VenueId)          .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
