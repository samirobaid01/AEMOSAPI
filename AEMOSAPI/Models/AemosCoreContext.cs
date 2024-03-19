using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AEMOSAPI.Models;

public partial class AemosCoreContext : DbContext
{
    public AemosCoreContext()
    {
    }

    public AemosCoreContext(DbContextOptions<AemosCoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<AreaDevice> AreaDevices { get; set; }

    public virtual DbSet<DataStream> DataStreams { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<TelemetryDatum> TelemetryData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=aemos_core;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Area>(entity =>
        {
            entity.ToTable("Area");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Image)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OrganizationId).HasColumnName("organizationId");
            entity.Property(e => e.ParentArea).HasColumnName("parentArea");
            entity.Property(e => e.Uuid).HasColumnName("uuid");

            entity.HasOne(d => d.Organization).WithMany(p => p.Areas)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_Area_Organization");
        });

        modelBuilder.Entity<AreaDevice>(entity =>
        {
            entity.HasKey(e => new { e.AreaId, e.DeviceId });

            entity.ToTable("AreaDevice");

            entity.Property(e => e.AreaId).HasColumnName("areaId");
            entity.Property(e => e.DeviceId).HasColumnName("deviceId");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Detail)
                .IsUnicode(false)
                .HasColumnName("detail");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Area).WithMany(p => p.AreaDevices)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AreaDevice_Area");

            entity.HasOne(d => d.Device).WithMany(p => p.AreaDevices)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AreaDevice_Device");
        });

        modelBuilder.Entity<DataStream>(entity =>
        {
            entity.ToTable("DataStream");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RecievedAt)
                .HasColumnType("datetime")
                .HasColumnName("recievedAt");
            entity.Property(e => e.TelemetryDataId).HasColumnName("telemetryDataId");
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("value");

            entity.HasOne(d => d.TelemetryData).WithMany(p => p.DataStreams)
                .HasForeignKey(d => d.TelemetryDataId)
                .HasConstraintName("FK_DataStream_TelemetryData");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Device");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.State)
                .HasColumnType("text")
                .HasColumnName("state");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Uuid).HasColumnName("uuid");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.Insight).HasColumnType("text");
            entity.Property(e => e.Severity).HasMaxLength(50);
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserGroup).HasColumnType("text");

            entity.HasOne(d => d.Organization).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.OrganizationId)
                .HasConstraintName("FK_Notification_Organization");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("Organization");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contactNumber");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Detail)
                .IsUnicode(false)
                .HasColumnName("detail");
            entity.Property(e => e.Email)
                .HasColumnType("text")
                .HasColumnName("email");
            entity.Property(e => e.Image)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.IsParent).HasColumnName("isParent");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ParentId).HasColumnName("parentId");
            entity.Property(e => e.PaymentMethods)
                .IsUnicode(false)
                .HasColumnName("paymentMethods");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.Zip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("zip");
        });

        modelBuilder.Entity<TelemetryDatum>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Datatype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("datatype");
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.DeviceId).HasColumnName("deviceId");
            entity.Property(e => e.VariableName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("variableName");

            entity.HasOne(d => d.Device).WithMany(p => p.TelemetryData)
                .HasForeignKey(d => d.DeviceId)
                .HasConstraintName("FK_TelemetryData_Device");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
