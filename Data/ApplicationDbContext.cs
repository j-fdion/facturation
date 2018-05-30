using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppFactu.Models;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AppFactu.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Personne> Personnes { get; set; }

        public DbSet<Formation> Formations { get; set; }

        public DbSet<Formateur> Formateurs { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Entreprise> Entreprises { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<PersonneSession> PersonneSessions { get; set; }

        public DbSet<Salle> Salles { get; set; }

        public DbSet<Facture> Factures { get; set; }

        public DbSet<FraisSupplementaire> FraisSupplementaires { get; set; }

        public DbSet<FraisSupplementaireSession> FraisSupplementaireSessions { get; set; }

        public DbSet<FormateurFormation> FormateurFormations { get; set; }

        public DbSet<Accessoire> Accessoires { get; set; }

        public DbSet<TypeAccessoire> TypeAccessoires { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public DbSet<ParticipantSession> ParticipantSessions { get; set; }

        public DbSet<ContactEntreprise> ContactEntreprises { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<PersonneSession>()
            .HasKey(t => new { t.PersonneId, t.SessionId });

            builder.Entity<FraisSupplementaireSession>()
            .HasKey(t => new { t.SessionId, t.FraisSupplementaireId });

            builder.Entity<Facture>().Property(u => u.NumericId).UseSqlServerIdentityColumn();
            builder.Entity<Facture>().Property(u => u.NumericId).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Entity<FormateurFormation>()
            .HasKey(t => new { t.FormateurId, t.FormationId });

            builder.Entity<FormateurFormation>()
            .HasOne(pt => pt.Formateur)
            .WithMany(p => p.FormateurFormations)
            .HasForeignKey(pt => pt.FormateurId);

            builder.Entity<FormateurFormation>()
            .HasOne(pt => pt.Formation)
            .WithMany(t => t.FormateurFormations)
            .HasForeignKey(pt => pt.FormationId);

            builder.Entity<ParticipantSession>()
            .HasKey(t => new { t.ParticipantId, t.SessionId });

            builder.Entity<ContactEntreprise>()
            .HasKey(t => new { t.ContactId, t.EntrepriseId });

            builder.Entity<ContactEntreprise>()
                .HasOne(pt => pt.Contact)
                .WithMany(p => p.ContactEntreprises)
                .HasForeignKey(pt => pt.ContactId);

            builder.Entity<ContactEntreprise>()
                .HasOne(pt => pt.Entreprise)
                .WithMany(t => t.ContactEntreprises)
                .HasForeignKey(pt => pt.EntrepriseId);

            builder.Entity<Session>().HasOne(x => x.Entreprise)
            .WithMany()
            .HasForeignKey(x => x.EntrepriseId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Session>().HasOne(x => x.Contact)
            .WithMany()
            .HasForeignKey(x => x.ContactId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Session>().HasOne(x => x.Formation)
            .WithMany()
            .HasForeignKey(x => x.FormationId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Session>().HasOne(x => x.Formateur)
            .WithMany()
            .HasForeignKey(x => x.FormateurId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Session>().HasOne(x => x.Salle)
            .WithMany()
            .HasForeignKey(x => x.SalleId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        }
    }
}

