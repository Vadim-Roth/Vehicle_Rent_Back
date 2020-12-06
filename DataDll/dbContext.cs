using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RedWheels
{
    public partial class dbContext : DbContext
    {
        public dbContext()
        {
        }

        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<OrderList> OrderList { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VehicleTypes> VehicleTypes { get; set; }
        public virtual DbSet<Vehicles> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\koman\\source\\repos\\RedWheels\\RedWheels\\RedWheels.mdf;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branches>(entity =>
            {
                entity.HasKey(e => e.BranchId)
                    .HasName("PK__Branches__751EBD5F9B3C730A");

                entity.Property(e => e.BranchId).HasColumnName("branchId");

                entity.Property(e => e.BranchName)
                    .IsRequired()
                    .HasColumnName("branchName")
                    .HasMaxLength(50);

                entity.Property(e => e.ExactLocation)
                    .IsRequired()
                    .HasColumnName("exactLocation")
                    .HasMaxLength(50);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("location")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<OrderList>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__OrderLis__0809335D30B9498A");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.CustId).HasColumnName("custId");

                entity.Property(e => e.EmpRegister).HasColumnName("empRegister");

                entity.Property(e => e.OrderEnd)
                    .IsRequired()
                    .HasColumnName("orderEnd")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderRealEnd)
                    .HasColumnName("orderRealEnd")
                    .HasMaxLength(50);

                entity.Property(e => e.OrderStart)
                    .IsRequired()
                    .HasColumnName("orderStart")
                    .HasMaxLength(50);

                entity.Property(e => e.VehicleNumber).HasColumnName("vehicleNumber");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__Users__CB9A1CFF83F14ED1");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.UserBday)
                    .HasColumnName("userBDay")
                    .HasMaxLength(50);

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasColumnName("userEmail")
                    .HasMaxLength(50);

                entity.Property(e => e.UserGender)
                    .IsRequired()
                    .HasColumnName("userGender")
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("userName")
                    .HasMaxLength(50);

                entity.Property(e => e.UserNick)
                    .IsRequired()
                    .HasColumnName("userNick")
                    .HasMaxLength(50);

                entity.Property(e => e.UserPass)
                    .IsRequired()
                    .HasColumnName("userPass")
                    .HasMaxLength(50);

                entity.Property(e => e.UserPicture)
                    .HasColumnName("userPicture")
                    .HasMaxLength(50);

                entity.Property(e => e.UserRole)
                    .IsRequired()
                    .HasColumnName("userRole")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VehicleTypes>(entity =>
            {
                entity.HasKey(e => e.ModelId);

                entity.Property(e => e.ModelId).HasColumnName("modelId");

                entity.Property(e => e.DailyCost).HasColumnName("dailyCost");

                entity.Property(e => e.DailyDelay).HasColumnName("dailyDelay");

                entity.Property(e => e.Gear)
                    .IsRequired()
                    .HasColumnName("gear")
                    .HasMaxLength(50);

                entity.Property(e => e.Manufacturer)
                    .IsRequired()
                    .HasColumnName("manufacturer")
                    .HasMaxLength(50);

                entity.Property(e => e.ModelName)
                    .IsRequired()
                    .HasColumnName("modelName")
                    .HasMaxLength(50);

                entity.Property(e => e.ProdYear).HasColumnName("prodYear");
            });

            modelBuilder.Entity<Vehicles>(entity =>
            {
                entity.HasKey(e => e.VehicleNumber);

                entity.Property(e => e.VehicleNumber)
                    .HasColumnName("vehicleNumber")
                    .ValueGeneratedNever();

                entity.Property(e => e.BranchId).HasColumnName("branchID");

                entity.Property(e => e.CurrentKilos).HasColumnName("currentKilos");

                entity.Property(e => e.IsAvailable).HasColumnName("isAvailable");

                entity.Property(e => e.IsFunctional).HasColumnName("isFunctional");

                entity.Property(e => e.ModelId).HasColumnName("modelId");

                entity.Property(e => e.VehiclePicture)
                    .HasColumnName("vehiclePicture")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
