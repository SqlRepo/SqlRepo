using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Abstractions;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class EntityMappingProfileMapShould
    {
        private const string ChildIdColumnName = "Child_Id";
        private const string ChildNameColumnName = "Child_Name";
        private const string ChildGenderColumnName = "Child_Gender";
        private const string ChildDateOfBirthColumnName = "Child_DateOfBirth";
        private const string IdColumnName = "Id";
        private const string NameColumnName = "Name";
        private const string GenderColumnName = "Gender";
        private const string dateOfBirthColumnName = "DateOfBirth";
        private const string AddressLine1ColumnName = "Address_Line1";
        private const string AddressLine2ColumnName = "Address_Line2";
        private const string AddressTownColumnName = "Address_Town";
        private const string AddressRegionColumnName = "Address_Region";
        private const string AddressPostCodeColumnName = "Address_PostCode";
        private const string LocationLatitudeColumnName = "Location_Latitude";
        private const string LocationLongitudeColumnName = "Location_Longitude";
        private const string LocationTownColumnName = "Location_Town";

        [SetUp]
        public void SetUp()
        {
            this.target = new EntityMappingProfile<Person>();
        }

        [Test]
        public void MapLocationCorrectlyFromProfile()
        {
            var dataRecord = DataRecordMockBuilder.CreateNew()
                                                  .WithIntColumn(IdColumnName, 0, 1)
                                                  .WithStringColumn(NameColumnName, 1, "name")
                                                  .WithIntColumn(GenderColumnName, 2, (int)Gender.Male)
                                                  .WithDateTimeColumn(dateOfBirthColumnName,
                                                      3,
                                                      new DateTime(1956, 02, 10))
                                                  .WithDoubleColumn(LocationLatitudeColumnName, 4, 0.5)
                                                  .WithDoubleColumn(LocationLongitudeColumnName, 5, 1.2)
                                                  .WithStringColumn(LocationTownColumnName, 5, "town")
                                                  .Build();
            var person = new Person();

            var locationMappingProfile = new EntityMappingProfile<Location>();
            locationMappingProfile
                .ForMember(e => e.Latitude, c => c.MapFromColumnName(LocationLatitudeColumnName, true))
                .ForMember(e => e.Longitude, c => c.MapFromColumnName(LocationLongitudeColumnName, true))
                .ForMember(e => e.Town, c => c.MapFromColumnName(LocationTownColumnName));

            this.target.ForMember(e => e.Id, c => c.MapFromColumnName(IdColumnName, true))
                .ForMember(e => e.Name, c => c.MapFromColumnName(NameColumnName))
                .ForMember(e => e.Gender, c => c.MapFromColumnName(GenderColumnName))
                .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName(dateOfBirthColumnName))
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
            var locationColumnNames = new[] {LocationLatitudeColumnName, LocationLongitudeColumnName, LocationTownColumnName};

            var dataRecord = DataRecordMockBuilder.CreateNew()
                                                  .WithIntColumn(IdColumnName, 0, 1)
                                                  .WithStringColumn(NameColumnName, 1, "name")
                                                  .WithIntColumn(GenderColumnName, 2, (int)Gender.Male)
                                                  .WithDateTimeColumn(dateOfBirthColumnName,
                                                      3,
                                                      new DateTime(1956, 02, 10))
                                                  .WithDoubleColumn(LocationLatitudeColumnName, 4, 0.5)
                                                  .WithDoubleColumn(LocationLongitudeColumnName, 5, 1.2)
                                                  .WithStringColumn(LocationTownColumnName, 5, "town")
                                                  .Build();
            var person = new Person();

            this.target.ForMember(e => e.Id, c => c.MapFromColumnName(IdColumnName, true))
                .ForMember(e => e.Name, c => c.MapFromColumnName(NameColumnName))
                .ForMember(e => e.Gender, c => c.MapFromColumnName(GenderColumnName))
                .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName(dateOfBirthColumnName))
                .ForMember(e => e.Location,
                    (p) => p.ForMember(e => e.Latitude, c => c.MapFromColumnName(LocationLatitudeColumnName, true))
                            .ForMember(e => e.Longitude, c => c.MapFromColumnName(LocationLongitudeColumnName, true))
                            .ForMember(e => e.Town, c => c.MapFromColumnName(LocationTownColumnName)))
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
                                                  .WithIntColumn(IdColumnName, 0, 1)
                                                  .WithStringColumn(NameColumnName, 1, "name")
                                                  .WithIntColumn(GenderColumnName, 2, (int)Gender.Male)
                                                  .WithDateTimeColumn(dateOfBirthColumnName,
                                                      3,
                                                      new DateTime(1956, 02, 10))
                                                  .WithIntColumn(ChildIdColumnName, 6, 2)
                                                  .WithStringColumn(ChildNameColumnName, 7, "child")
                                                  .WithIntColumn(ChildGenderColumnName, 8, (int)Gender.Female)
                                                  .WithDateTimeColumn(ChildDateOfBirthColumnName,
                                                      9,
                                                      new DateTime(1986, 11, 05))
                                                  .Build();
            var person = new Person();

            var childMappingProfile = new EntityMappingProfile<Person>();
            childMappingProfile.ForMember(e => e.Id, c => c.MapFromColumnName(ChildIdColumnName, true))
                               .ForMember(e => e.Name, c => c.MapFromColumnName(ChildNameColumnName))
                               .ForMember(e => e.Gender, c => c.MapFromColumnName(ChildGenderColumnName))
                               .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName(ChildDateOfBirthColumnName));

            this.target.ForMember(e => e.Id, c => c.MapFromColumnName(IdColumnName, true))
                .ForMember(e => e.Name, c => c.MapFromColumnName(NameColumnName))
                .ForMember(e => e.Gender, c => c.MapFromColumnName(GenderColumnName))
                .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName(dateOfBirthColumnName))
                .ForGenericCollectionMember<List<Person>, Person>(e => e.Children, childMappingProfile)
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

        [Test]
        public void MapChildrenCorrectlyFromProfileConfig()
        {
            var dataRecord = DataRecordMockBuilder.CreateNew()
                                                  .WithIntColumn(IdColumnName, 0, 1)
                                                  .WithStringColumn(NameColumnName, 1, "name")
                                                  .WithIntColumn(GenderColumnName, 2, (int)Gender.Male)
                                                  .WithDateTimeColumn(dateOfBirthColumnName,
                                                      3,
                                                      new DateTime(1956, 02, 10))
                                                  .WithIntColumn(ChildIdColumnName, 6, 2)
                                                  .WithStringColumn(ChildNameColumnName, 7, "child")
                                                  .WithIntColumn(ChildGenderColumnName, 8, (int)Gender.Female)
                                                  .WithDateTimeColumn(ChildDateOfBirthColumnName,
                                                      9,
                                                      new DateTime(1986, 11, 05))
                                                  .Build();
            var person = new Person();

            this.target.ForMember(e => e.Id, c => c.MapFromColumnName(IdColumnName, true))
                .ForMember(e => e.Name, c => c.MapFromColumnName(NameColumnName))
                .ForMember(e => e.Gender, c => c.MapFromColumnName(GenderColumnName))
                .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName(dateOfBirthColumnName))
                .ForGenericCollectionMember<List<Person>, Person>(e => e.Children,
                    cc => cc.ForMember(e => e.Id, c => c.MapFromColumnName(ChildIdColumnName, true))
                            .ForMember(e => e.Name, c => c.MapFromColumnName(ChildNameColumnName))
                            .ForMember(e => e.Gender, c => c.MapFromColumnName(ChildGenderColumnName))
                            .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName(ChildDateOfBirthColumnName)))
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

        [Test]
        public void MapChildrenAndAddressesCorrectlyFromProfiles()
        {
            var dataRecord = DataRecordMockBuilder.CreateNew()
                                                  .WithIntColumn(IdColumnName, 0, 1)
                                                  .WithStringColumn(NameColumnName, 1, "name")
                                                  .WithIntColumn(GenderColumnName, 2, (int)Gender.Male)
                                                  .WithDateTimeColumn(dateOfBirthColumnName,
                                                      3,
                                                      new DateTime(1956, 02, 10))
                                                  .WithIntColumn(ChildIdColumnName, 6, 2)
                                                  .WithStringColumn(ChildNameColumnName, 7, "child")
                                                  .WithIntColumn(ChildGenderColumnName, 8, (int)Gender.Female)
                                                  .WithDateTimeColumn(ChildDateOfBirthColumnName,
                                                      9,
                                                      new DateTime(1986, 11, 05))
                                                  .WithStringColumn(AddressLine1ColumnName, 10, "line 1")
                                                  .WithStringColumn(AddressLine2ColumnName, 11, "line 2")
                                                  .WithStringColumn(AddressTownColumnName, 12, "town")
                                                  .WithStringColumn(AddressRegionColumnName, 13, "region")
                                                  .WithStringColumn(AddressPostCodeColumnName, 14, "postal code")
                                                  .Build();
            var person = new Person();

            var childMappingProfile = new EntityMappingProfile<Person>();
            childMappingProfile.ForMember(e => e.Id, c => c.MapFromColumnName(ChildIdColumnName, true))
                               .ForMember(e => e.Name, c => c.MapFromColumnName(ChildNameColumnName))
                               .ForMember(e => e.Gender, c => c.MapFromColumnName(ChildGenderColumnName))
                               .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName(ChildDateOfBirthColumnName));

            var addressMappingProfile = new EntityMappingProfile<Address>();
            addressMappingProfile.ForMember(e => e.Line1, c => c.MapFromColumnName(AddressLine1ColumnName, true))
                                 .ForMember(e => e.Line2, c => c.MapFromColumnName(AddressLine2ColumnName))
                                 .ForMember(e => e.Town, c => c.MapFromColumnName(AddressTownColumnName))
                                 .ForMember(e => e.Region,
                                     c => c.MapFromColumnName(AddressRegionColumnName))
                                 .ForMember(e => e.PostCode, c => c.MapFromColumnName(AddressPostCodeColumnName));

            this.target.ForMember(e => e.Id, c => c.MapFromColumnName(IdColumnName, true))
                .ForMember(e => e.Name, c => c.MapFromColumnName(NameColumnName))
                .ForMember(e => e.Gender, c => c.MapFromColumnName(GenderColumnName))
                .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName(dateOfBirthColumnName))
                .ForGenericCollectionMember<List<Person>, Person>(e => e.Children, childMappingProfile)
                .ForArrayMember(e => e.Addresses, addressMappingProfile)
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
            var firstChild = person.Children.First();
            firstChild.Id.Should()
                      .Be(2);
            firstChild.Name.Should()
                      .Be("child");
            firstChild.Gender.Should()
                      .Be(Gender.Female);
            firstChild.DateOfBirth.Should()
                      .BeSameDateAs(new DateTime(1986, 11, 05));

            person.Addresses.Should()
                  .NotBeNullOrEmpty();
            var firstAddress = person.Addresses.First();
            firstAddress.Line1.Should()
                        .Be("line 1");
            firstAddress.Line2.Should()
                        .Be("line 2");
            firstAddress.Town.Should()
                        .Be("town");
            firstAddress.Region.Should()
                        .Be("region");
            firstAddress.PostCode.Should()
                        .Be("postal code");

        }

        private IEntityMappingProfile<Person> target;
    }
}