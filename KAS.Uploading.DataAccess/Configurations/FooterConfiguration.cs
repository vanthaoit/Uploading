using KAS.Uploading.DataAccess.Extensions;
using KAS.Uploading.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.DataAccess.Configurations
{
	public class FooterConfiguration : DbEntityConfiguration<Footer>
	{
		public override void Configure(EntityTypeBuilder<Footer> entity)
		{
			entity.HasKey(c => c.ID);
			entity.Property(c => c.ID).HasMaxLength(255)
				.HasColumnType("varchar(255)").IsRequired();
			// etc.
		}
	}
}
