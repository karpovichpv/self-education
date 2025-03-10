﻿using PeopleViewer.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeopleViewer.Presentation.Tests
{
	internal class FakeReader : IPersonReader
	{
		List<Person> testData = new List<Person>()
		{
			new Person() {Id = 1,
				GivenName = "John", FamilyName = "Smith",
				Rating = 7, StartDate = new DateTime(2000, 10, 1)},
			new Person() {Id = 2,
				GivenName = "Mary", FamilyName = "Thomas",
				Rating = 9, StartDate = new DateTime(1971, 7, 23)},
		};

		public IEnumerable<Person> GetPeople()
		{
			return testData;
		}

		public Person GetPerson(int id)
		{
			return testData.FirstOrDefault(p => p.Id == id);
		}
	}
}
