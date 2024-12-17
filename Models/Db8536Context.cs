using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Seawise.Models;

public partial class Db8536Context : DbContext
{
    public Db8536Context()
    {
    }

    public Db8536Context(DbContextOptions<Db8536Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }

    public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }

    public virtual DbSet<InspectionCriteria> InspectionCriterias { get; set; }

    public virtual DbSet<InspectionFinding> InspectionFindings { get; set; }

    public virtual DbSet<InspectionParticipant> InspectionParticipants { get; set; }

    public virtual DbSet<InspectionRecord> InspectionRecords { get; set; }

    public virtual DbSet<InspectionType> InspectionTypes { get; set; }

    public virtual DbSet<MaintenanceParticipant> MaintenanceParticipants { get; set; }

    public virtual DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

    public virtual DbSet<Ship> Ships { get; set; }

    public virtual DbSet<ShipEquipment> ShipEquipments { get; set; }

    public virtual DbSet<ShipEquipmentCategory> ShipEquipmentCategories { get; set; }

    public virtual DbSet<ShipOwner> ShipOwners { get; set; }

    public virtual DbSet<ShipType> ShipTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db8536.public.databaseasp.net;Database=db8536;User Id=db8536;Password=R!d4?t7WaJ@2;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryCode);

            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.InternalEmployeeCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PhotoURL");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.EmployeePosition).WithMany(p => p.Employees)
                .HasForeignKey(d => d.EmployeePositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_EmployeePositions");
        });

        modelBuilder.Entity<EmployeeDepartment>(entity =>
        {
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EmployeePosition>(entity =>
        {
            entity.Property(e => e.PositionName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.EmployeeDepartment).WithMany(p => p.EmployeePositions)
                .HasForeignKey(d => d.EmployeeDepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeePositions_EmployeeDepartments");
        });

        modelBuilder.Entity<InspectionCriteria>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.InspectionType).WithMany(p => p.InspectionCriteria)
                .HasForeignKey(d => d.InspectionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InspectionCriterias_InspectionTypes");
        });

        modelBuilder.Entity<InspectionFinding>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.InspectionCriteria).WithMany(p => p.InspectionFindings)
                .HasForeignKey(d => d.InspectionCriteriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InspectionFindings_InspectionCriterias");

            entity.HasOne(d => d.InspectionRecord).WithMany(p => p.InspectionFindings)
                .HasForeignKey(d => d.InspectionRecordId)
                .HasConstraintName("FK_InspectionFindings_InspectionRecords");

            entity.HasOne(d => d.InspectionType).WithMany(p => p.InspectionFindings)
                .HasForeignKey(d => d.InspectionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InspectionFindings_InspectionTypes");
        });

        modelBuilder.Entity<InspectionParticipant>(entity =>
        {
            entity.HasOne(d => d.Employee).WithMany(p => p.InspectionParticipants)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InspectionParticipants_Employees");

            entity.HasOne(d => d.InspectionRecord).WithMany(p => p.InspectionParticipants)
                .HasForeignKey(d => d.InspectionRecordId)
                .HasConstraintName("FK_InspectionParticipants_InspectionRecords");
        });

        modelBuilder.Entity<InspectionRecord>(entity =>
        {
            entity.Property(e => e.Time).HasColumnType("datetime");

            entity.HasOne(d => d.InspectionType).WithMany(p => p.InspectionRecords)
                .HasForeignKey(d => d.InspectionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InspectionRecords_InspectionTypes");

            entity.HasOne(d => d.Ship).WithMany(p => p.InspectionRecords)
                .HasForeignKey(d => d.ShipId)
                .HasConstraintName("FK_InspectionRecords_Ships");
        });

        modelBuilder.Entity<InspectionType>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MaintenanceParticipant>(entity =>
        {
            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.StartedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.MaintenanceParticipants)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaintenanceParticipants_Employees");

            entity.HasOne(d => d.MaintenanceRecord).WithMany(p => p.MaintenanceParticipants)
                .HasForeignKey(d => d.MaintenanceRecordId)
                .HasConstraintName("FK_MaintenanceParticipants_MaintenanceRecords");
        });

        modelBuilder.Entity<MaintenanceRecord>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Time).HasColumnType("datetime");

            entity.HasOne(d => d.ShipEquipment).WithMany(p => p.MaintenanceRecords)
                .HasForeignKey(d => d.ShipEquipmentId)
                .HasConstraintName("FK_MaintenanceRecords_ShipEquipments");
        });

        modelBuilder.Entity<Ship>(entity =>
        {
            entity.HasKey(e => e.ShipId).HasName("PK__SHIPS");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Imonumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("IMONumber");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PhotoURL");
            entity.Property(e => e.ShipName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.Ships)
                .HasForeignKey(d => d.CountryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ships_Countries");

            entity.HasOne(d => d.ShipOwner).WithMany(p => p.Ships)
                .HasForeignKey(d => d.ShipOwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ships_ShipOwners");

            entity.HasOne(d => d.ShipType).WithMany(p => p.Ships)
                .HasForeignKey(d => d.ShipTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ships_ShipTypes");
        });

        modelBuilder.Entity<ShipEquipment>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EquipmentName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PhotoURL");

            entity.HasOne(d => d.ShipEquipmentCategory).WithMany(p => p.ShipEquipments)
                .HasForeignKey(d => d.ShipEquipmentCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShipEquipments_ShipEquipmentCategories");

            entity.HasOne(d => d.Ship).WithMany(p => p.ShipEquipments)
                .HasForeignKey(d => d.ShipId)
                .HasConstraintName("FK_ShipEquipments_Ships");
        });

        modelBuilder.Entity<ShipEquipmentCategory>(entity =>
        {
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ShipOwner>(entity =>
        {
            entity.HasKey(e => e.ShipOwnerId).HasName("PK__OWNERS");

            entity.Property(e => e.Address)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("PhotoURL");

            entity.HasOne(d => d.CountryCodeNavigation).WithMany(p => p.ShipOwners)
                .HasForeignKey(d => d.CountryCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ShipOwners_Countries");
        });

        modelBuilder.Entity<ShipType>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ShipTypeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
