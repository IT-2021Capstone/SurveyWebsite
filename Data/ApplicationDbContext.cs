using System;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SurveyWebsite.Data
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        // public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        //public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        //public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        //public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<MutipleAnswerQoftheDay> MutipleAnswerQoftheDays { get; set; }
        public virtual DbSet<MutipleChoiceResponse> MutipleChoiceResponses { get; set; }
        public virtual DbSet<MutipleChoiceText> MutipleChoiceTexts { get; set; }
        public virtual DbSet<OpenEndedResponse> OpenEndedResponses { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionOfTheDay> QuestionOfTheDays { get; set; }
        public virtual DbSet<QuestionOfTheDayOpenResponse> QuestionOfTheDayOpenResponses { get; set; }
        public virtual DbSet<QuestionOfTheDayResponse> QuestionOfTheDayResponses { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<SurveyOrder> SurveyOrders { get; set; }
        public virtual DbSet<SurveyTaken> SurveyTakens { get; set; }
        public virtual DbSet<Surveylist> Surveylists { get; set; }
        public virtual DbSet<TrueFalseResponse> TrueFalseResponses { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning Change to Azure Connection
                optionsBuilder.UseSqlServer("DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<IdentityUserLogin<string>>();
            //modelBuilder.Ignore<IdentityUserRole<string>>();
           // modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
           // modelBuilder.Ignore<IdentityUser<string>>();
           // modelBuilder.Ignore<IdentityRole<string>>();
            modelBuilder.Entity<AspNetUserRole>().HasKey(p => new { p.UserId, p.RoleId });
            //modelBuilder.Entity<AspNetRole>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedName] IS NOT NULL)");

            //    entity.Property(e => e.Name).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedName).HasMaxLength(256);
            //});

            //modelBuilder.Entity<AspNetRoleClaim>(entity =>
            //{
            //    entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            //    entity.Property(e => e.RoleId).IsRequired();

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetRoleClaims)
            //        .HasForeignKey(d => d.RoleId);
            //});

            //modelBuilder.Entity<AspNetUser>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            //    entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedUserName] IS NOT NULL)");

            //    entity.Property(e => e.Email).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

            //    entity.Property(e => e.UserName).HasMaxLength(256);
            //});

            //modelBuilder.Entity<AspNetUserClaim>(entity =>
            //{
            //    entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            //    entity.Property(e => e.UserId).IsRequired();

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserClaims)
            //        .HasForeignKey(d => d.UserId);
            //});

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            //modelBuilder.Entity<AspNetUserRole>(entity =>
            //{
            //    entity.HasKey(e => new { e.UserId, e.RoleId });

            //    entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");


            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.RoleId);

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.UserId);
            //});

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<MutipleAnswerQoftheDay>(entity =>
            {
                entity.ToTable("MutipleAnswerQoftheDay");

                entity.Property(e => e.MutipleAnswerQoftheDayId).HasColumnName("MutipleAnswerQoftheDayID");

                entity.Property(e => e.QuestionOfTheDayId).HasColumnName("QuestionOfTheDayID");

                entity.HasOne(d => d.QuestionOfTheDay)
                    .WithMany(p => p.MutipleAnswerQoftheDays)
                    .HasForeignKey(d => d.QuestionOfTheDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MutipleAnswerQoftheDay_QuestionOfTheDay");
            });

            modelBuilder.Entity<MutipleChoiceResponse>(entity =>
            {
                entity.HasKey(e => e.MutipleChoiceId)
                    .HasName("PK_MutipleChoice");

                entity.HasIndex(e => e.QuestionId, "IX_MutipleChoiceResponses_QuestionID");

                entity.Property(e => e.MutipleChoiceId).HasColumnName("MutipleChoiceID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.MutipleChoiceResponses)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MutipleChoiceResponses_Questions");
            });

            modelBuilder.Entity<MutipleChoiceText>(entity =>
            {
                entity.HasKey(e => e.MutipleChoiceAnswerId);

                entity.ToTable("MutipleChoiceText");

                entity.HasIndex(e => e.QuestionId, "IX_MutipleChoiceText_QuestionID");

                entity.Property(e => e.MutipleChoiceAnswerId).HasColumnName("MutipleChoiceAnswerID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.MutipleChoiceTexts)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_MutipleChoiceText_Questions");
            });

            modelBuilder.Entity<OpenEndedResponse>(entity =>
            {
                entity.HasKey(e => e.OpenEndedId)
                    .HasName("PK_OpenEndedQuestion");

                entity.HasIndex(e => e.QuestionId, "IX_OpenEndedResponses_QuestionID");

                entity.Property(e => e.OpenEndedId).HasColumnName("OpenEndedID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.OpenEndedResponses)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OpenEndedResponses_Questions1");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasIndex(e => e.QuestionType, "IX_Questions_QuestionType");

                entity.HasIndex(e => e.SurveyId, "IX_Questions_SurveyID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.HasOne(d => d.QuestionTypeNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Questions_QuestionType");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.SurveyId)
                    .HasConstraintName("FK_Questions_Surveylist");
            });

            modelBuilder.Entity<QuestionOfTheDay>(entity =>
            {
                entity.ToTable("QuestionOfTheDay");

                entity.Property(e => e.QuestionOfTheDayId).HasColumnName("QuestionOfTheDayID");

                entity.Property(e => e.DateEnded).HasColumnType("datetime");

                entity.Property(e => e.DateStarted).HasColumnType("datetime");
            });

            modelBuilder.Entity<QuestionOfTheDayOpenResponse>(entity =>
            {
                entity.HasKey(e => e.QuestionOfTheDayOpenResponsesId);

                entity.ToTable("QuestionOfTheDayOpenResponse");

                entity.Property(e => e.QuestionOfTheDayOpenResponsesId).HasColumnName("QuestionOfTheDayOpenResponsesID");

                entity.Property(e => e.QuestionOfTheDayId).HasColumnName("QuestionOfTheDayID");

                entity.Property(e => e.QuestionOfTheDayOpenResponse1).HasColumnName("QuestionOfTheDayOpenResponse");

                entity.HasOne(d => d.QuestionOfTheDay)
                    .WithMany(p => p.QuestionOfTheDayOpenResponses)
                    .HasForeignKey(d => d.QuestionOfTheDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionOfTheDayOpenResponse_QuestionOfTheDay");
            });

            modelBuilder.Entity<QuestionOfTheDayResponse>(entity =>
            {
                entity.HasKey(e => e.QuestionOfTheDayResoponseId);

                entity.HasIndex(e => e.QuestionOfTheDayId, "IX_QuestionOfTheDayResponses_QuestionOfTheDayID");

                entity.Property(e => e.QuestionOfTheDayResoponseId).HasColumnName("QuestionOfTheDayResoponseID");

                entity.Property(e => e.QuestionOfTheDayId).HasColumnName("QuestionOfTheDayID");

                entity.HasOne(d => d.QuestionOfTheDay)
                    .WithMany(p => p.QuestionOfTheDayResponses)
                    .HasForeignKey(d => d.QuestionOfTheDayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionOfTheDayResponses_QuestionOfTheDay");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.ToTable("QuestionType");

                entity.Property(e => e.QuestionTypeId).HasColumnName("QuestionTypeID");

                entity.Property(e => e.QuestionType1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("QuestionType");
            });

            modelBuilder.Entity<SurveyOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SurveyOrder");

                entity.HasIndex(e => e.QuestionOfTheDayId, "IX_SurveyOrder_QuestionOfTheDayID");

                entity.HasIndex(e => e.SurveyId, "IX_SurveyOrder_SurveyID");

                entity.Property(e => e.EndTime).HasColumnType("datetime");

                entity.Property(e => e.QuestionOfTheDayId).HasColumnName("QuestionOfTheDayID");

                entity.Property(e => e.StartTime).HasColumnType("datetime");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.HasOne(d => d.QuestionOfTheDay)
                    .WithMany()
                    .HasForeignKey(d => d.QuestionOfTheDayId)
                    .HasConstraintName("FK_SurveyOrder_QuestionOfTheDay");

                entity.HasOne(d => d.Survey)
                    .WithMany()
                    .HasForeignKey(d => d.SurveyId)
                    .HasConstraintName("FK_SurveyOrder_Surveylist");
            });

            modelBuilder.Entity<SurveyTaken>(entity =>
            {
                entity.ToTable("SurveyTaken");

                entity.HasIndex(e => e.LoginId, "IX_SurveyTaken_LoginID");

                entity.HasIndex(e => e.SurveyId, "IX_SurveyTaken_SurveyID");

                entity.Property(e => e.SurveyTakenId).HasColumnName("SurveyTakenID");

                entity.Property(e => e.LoginId)
                    .IsRequired()
                    .HasColumnName("LoginID");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.HasOne(d => d.Login)
                    .WithMany(p => p.SurveyTakens)
                    .HasForeignKey(d => d.LoginId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyTaken_AspNetUsers");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.SurveyTakens)
                    .HasForeignKey(d => d.SurveyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyTaken_Surveylist");
            });

            modelBuilder.Entity<Surveylist>(entity =>
            {
                entity.HasKey(e => e.SurveyId);

                entity.ToTable("Surveylist");

                entity.HasIndex(e => e.UserId, "IX_Surveylist_UserID");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Surveylists)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Surveylist_AspNetUsers");
            });

            modelBuilder.Entity<TrueFalseResponse>(entity =>
            {
                entity.HasKey(e => e.TrueFalseId)
                    .HasName("PK_TrueFalseResponse");

                entity.HasIndex(e => e.QuestionId, "IX_TrueFalseResponses_QuestionID");

                entity.Property(e => e.TrueFalseId).HasColumnName("TrueFalseID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TrueFalseResponses)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TrueFalseResponses_Questions1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
