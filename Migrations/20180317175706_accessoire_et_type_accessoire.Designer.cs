﻿// <auto-generated />
using AppFactu.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace AppFactu.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180317175706_accessoire_et_type_accessoire")]
    partial class accessoire_et_type_accessoire
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AppFactu.Models.Accessoire", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<string>("Modele")
                        .IsRequired();

                    b.Property<float>("Prix");

                    b.Property<Guid>("TypeAccessoireId");

                    b.HasKey("Id");

                    b.HasIndex("TypeAccessoireId");

                    b.ToTable("Accessoires");
                });

            modelBuilder.Entity("AppFactu.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AppFactu.Models.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Commentaire");

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<string>("Email");

                    b.Property<Guid?>("EntrepriseId");

                    b.Property<string>("NoTelephone1")
                        .IsRequired();

                    b.Property<string>("NoTelephone2");

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.Property<string>("Prenom")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EntrepriseId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("AppFactu.Models.Entreprise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Entreprises");
                });

            modelBuilder.Entity("AppFactu.Models.Facture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BonCommande");

                    b.Property<DateTime?>("Date");

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<Guid?>("EntrepriseId");

                    b.Property<string>("NoFacture");

                    b.Property<int>("NumericId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("PeriodeDebut");

                    b.Property<DateTime?>("PeriodeFin");

                    b.HasKey("Id");

                    b.HasIndex("EntrepriseId");

                    b.ToTable("Factures");
                });

            modelBuilder.Entity("AppFactu.Models.Formateur", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.Property<string>("Prenom")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Formateurs");
                });

            modelBuilder.Entity("AppFactu.Models.FormateurFormation", b =>
                {
                    b.Property<Guid>("FormateurId");

                    b.Property<Guid>("FormationId");

                    b.HasKey("FormateurId", "FormationId");

                    b.HasIndex("FormationId");

                    b.ToTable("FormateurFormations");
                });

            modelBuilder.Entity("AppFactu.Models.Formation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<float>("Duree");

                    b.Property<float>("PrixForfaitaire");

                    b.Property<float>("TauxHoraire");

                    b.Property<string>("Titre")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Formations");
                });

            modelBuilder.Entity("AppFactu.Models.FraisSupplementaire", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("CoutUnite");

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.Property<string>("NomQuantite")
                        .IsRequired();

                    b.Property<string>("NomUnite")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("FraisSupplementaires");
                });

            modelBuilder.Entity("AppFactu.Models.FraisSupplementaireSession", b =>
                {
                    b.Property<Guid>("SessionId");

                    b.Property<Guid>("FraisSupplementaireId");

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<float>("Quantite");

                    b.HasKey("SessionId", "FraisSupplementaireId");

                    b.HasIndex("FraisSupplementaireId");

                    b.ToTable("FraisSupplementaireSessions");
                });

            modelBuilder.Entity("AppFactu.Models.Personne", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Commentaire");

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<DateTime?>("DateNaissance")
                        .IsRequired();

                    b.Property<Guid>("EntrepriseId");

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.Property<string>("Prenom")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("EntrepriseId");

                    b.ToTable("Personnes");
                });

            modelBuilder.Entity("AppFactu.Models.PersonneSession", b =>
                {
                    b.Property<Guid>("PersonneId");

                    b.Property<Guid>("SessionId");

                    b.Property<Guid?>("FactureId");

                    b.HasKey("PersonneId", "SessionId");

                    b.HasIndex("FactureId");

                    b.HasIndex("SessionId");

                    b.ToTable("PersonneSessions");
                });

            modelBuilder.Entity("AppFactu.Models.Salle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Salles");
                });

            modelBuilder.Entity("AppFactu.Models.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BonCommande");

                    b.Property<DateTimeOffset?>("Date")
                        .IsRequired();

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<Guid?>("FactureId");

                    b.Property<Guid>("FormateurId");

                    b.Property<Guid>("FormationId");

                    b.Property<Guid>("SalleId");

                    b.HasKey("Id");

                    b.HasIndex("FactureId");

                    b.HasIndex("FormateurId");

                    b.HasIndex("FormationId");

                    b.HasIndex("SalleId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("AppFactu.Models.TypeAccessoire", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("DateCreation");

                    b.Property<DateTimeOffset?>("DateModification");

                    b.Property<string>("Nom")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("TypeAccessoires");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AppFactu.Models.Accessoire", b =>
                {
                    b.HasOne("AppFactu.Models.TypeAccessoire", "TypeAccessoire")
                        .WithMany()
                        .HasForeignKey("TypeAccessoireId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AppFactu.Models.Contact", b =>
                {
                    b.HasOne("AppFactu.Models.Entreprise")
                        .WithMany("Contacts")
                        .HasForeignKey("EntrepriseId");
                });

            modelBuilder.Entity("AppFactu.Models.Facture", b =>
                {
                    b.HasOne("AppFactu.Models.Entreprise", "Entreprise")
                        .WithMany()
                        .HasForeignKey("EntrepriseId");
                });

            modelBuilder.Entity("AppFactu.Models.FormateurFormation", b =>
                {
                    b.HasOne("AppFactu.Models.Formateur", "Formateur")
                        .WithMany("FormateurFormations")
                        .HasForeignKey("FormateurId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AppFactu.Models.Formation", "Formation")
                        .WithMany("FormateurFormations")
                        .HasForeignKey("FormationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AppFactu.Models.FraisSupplementaireSession", b =>
                {
                    b.HasOne("AppFactu.Models.FraisSupplementaire", "FraisSupplementaire")
                        .WithMany("FraisSupplementaireSessions")
                        .HasForeignKey("FraisSupplementaireId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AppFactu.Models.Session", "Session")
                        .WithMany("FraisSupplementaireSessions")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AppFactu.Models.Personne", b =>
                {
                    b.HasOne("AppFactu.Models.Entreprise", "Entreprise")
                        .WithMany()
                        .HasForeignKey("EntrepriseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AppFactu.Models.PersonneSession", b =>
                {
                    b.HasOne("AppFactu.Models.Facture")
                        .WithMany("PersonneSessions")
                        .HasForeignKey("FactureId");

                    b.HasOne("AppFactu.Models.Personne", "Personne")
                        .WithMany("PersonneSessions")
                        .HasForeignKey("PersonneId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AppFactu.Models.Session", "Session")
                        .WithMany("PersonneSessions")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AppFactu.Models.Session", b =>
                {
                    b.HasOne("AppFactu.Models.Facture")
                        .WithMany("Sessions")
                        .HasForeignKey("FactureId");

                    b.HasOne("AppFactu.Models.Formateur", "Formateur")
                        .WithMany()
                        .HasForeignKey("FormateurId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AppFactu.Models.Formation", "Formation")
                        .WithMany()
                        .HasForeignKey("FormationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AppFactu.Models.Salle", "Salle")
                        .WithMany()
                        .HasForeignKey("SalleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AppFactu.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AppFactu.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AppFactu.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AppFactu.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
