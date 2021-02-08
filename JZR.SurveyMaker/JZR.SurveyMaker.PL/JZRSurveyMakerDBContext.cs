using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace JZR.SurveyMaker.PL
{
    public partial class SurveyEntities : DbContext
    {
        public SurveyEntities()
        {
        }

        public SurveyEntities(DbContextOptions<SurveyEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<tblAnswer> tblAnswers { get; set; }
        public virtual DbSet<tblQuestion> tblQuestions { get; set; }
        public virtual DbSet<tblQuestionAnswer> tblQuestionAnswers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectsV13;Database=JZR.SurveyMaker.DB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<tblAnswer>(entity =>
            {
                entity.ToTable("tblAnswer");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<tblQuestion>(entity =>
            {
                entity.ToTable("tblQuestion");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Question).IsRequired();
            });

            modelBuilder.Entity<tblQuestionAnswer>(entity =>
            {
                entity.ToTable("tblQuestionAnswer");

                entity.HasIndex(e => new { e.AnswerId, e.QuestionId }, "UQ__tblQuest__045E56DD319463CA")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.TblQuestionAnswers)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblQuesti__Answe__286302EC");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.TblQuestionAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblQuesti__Quest__29572725");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
