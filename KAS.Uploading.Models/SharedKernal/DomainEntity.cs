using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace KAS.Uploading.Models.SharedKernal
{
	public abstract class DomainEntity<T>
	{
		[Key]
		public T ID { get; set; }

		///// <summary>
		///// True if domain entity has an identity
		///// </summary>
		///// <returns></returns>
		//public bool IsTransient()
		//{
		//	return Id.Equals(default(T));
		//}
	}
}
