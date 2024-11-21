using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IIMSv1.Models;
using static System.Formats.Asn1.AsnWriter;

namespace IIMSv1.Data
{
    public class DataSeed
    {
        public static UserManager<AccountUser> SeedUserManager;
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var _context = new InventoryDbContext(serviceProvider.GetRequiredService<DbContextOptions<InventoryDbContext>>());
            SeedUserManager = serviceProvider.GetService<UserManager<AccountUser>>();

            var Supply = new[] {
                "Office Supply",
                "Janitorial Supply",
                "Electrical Supply",
                "IT Equipment" };

            var Unit = new[] {
                "PACK",
                "JAR",
                "GAL",
                "REAM",
                "BOX",
                "TUBE",
                "PC",
                "ROLL",
                "PAD",
                "SET",
                "PAIR",
                "CAN",
                "BAR",};

            var SpecsType = new[] {
                "THICKNESS",
                "SIZE",
                "CAPACITY",
                "COLOR",
                "PACKAGING",
                "TYPE",
                "MODEL",
                "OTHERS",
                "BRAND",
                };
            
            DepartmentCluster DepartmentCluster = new DepartmentCluster()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Default Cluster",
                NormalizedName = "DEFAULT CLUSTER",

            };
            Department Department = new Department()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Default Department",
                NormalizedName = "DEFAULT DEPARTMENT".Trim().ToUpper(),
                DepartmentClusterID = DepartmentCluster.Id,
                IsActive = true,
            };
            var date = DateOnly.FromDateTime(DateTime.Now);
            var time = TimeOnly.FromDateTime(DateTime.Now);
            Employee employee = new Employee()
            {
                Id = Guid.NewGuid().ToString(),
                lastName = "System",
                firstName = "Admin",
                middleName = "",
                departmentID = Department.Id,
                nameExt = "",
                Designation = "SUPER ADMIN",
                displayName = "System, Admin",
                DateCreated = date,
                DateUpdated = date,
                TimeCreated = time,
                TimeUpdated = time,
            };
            AccountUser newuser = new AccountUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserID = employee.Id,
                UserName = "Admin",
                Email = "systemadmin@example.com",
                NormalizedEmail = "SYSTEMADMIN@EXAMPLE.COM",
                NormalizedUserName = "SYSTEMADMIN",
                IsActive = true,
                IsAdmin = true,
            };

            if (!_context.Employees.Any())
            {
                _context.Employees.Add(employee);
                _context.Departments.Add(Department);
                _context.departmentClusters.Add(DepartmentCluster);
                _context.SaveChanges();

                var result1 = SeedUserManager.CreateAsync(newuser, "@Randomber16").Result;
                var result2 = SeedUserManager.AddToRoleAsync(newuser, "System Administrator").Result;
            }

            foreach (var supp in Supply)
            {
                Supplies? esupp = await _context.Supplies
                    .SingleOrDefaultAsync(x => x.supplyName.Equals(supp));

                if (esupp == null)
                {
                    Supplies supply = new Supplies()
                    {
                        Id = Guid.NewGuid().ToString(),
                        supplyName = supp.ToString(),
                        IsEnabled = true,
                    };
                    _context.AddAsync(supply);
                    _context.SaveChanges();
                }

            }

            foreach (var un in Unit)
            {
                ItemUnit? eun = await _context.ItemUnits
                    .SingleOrDefaultAsync(x => x.UnitName.Equals(un));

                if (eun == null)
                {
                    ItemUnit NewUnit = new ItemUnit()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UnitName = un.ToString(),
                        IsEnabled = true,
                    };
                    _context.AddAsync(NewUnit);
                    _context.SaveChanges();
                }
            }

            foreach (var spec in SpecsType)
            {
                ItemSpecType? espec = await _context.ItemSpecType
                    .SingleOrDefaultAsync(x => x.itemSpecType.Equals(spec));

                if (espec == null)
                {
                    ItemSpecType newItemSpecType = new ItemSpecType()
                    {
                        Id = Guid.NewGuid().ToString(),
                        itemSpecType = spec.ToString(),
                        IsEnabled = true,
                    };
                    _context.AddAsync(newItemSpecType);
                    _context.SaveChanges();
                }
            }

        }

    }
}
