using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMappingProfileMapShould
    {
        [SetUp]
        public void SetUp()
        {
            this.target = new EntityMappingProfile<Person>();
        }

        [Test]
        public void MapLocationCorrectlyFromProfile()
        {
            var locationColumnNames = new[] {"Location_Latitude", "Location_Longitude", "Location_Town"};

            var dataRecord = DataRecordMockBuilder.CreateNew()
                                                  .WithIntColumn("Id", 0, 1)
                                                  .WithStringColumn("Name", 1, "name")
                                                  .WithIntColumn("Gender", 2, (int)Gender.Male)
                                                  .WithDateTimeColumn("DateOfBirth",
                                                      3,
                                                      new DateTime(1956, 02, 10))
                                                  .WithDoubleColumn(locationColumnNames[0], 4, 0.5)
                                                  .WithDoubleColumn(locationColumnNames[1], 5, 1.2)
                                                  .WithStringColumn(locationColumnNames[2], 5, "town")
                                                  .Build();
            var person = new Person();

            var locationMappingProfile = new EntityMappingProfile<Location>();
            locationMappingProfile
                .ForMember(e => e.Latitude, c => c.MapFromColumnName(locationColumnNames[0]))
                .ForMember(e => e.Longitude, c => c.MapFromColumnName(locationColumnNames[1]))
                .ForMember(e => e.Town, c => c.MapFromColumnName(locationColumnNames[2]));

            this.target.ForMember(e => e.Id, c => c.MapFromColumnName("Id"))
                .ForMember(e => e.Name, c => c.MapFromColumnName("Name"))
                .ForMember(e => e.Gender, c => c.MapFromColumnName("Gender"))
                .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName("DateOfBirth"))
                .ForMember(e => e.Location, locationMappingProfile)
                .Map(person, dataRecord);

            person.Id.Should()
                  .Be(1);
            person.Name.Should()
                  .Be("name");
            person.Gender.Should()
                  .Be(Gender.Male);
            person.DateOfBirth.Should()
                  .BeSameDateAs(new DateTime(1956, 02, 10));

            person.Location.Should()
                  .NotBeNull();
            person.Location.Latitude.Should()
                  .Be(0.5);
            person.Location.Longitude.Should()
                  .Be(1.2);
            person.Location.Town.Should()
                  .Be("town");
        }

        [Test]
        public void MapLocationCorrectlyFromProfileConfig()
        {
            var locationColumnNames = new[] {"Location_Latitude", "Location_Longitude", "Location_Town"};

            var dataRecord = DataRecordMockBuilder.CreateNew()
                                                  .WithIntColumn("Id", 0, 1)
                                                  .WithStringColumn("Name", 1, "name")
                                                  .WithIntColumn("Gender", 2, (int)Gender.Male)
                                                  .WithDateTimeColumn("DateOfBirth",
                                                      3,
                                                      new DateTime(1956, 02, 10))
                                                  .WithDoubleColumn(locationColumnNames[0], 4, 0.5)
                                                  .WithDoubleColumn(locationColumnNames[1], 5, 1.2)
                                                  .WithStringColumn(locationColumnNames[2], 5, "town")
                                                  .Build();
            var person = new Person();

            this.target.ForMember(e => e.Id, c => c.MapFromColumnName("Id"))
                .ForMember(e => e.Name, c => c.MapFromColumnName("Name"))
                .ForMember(e => e.Gender, c => c.MapFromColumnName("Gender"))
                .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName("DateOfBirth"))
                .ForMember(e => e.Location,
                    (p) => p.ForMember(e => e.Latitude, c => c.MapFromColumnName(locationColumnNames[0]))
                            .ForMember(e => e.Longitude, c => c.MapFromColumnName(locationColumnNames[1]))
                            .ForMember(e => e.Town, c => c.MapFromColumnName(locationColumnNames[2])))
                .Map(person, dataRecord);

            person.Id.Should()
                  .Be(1);
            person.Name.Should()
                  .Be("name");
            person.Gender.Should()
                  .Be(Gender.Male);
            person.DateOfBirth.Should()
                  .BeSameDateAs(new DateTime(1956, 02, 10));

            person.Location.Should()
                  .NotBeNull();
            person.Location.Latitude.Should()
                  .Be(0.5);
            person.Location.Longitude.Should()
                  .Be(1.2);
            person.Location.Town.Should()
                  .Be("town");
        }

        [Test]
        public void MapChildrenCorrectlyFromProfile()
        {

            var dataRecord = DataRecordMockBuilder.CreateNew()
                                                  .WithIntColumn("Id", 0, 1)
                                                  .WithStringColumn("Name", 1, "name")
                                                  .WithIntColumn("Gender", 2, (int)Gender.Male)
                                                  .WithDateTimeColumn("DateOfBirth",
                                                      3,
                                                      new DateTime(1956, 02, 10))
                                                  .WithIntColumn("Child_Id", 6, 2)
                                                  .WithStringColumn("Child_Name", 7, "child")
                                                  .WithIntColumn("Child_Gender", 8, (int)Gender.Female)
                                                  .WithDateTimeColumn("Child_DateOfBirth", 9, new DateTime(1986, 11, 05))
                                                  .Build();
            var person = new Person();

           var childMappingProfile = new EntityMappingProfile<Person>();
            childMappingProfile.ForMember(e => e.Id, c => c.MapFromColumnName("Child_Id"))
                               .ForMember(e => e.Name, c => c.MapFromColumnName("Child_Name"))
                               .ForMember(e => e.Gender, c => c.MapFromColumnName("Child_Gender"))
                               .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName("Child_DateOfBirth"));

            this.target.ForMember(e => e.Id, c => c.MapFromColumnName("Id"))
                .ForMember(e => e.Name, c => c.MapFromColumnName("Name"))
                .ForMember(e => e.Gender, c => c.MapFromColumnName("Gender"))
                .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName("DateOfBirth"))
                .ForEnumerableMember<List<Person>, Person>(e => e.Children, childMappingProfile)
                .Map(person, dataRecord);

            person.Id.Should()
                  .Be(1);
            person.Name.Should()
                  .Be("name");
            person.Gender.Should()
                  .Be(Gender.Male);
            person.DateOfBirth.Should()
                  .BeSameDateAs(new DateTime(1956, 02, 10));

            person.Children.Should()
                  .NotBeNullOrEmpty();
        }

        private IEntityMappingProfile<Person> target;
    }
}