using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Data.Context;

public partial class AquaVivariumContext : IdentityDbContext<ApplicationUser>
{  
    public AquaVivariumContext()
    {
    }

    public AquaVivariumContext(DbContextOptions<AquaVivariumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acuario> Acuarios { get; set; }

    public virtual DbSet<AcuarioEspecie> AcuarioEspecies { get; set; }

    public virtual DbSet<EspecieConsulta> EspecieConsultas { get; set; }

    public virtual DbSet<EspecieImagen> EspecieImagenes { get; set; }

    public virtual DbSet<EspecieRespuesta> EspecieRespuestas { get; set; }

    public virtual DbSet<Especie> Especies { get; set; }

    public virtual DbSet<EstilosAquascaping> EstilosAquascapings { get; set; }

    public virtual DbSet<EstilosAquascapingImagen> EstilosAquascapingImagenes { get; set; }

    public virtual DbSet<Pez> Peces { get; set; }

    public virtual DbSet<Planta> Plantas { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Acuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Acuarios__3214EC0767DBFA4E");

            entity.HasOne(d => d.Estilo).WithMany(p => p.Acuarios)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Acuario_Estilo");
        });

        modelBuilder.Entity<AcuarioEspecie>(entity =>
        {
            entity.HasKey(e => new { e.AcuarioId, e.EspecieId }).HasName("PK__AcuarioE__B6CBBCE756037EAC");

            entity.Property(e => e.Cantidad).HasDefaultValue(1);

            entity.HasOne(d => d.Acuario).WithMany(p => p.AcuarioEspecies).HasConstraintName("FK_Acuario_Rel");

            entity.HasOne(d => d.Especie).WithMany(p => p.AcuarioEspecies).HasConstraintName("FK_Especie_Rel");
        });

        modelBuilder.Entity<EspecieConsulta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EspecieC__3214EC07F7DEB0CB");

            entity.Property(e => e.FechaPublicacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Especie).WithMany(p => p.EspecieConsulta).HasConstraintName("FK_Consultas_Especies");
        });

        modelBuilder.Entity<EspecieImagen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EspecieI__3214EC074E78620F");

            entity.HasOne(d => d.Especie).WithMany(p => p.EspecieImagenes).HasConstraintName("FK_Imagenes_Especies");
        });

        modelBuilder.Entity<EspecieRespuesta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EspecieR__3214EC079D2D1592");

            entity.Property(e => e.FechaPublicacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Consulta).WithMany(p => p.EspecieRespuesta).HasConstraintName("FK_Respuestas_Consultas");
        });

        modelBuilder.Entity<Especie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Especies__3214EC072D8298EA");
        });

        modelBuilder.Entity<EstilosAquascaping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EstilosA__3214EC073E29BF9B");
        });

        modelBuilder.Entity<EstilosAquascapingImagen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EstiloAq__3214EC07ADBD1721");

            entity.HasOne(d => d.Estilo).WithMany(p => p.EstilosAquascapingImagenes).HasConstraintName("FK_Estilo_Imagenes");
        });

        modelBuilder.Entity<Pez>(entity =>
        {
            entity.HasKey(e => e.EspecieId).HasName("PK__Peces__9CF6043CF1EA2CB2");

            entity.Property(e => e.EspecieId).ValueGeneratedNever();

            entity.HasOne(d => d.Especie).WithOne(p => p.Pez).HasConstraintName("FK_Peces_Especies");
        });

        modelBuilder.Entity<Planta>(entity =>
        {
            entity.HasKey(e => e.EspecieId).HasName("PK__Plantas__9CF6043CC082378B");

            entity.Property(e => e.EspecieId).ValueGeneratedNever();
            entity.Property(e => e.NecesitaCo2).HasDefaultValue(false);

            entity.HasOne(d => d.Especie).WithOne(p => p.Planta).HasConstraintName("FK_Plantas_Especies");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
