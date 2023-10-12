using System;
using Bogus;

namespace SauceDemoCSTests.Utilities
{
	public class FactoryData
	{
        private static Faker faker = new Faker();

		public static String GetRandomFirstName()
		{
            return faker.Name.FirstName();
        }

		public static String GetRandomLastName()
		{
			return faker.Name.LastName();
        }

        public static String GetRandomPostalCode()
        {
            return faker.Address.ZipCode().Substring(0, 5);
        }

    }
}

