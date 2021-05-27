using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.Models
{
    public partial class SurveySiteContext : DbContext
    {
        public SurveySiteContext()
        {
        }

        public SurveySiteContext(DbContextOptions<SurveySiteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<MutipleChoiceResponse> MutipleChoiceResponses { get; set; }
        public virtual DbSet<MutipleChoiceText> MutipleChoiceTexts { get; set; }
        public virtual DbSet<OpenEndedResponse> OpenEndedResponses { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionOfTheDay> QuestionOfTheDays { get; set; }
        public virtual DbSet<QuestionOfTheDayResponse> QuestionOfTheDayResponses { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<RoleType> RoleTypes { get; set; }
        public virtual DbSet<SurveyOrder> SurveyOrders { get; set; }
        public virtual DbSet<SurveyTaken> SurveyTakens { get; set; }
        public virtual DbSet<Surveylist> Surveylists { get; set; }
        public virtual DbSet<TrueFalseResponse> TrueFalseResponses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=SurveySite;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Login>(entity =>
            {
                entity.Property(e => e.LoginId).HasColumnName("LoginID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LoginUserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.UserAddress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.Logins)
                    .HasForeignKey(d => d.Role)
                    .HasConstraintName("FK_Logins_RoleType");
            });

            modelBuilder.Entity<MutipleChoiceResponse>(entity =>
            {
                entity.HasKey(e => e.MutipleChoiceId)
                    .HasName("PK_MutipleChoice");

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

            modelBuilder.Entity<QuestionOfTheDayResponse>(entity =>
            {
                entity.HasKey(e => e.QuestionOfTheDayResoponseId);

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

            modelBuilder.Entity<RoleType>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("RoleType");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");
            });

            modelBuilder.Entity<SurveyOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SurveyOrder");

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

                entity.Property(e => e.SurveyTakenId).HasColumnName("SurveyTakenID");

                entity.Property(e => e.LoginId).HasColumnName("LoginID");

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.HasOne(d => d.Login)
                    .WithMany(p => p.SurveyTakens)
                    .HasForeignKey(d => d.LoginId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SurveyTaken_Logins");

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

                entity.Property(e => e.SurveyId).HasColumnName("SurveyID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.LoginId).HasColumnName("LoginID");

                entity.HasOne(d => d.Login)
                    .WithMany(p => p.Surveylists)
                    .HasForeignKey(d => d.LoginId)
                    .HasConstraintName("FK_Surveylist_Logins");
            });

            modelBuilder.Entity<TrueFalseResponse>(entity =>
            {
                entity.HasKey(e => e.TrueFalseId)
                    .HasName("PK_TrueFalseResponse");

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
