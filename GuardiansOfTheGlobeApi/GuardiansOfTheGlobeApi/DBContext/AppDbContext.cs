using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GuardiansOfTheGlobeApi.Models;

namespace GuardiansOfTheGlobeApi.DBContext
{
    public partial class AppDbContext : DbContext
    {
        internal object VillanoPeleasResult;

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Agenda> Agenda { get; set; } = null!;
        public virtual DbSet<Hero> Heroes { get; set; } = null!;
        public virtual DbSet<Patrocinador> Patrocinadores { get; set; } = null!;
        public virtual DbSet<Pelea> Peleas { get; set; } = null!;
        public virtual DbSet<Villano> Villanos { get; set; } = null!;
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agenda>(entity =>
            {
                entity.HasOne(d => d.IdHeroeNavigation)
                    .WithMany(p => p.Agenda)
                    .HasForeignKey(d => d.IdHeroe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__agenda__id_heroe__3E52440B");
            });
            modelBuilder.Entity<Patrocinador>(entity =>
            {
                entity.HasOne(d => d.IdHeroeNavigation)
                    .WithMany(p => p.Patrocinadores)
                    .HasForeignKey(d => d.IdHeroe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__patrocina__id_he__412EB0B6");
            });

            modelBuilder.Entity<Pelea>(entity =>
            {
                entity.HasOne(d => d.IdHeroeNavigation)
                    .WithMany(p => p.Peleas)
                    .HasForeignKey(d => d.IdHeroe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__peleas__id_heroe__440B1D61");

                entity.HasOne(d => d.IdVillanoNavigation)
                    .WithMany(p => p.Peleas)
                    .HasForeignKey(d => d.IdVillano)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__peleas__id_villa__44FF419A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
