using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                  new Product { Id = 1, Name = "Kalem 1", CategoryId = 1, Price = 20, CreatedDate = DateTime.Now },
                  new Product { Id = 2, Name = "Kalem 2", CategoryId = 1, Price = 25, CreatedDate = DateTime.Now },
                  new Product { Id = 3, Name = "Kalem 3", CategoryId = 1, Price = 30, CreatedDate = DateTime.Now },
                  new Product { Id = 4, Name = "Kalem 4", CategoryId = 1, Price = 35, CreatedDate = DateTime.Now },
                  new Product { Id = 5, Name = "Kitap 1", CategoryId = 2, Price = 55, CreatedDate = DateTime.Now },
                  new Product { Id = 6, Name = "Kitap 2", CategoryId = 2, Price = 65, CreatedDate = DateTime.Now }
                );
        }
    }
}
