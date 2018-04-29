using System;
namespace MusicStoreDemo.Common
{
    public class MusicStoreConstants
	{
		public class Roles
		{ 
			public const string AdminUser = "AdminUser";
			public const string TestUser = "TestUser";
		}
		public class CLaimTypes
		{
			public const string MusicStoreApiWriteAccess = "musicStoreApi.writeAccess";
			public const string MusicStoreApiReadAccess = "musicStoreApi.readAccess";
			public const string MusicStoreApiAgeDemographic = "musicStoreApi.userAgeDemographic";
		}
    }
}
