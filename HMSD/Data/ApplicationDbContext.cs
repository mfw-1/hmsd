using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HMSD.Data
{
    // =========================
    // BASE ENTITY (COMMON FIELDS)
    // =========================
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }

    // =========================
    // ENUMS
    // =========================
    public enum RoomStatus { Available, Occupied, Maintenance }
    public enum BedStatus { Available, Occupied, Reserved }
    public enum AllocationStatus { Active, Completed, Cancelled }
    public enum ComplaintStatus { Pending, InProgress, Resolved }
    public enum FeeStatus { Pending, Paid, Partial }
    public enum StaffStatus { Active, Inactive }

    // =========================
    // APPLICATION USER
    // =========================
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    // =========================
    // HOSTEL
    // =========================
    public class Hostel : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public int TotalRooms { get; set; }

        [Required, MaxLength(20)]
        public string GenderType { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Floor>? Floors { get; set; }
        public ICollection<Room>? Rooms { get; set; }
    }

    // =========================
    // FLOOR
    // =========================
    public class Floor : BaseEntity
    {
        [Required]
        public int HostelId { get; set; }
        public Hostel Hostel { get; set; } = null!;

        [Required]
        public int FloorNumber { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Room>? Rooms { get; set; }
    }

    // =========================
    // ROOM TYPE
    // =========================
    public class RoomType : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Capacity { get; set; }

        [Required]
        public decimal MonthlyRent { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Room>? Rooms { get; set; }
    }

    // =========================
    // ROOM
    // =========================
    public class Room : BaseEntity
    {
        [Required]
        public int HostelId { get; set; }
        public Hostel Hostel { get; set; } = null!;

        [Required]
        public int FloorId { get; set; }
        public Floor Floor { get; set; } = null!;

        [Required]
        public int RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;

        [Required, MaxLength(20)]
        public string RoomNumber { get; set; } = string.Empty;

        [Required]
        public int Capacity { get; set; }

        public int CurrentOccupancy { get; set; } = 0;

        [Required]
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Bed>? Beds { get; set; }
    }

    // =========================
    // BED
    // =========================
    public class Bed : BaseEntity
    {
        [Required]
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        [Required, MaxLength(20)]
        public string BedNumber { get; set; } = string.Empty;

        [Required]
        public BedStatus Status { get; set; } = BedStatus.Available;

        public ICollection<Allocation>? Allocations { get; set; }
    }

    // =========================
    // STUDENT
    // =========================
    public class Student : BaseEntity
    {
        [Required, MaxLength(20)]
        public string RegistrationNo { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, MaxLength(15)]
        public string CNIC { get; set; } = string.Empty;

        [Required, MaxLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string City { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string GuardianName { get; set; } = string.Empty;

        [Required, Phone]
        public string GuardianPhone { get; set; } = string.Empty;

        [Phone]
        public string EmergencyContact { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string InstituteName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Department { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Semester { get; set; } = string.Empty;

        public ICollection<Allocation>? Allocations { get; set; }
        public ICollection<StudentFee>? StudentFees { get; set; }
        public ICollection<Complaint>? Complaints { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
        public ICollection<Visitor>? Visitors { get; set; }
    }

    // =========================
    // ALLOCATION
    // =========================
    public class Allocation : BaseEntity
    {
        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        [Required]
        public int BedId { get; set; }
        public Bed Bed { get; set; } = null!;

        [Required]
        public DateTime AllocatedDate { get; set; }

        public DateTime? ExpectedCheckoutDate { get; set; }
        public DateTime? ActualCheckoutDate { get; set; }

        [Required]
        public AllocationStatus Status { get; set; } = AllocationStatus.Active;

        [MaxLength(500)]
        public string? Remarks { get; set; }
    }

    // =========================
    // FEE TYPE
    // =========================
    public class FeeType : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Amount { get; set; }

        public ICollection<StudentFee>? StudentFees { get; set; }
    }

    // =========================
    // STUDENT FEE
    // =========================
    public class StudentFee : BaseEntity
    {
        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        [Required]
        public int FeeTypeId { get; set; }
        public FeeType FeeType { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public FeeStatus Status { get; set; } = FeeStatus.Pending;

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        public ICollection<Payment>? Payments { get; set; }
    }

    // =========================
    // PAYMENT
    // =========================
    public class Payment : BaseEntity
    {
        [Required]
        public int StudentFeeId { get; set; }
        public StudentFee StudentFee { get; set; } = null!;

        [Required]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required, MaxLength(30)]
        public string PaymentMethod { get; set; } = string.Empty;

        public string? TransactionId { get; set; }
        public string? ReceivedBy { get; set; }
    }

    // =========================
    // COMPLAINT
    // =========================
    public class Complaint : BaseEntity
    {
        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;
    }

    // =========================
    // ATTENDANCE
    // =========================
    public class Attendance : BaseEntity
    {
        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Present";
    }

    // =========================
    // STAFF
    // =========================
    public class Staff : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Role { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [Required]
        public StaffStatus Status { get; set; } = StaffStatus.Active;
    }

    // =========================
    // VISITOR
    // =========================
    public class Visitor : BaseEntity
    {
        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        [Required, MaxLength(100)]
        public string VisitorName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Relation { get; set; } = string.Empty;

        [Required, Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public DateTime CheckInTime { get; set; }

        public DateTime? CheckOutTime { get; set; }

        [Required, MaxLength(200)]
        public string Purpose { get; set; } = string.Empty;
    }

    // =========================
    // NOTICE
    // =========================
    public class Notice : BaseEntity
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string PostedBy { get; set; } = string.Empty;

        public DateTime? ExpiryDate { get; set; }
    }

    // =========================
    // DB CONTEXT
    // =========================
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Hostel> Hostels => Set<Hostel>();
        public DbSet<Floor> Floors => Set<Floor>();
        public DbSet<RoomType> RoomTypes => Set<RoomType>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Bed> Beds => Set<Bed>();

        public DbSet<Student> Students => Set<Student>();
        public DbSet<Allocation> Allocations => Set<Allocation>();

        public DbSet<FeeType> FeeTypes => Set<FeeType>();
        public DbSet<StudentFee> StudentFees => Set<StudentFee>();
        public DbSet<Payment> Payments => Set<Payment>();

        public DbSet<Complaint> Complaints => Set<Complaint>();
        public DbSet<Attendance> Attendances => Set<Attendance>();

        public DbSet<Staff> Staffs => Set<Staff>();
        public DbSet<Visitor> Visitors => Set<Visitor>();
        public DbSet<Notice> Notices => Set<Notice>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // =========================
            // UNIQUE INDEXES
            // =========================

            builder.Entity<Student>()
                .HasIndex(s => s.CNIC)
                .IsUnique();

            builder.Entity<Student>()
                .HasIndex(s => s.RegistrationNo)
                .IsUnique();

            builder.Entity<Room>()
                .HasIndex(r => new { r.HostelId, r.RoomNumber })
                .IsUnique();

            // =========================
            // FIX MULTIPLE CASCADE PATH
            // =========================

            builder.Entity<Room>()
                .HasOne(r => r.Hostel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HostelId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // DECIMAL PRECISION
            // =========================

            builder.Entity<FeeType>()
                .Property(f => f.Amount)
                .HasPrecision(18, 2);

            builder.Entity<StudentFee>()
                .Property(f => f.Amount)
                .HasPrecision(18, 2);

            builder.Entity<Payment>()
                .Property(p => p.AmountPaid)
                .HasPrecision(18, 2);

            builder.Entity<RoomType>()
                .Property(r => r.MonthlyRent)
                .HasPrecision(18, 2);

            builder.Entity<Staff>()
                .Property(s => s.Salary)
                .HasPrecision(18, 2);
        }
    }
}