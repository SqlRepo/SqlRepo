using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SqlRepo.Tests.TestObjects;

namespace SqlRepo.Tests
{
    [TestFixture]
    public class DataReaderEntityMapperShould
    {
        [SetUp]
        public void Setup()
        {
            this.target = new DataReaderEntityMapper();
        }

        [Test]
        public void MapCorrectlyWhenEachRowIsEntityWithMatchingMemberNames()
        {
            var dateOfBirth = DateTime.Today;
            var dataReader = DataReaderMockBuilder.CreateNew(2)
                                                  .WithIntColumn("Id",
                                                                 0,
                                                                 1,
                                                                 2)
                                                  .WithStringColumn("Name",
                                                                    1,
                                                                    "Male",
                                                                    "Female")
                                                  .WithIntColumn("Gender",
                                                                 2,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("DateOfBirth",
                                                                      3,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .Build();
            var actual = this.target.Map<Person>(dataReader)
                             .ToList();
            actual.Should()
                  .NotBeNullOrEmpty();
            actual.Count.Should()
                  .Be(2);

            var first = actual.First();
            first.Id.Should()
                 .Be(1);
            first.Name.Should()
                 .Be("Male");
            first.Gender.Should()
                 .Be(Gender.Male);
            first.DateOfBirth.Should()
                 .Be(dateOfBirth);

            var last = actual.Last();
            last.Id.Should()
                .Be(2);
            last.Name.Should()
                .Be("Female");
            last.Gender.Should()
                .Be(Gender.Female);
            last.DateOfBirth.Should()
                .Be(dateOfBirth);
        }

        [Test]
        public void MapMultipleEntitiesWithSingleChildCorrectlyUsingProfile()
        {
            var dateOfBirth = DateTime.Today;
            var dataReader = DataReaderMockBuilder.CreateNew(2)
                                                  .WithIntColumn("Id",
                                                                 0,
                                                                 1,
                                                                 2)
                                                  .WithStringColumn("Name",
                                                                    1,
                                                                    "Male",
                                                                    "Female")
                                                  .WithIntColumn("Gender",
                                                                 2,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("DateOfBirth",
                                                                      3,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .WithIntColumn("Child_Id",
                                                                 4,
                                                                 3,
                                                                 4)
                                                  .WithStringColumn("Child_Name",
                                                                    5,
                                                                    "Boy",
                                                                    "Girl")
                                                  .WithIntColumn("Child_Gender",
                                                                 6,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("Child_DateOfBirth",
                                                                      7,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .Build();

            var mappingProfile = new EntityMappingProfile<Person>();
            mappingProfile.ForMember(e => e.Id, c => c.MapFromColumnName("Id", true))
                          .ForMember(e => e.Name, c => c.MapFromColumnName("Name"))
                          .ForMember(e => e.Gender, c => c.MapFromColumnName("Gender"))
                          .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName("DateOfBirth"))
                          .ForGenericCollectionMember<List<Person>, Person>(e => e.Children,
                                                                            p => p.ForMember(c => c.Id,
                                                                                             cc => cc
                                                                                                 .MapFromColumnName("Child_Id",
                                                                                                                    true))
                                                                                  .ForMember(c => c.Name,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_Name"))
                                                                                  .ForMember(c => c.Gender,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_Gender"))
                                                                                  .ForMember(c => c
                                                                                                 .DateOfBirth,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_DateOfBirth")));

            this.target.UseMappingProfile(mappingProfile);
            var actual = this.target.Map<Person>(dataReader)
                             .ToList();
            actual.Should()
                  .NotBeNullOrEmpty();
            actual.Count.Should()
                  .Be(2);

            var firstParent = actual.First();
            firstParent.Id.Should()
                       .Be(1);
            firstParent.Name.Should()
                       .Be("Male");
            firstParent.Gender.Should()
                       .Be(Gender.Male);
            firstParent.DateOfBirth.Should()
                       .Be(dateOfBirth);

            firstParent.Children.Should()
                       .NotBeNullOrEmpty();
            firstParent.Children.Count()
                       .Should()
                       .Be(1);

            var firstChild = firstParent.Children.First();
            firstChild.Id.Should()
                      .Be(3);
            firstChild.Name.Should()
                      .Be("Boy");
            firstChild.Gender.Should()
                      .Be(Gender.Male);
            firstChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            var secondParent = actual.Last();
            secondParent.Id.Should()
                        .Be(2);
            secondParent.Name.Should()
                        .Be("Female");
            secondParent.Gender.Should()
                        .Be(Gender.Female);
            secondParent.DateOfBirth.Should()
                        .Be(dateOfBirth);

            secondParent.Children.Should()
                        .NotBeNullOrEmpty();
            secondParent.Children.Count()
                        .Should()
                        .Be(1);

            var secondChild = secondParent.Children.First();
            secondChild.Id.Should()
                       .Be(4);
            secondChild.Name.Should()
                       .Be("Girl");
            secondChild.Gender.Should()
                       .Be(Gender.Female);
            secondChild.DateOfBirth.Should()
                       .Be(dateOfBirth);
        }

        [Test]
        public void MapMultipleEntitiesWithMultipleChildrenCorrectlyUsingProfile()
        {
            var dateOfBirth = DateTime.Today;
            var dataReader = DataReaderMockBuilder.CreateNew(4)
                                                  .WithIntColumn("Id",
                                                                 0,
                                                                 1,
                                                                 1,
                                                                 2,
                                                                 2)
                                                  .WithStringColumn("Name",
                                                                    1,
                                                                    "Male",
                                                                    "Male",
                                                                    "Female",
                                                                    "Female")
                                                  .WithIntColumn("Gender",
                                                                 2,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("DateOfBirth",
                                                                      3,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .WithIntColumn("Child_Id",
                                                                 4,
                                                                 3,
                                                                 4,
                                                                 5,
                                                                 6)
                                                  .WithStringColumn("Child_Name",
                                                                    5,
                                                                    "Boy",
                                                                    "Girl",
                                                                    "Boy",
                                                                    "Girl")
                                                  .WithIntColumn("Child_Gender",
                                                                 6,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("Child_DateOfBirth",
                                                                      7,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .Build();

            var mappingProfile = new EntityMappingProfile<Person>();
            mappingProfile.ForMember(e => e.Id, c => c.MapFromColumnName("Id", true))
                          .ForMember(e => e.Name, c => c.MapFromColumnName("Name"))
                          .ForMember(e => e.Gender, c => c.MapFromColumnName("Gender"))
                          .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName("DateOfBirth"))
                          .ForGenericCollectionMember<List<Person>, Person>(e => e.Children,
                                                                            p => p.ForMember(c => c.Id,
                                                                                             cc => cc
                                                                                                 .MapFromColumnName("Child_Id",
                                                                                                                    true))
                                                                                  .ForMember(c => c.Name,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_Name"))
                                                                                  .ForMember(c => c.Gender,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_Gender"))
                                                                                  .ForMember(c => c
                                                                                                 .DateOfBirth,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_DateOfBirth")));

            this.target.UseMappingProfile(mappingProfile);
            var actual = this.target.Map<Person>(dataReader)
                             .ToList();
            actual.Should()
                  .NotBeNullOrEmpty();
            actual.Count.Should()
                  .Be(2);

            var firstParent = actual.First();
            firstParent.Id.Should()
                       .Be(1);
            firstParent.Name.Should()
                       .Be("Male");
            firstParent.Gender.Should()
                       .Be(Gender.Male);
            firstParent.DateOfBirth.Should()
                       .Be(dateOfBirth);

            firstParent.Children.Should()
                       .NotBeNullOrEmpty();
            firstParent.Children.Count()
                       .Should()
                       .Be(2);

            var firstChild = firstParent.Children.First();
            firstChild.Id.Should()
                      .Be(3);
            firstChild.Name.Should()
                      .Be("Boy");
            firstChild.Gender.Should()
                      .Be(Gender.Male);
            firstChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            var secondChild = firstParent.Children.Last();
            secondChild.Id.Should()
                       .Be(4);
            secondChild.Name.Should()
                       .Be("Girl");
            secondChild.Gender.Should()
                       .Be(Gender.Female);
            secondChild.DateOfBirth.Should()
                       .Be(dateOfBirth);

            var secondParent = actual.Last();
            secondParent.Id.Should()
                        .Be(2);
            secondParent.Name.Should()
                        .Be("Female");
            secondParent.Gender.Should()
                        .Be(Gender.Female);
            secondParent.DateOfBirth.Should()
                        .Be(dateOfBirth);

            secondParent.Children.Should()
                        .NotBeNullOrEmpty();
            secondParent.Children.Count()
                        .Should()
                        .Be(2);

            var thirdChild = secondParent.Children.First();
            thirdChild.Id.Should()
                      .Be(5);
            thirdChild.Name.Should()
                      .Be("Boy");
            thirdChild.Gender.Should()
                      .Be(Gender.Male);
            thirdChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            var forthChild = secondParent.Children.Last();
            forthChild.Id.Should()
                      .Be(6);
            forthChild.Name.Should()
                      .Be("Girl");
            forthChild.Gender.Should()
                      .Be(Gender.Female);
            forthChild.DateOfBirth.Should()
                      .Be(dateOfBirth);
        }

        [Test]
        public void MapMultipleEntitiesWithMultipleChildrenInMultipleCollectionsCorrectlyUsingProfile()
        {
            var dateOfBirth = DateTime.Today;
            var dataReader = DataReaderMockBuilder.CreateNew(4)
                                                  .WithIntColumn("Id",
                                                                 0,
                                                                 1,
                                                                 1,
                                                                 2,
                                                                 2)
                                                  .WithStringColumn("Name",
                                                                    1,
                                                                    "Male",
                                                                    "Male",
                                                                    "Female",
                                                                    "Female")
                                                  .WithIntColumn("Gender",
                                                                 2,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("DateOfBirth",
                                                                      3,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .WithIntColumn("Child_Id",
                                                                 4,
                                                                 3,
                                                                 4,
                                                                 5,
                                                                 6)
                                                  .WithStringColumn("Child_Name",
                                                                    5,
                                                                    "Boy",
                                                                    "Girl",
                                                                    "Boy",
                                                                    "Girl")
                                                  .WithIntColumn("Child_Gender",
                                                                 6,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("Child_DateOfBirth",
                                                                      7,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .WithStringColumn("Address_Line1",
                                                                    8,
                                                                    "1 The Street",
                                                                    "1 The Street",
                                                                    "2 The Street",
                                                                    "2 The Street")
                                                  .WithStringColumn("Address_Town",
                                                                    9,
                                                                    "Town",
                                                                    "Town",
                                                                    "Town",
                                                                    "Town")
                                                  .WithStringColumn("Address_PostCode",
                                                                    10,
                                                                    "ABC 123",
                                                                    "ABC 123",
                                                                    "ABC 123",
                                                                    "ABC 123")
                                                  .Build();

            var mappingProfile = new EntityMappingProfile<Person>();
            mappingProfile.ForMember(e => e.Id, c => c.MapFromColumnName("Id", true))
                          .ForMember(e => e.Name, c => c.MapFromColumnName("Name"))
                          .ForMember(e => e.Gender, c => c.MapFromColumnName("Gender"))
                          .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName("DateOfBirth"))
                          .ForGenericCollectionMember<List<Person>, Person>(e => e.Children,
                                                                            p => p.ForMember(c => c.Id,
                                                                                             cc => cc
                                                                                                 .MapFromColumnName("Child_Id",
                                                                                                                    true))
                                                                                  .ForMember(c => c.Name,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_Name"))
                                                                                  .ForMember(c => c.Gender,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_Gender"))
                                                                                  .ForMember(c => c
                                                                                                 .DateOfBirth,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_DateOfBirth")))
                          .ForArrayMember<Address>(e => e.Addresses,
                                                   p => p.ForMember(c => c.Line1,
                                                                    cc =>
                                                                        cc.MapFromColumnName("Address_Line1",
                                                                                             true))
                                                         .ForMember(c => c.Town,
                                                                    cc =>
                                                                        cc.MapFromColumnName("Address_Town"))
                                                         .ForMember(e => e.PostCode,
                                                                    cc =>
                                                                        cc.MapFromColumnName("Address_PostCode",
                                                                                             true)));

            this.target.UseMappingProfile(mappingProfile);
            var actual = this.target.Map<Person>(dataReader)
                             .ToList();
            actual.Should()
                  .NotBeNullOrEmpty();
            actual.Count.Should()
                  .Be(2);

            var firstParent = actual.First();
            firstParent.Id.Should()
                       .Be(1);
            firstParent.Name.Should()
                       .Be("Male");
            firstParent.Gender.Should()
                       .Be(Gender.Male);
            firstParent.DateOfBirth.Should()
                       .Be(dateOfBirth);

            firstParent.Children.Should()
                       .NotBeNullOrEmpty();
            firstParent.Children.Count()
                       .Should()
                       .Be(2);
            firstParent.Addresses.Should()
                       .NotBeNullOrEmpty();
            firstParent.Addresses.Length.Should()
                       .Be(1);

            var firstChild = firstParent.Children.First();
            firstChild.Id.Should()
                      .Be(3);
            firstChild.Name.Should()
                      .Be("Boy");
            firstChild.Gender.Should()
                      .Be(Gender.Male);
            firstChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            var secondChild = firstParent.Children.Last();
            secondChild.Id.Should()
                       .Be(4);
            secondChild.Name.Should()
                       .Be("Girl");
            secondChild.Gender.Should()
                       .Be(Gender.Female);
            secondChild.DateOfBirth.Should()
                       .Be(dateOfBirth);

            var firstAddress = firstParent.Addresses.First();
            firstAddress.Line1.Should()
                        .Be("1 The Street");
            firstAddress.Town.Should()
                        .Be("Town");
            firstAddress.PostCode.Should()
                        .Be("ABC 123");

            var secondParent = actual.Last();
            secondParent.Id.Should()
                        .Be(2);
            secondParent.Name.Should()
                        .Be("Female");
            secondParent.Gender.Should()
                        .Be(Gender.Female);
            secondParent.DateOfBirth.Should()
                        .Be(dateOfBirth);

            secondParent.Children.Should()
                        .NotBeNullOrEmpty();
            secondParent.Children.Count()
                        .Should()
                        .Be(2);

            secondParent.Addresses.Should()
                        .NotBeNullOrEmpty();
            secondParent.Addresses.Length.Should()
                        .Be(1);

            var thirdChild = secondParent.Children.First();
            thirdChild.Id.Should()
                      .Be(5);
            thirdChild.Name.Should()
                      .Be("Boy");
            thirdChild.Gender.Should()
                      .Be(Gender.Male);
            thirdChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            var forthChild = secondParent.Children.Last();
            forthChild.Id.Should()
                      .Be(6);
            forthChild.Name.Should()
                      .Be("Girl");
            forthChild.Gender.Should()
                      .Be(Gender.Female);
            forthChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            var secondAddress = secondParent.Addresses.First();
            secondAddress.Line1.Should()
                         .Be("2 The Street");
            secondAddress.Town.Should()
                         .Be("Town");
            secondAddress.PostCode.Should()
                         .Be("ABC 123");
        }

        [Test]
        public void MapMultipleEntitiesWithMultipleLevelsOfChildrenCorrectlyUsingProfile()
        {
            var dateOfBirth = DateTime.Today;
            var dataReader = DataReaderMockBuilder.CreateNew(4)
                                                  .WithIntColumn("Id",
                                                                 0,
                                                                 1,
                                                                 1,
                                                                 2,
                                                                 2)
                                                  .WithStringColumn("Name",
                                                                    1,
                                                                    "Male",
                                                                    "Male",
                                                                    "Female",
                                                                    "Female")
                                                  .WithIntColumn("Gender",
                                                                 2,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("DateOfBirth",
                                                                      3,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .WithIntColumn("Child_Id",
                                                                 4,
                                                                 3,
                                                                 4,
                                                                 5,
                                                                 6)
                                                  .WithStringColumn("Child_Name",
                                                                    5,
                                                                    "Boy",
                                                                    "Girl",
                                                                    "Boy",
                                                                    "Girl")
                                                  .WithIntColumn("Child_Gender",
                                                                 6,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("Child_DateOfBirth",
                                                                      7,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .WithIntColumn("Grandchild_Id",
                                                                 8,
                                                                 7,
                                                                 8,
                                                                 9,
                                                                 10)
                                                  .WithStringColumn("Grandchild_Name",
                                                                    9,
                                                                    "Boy GC",
                                                                    "Girl GC",
                                                                    "Boy GC",
                                                                    "Girl GC")
                                                  .WithIntColumn("Grandchild_Gender",
                                                                 10,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("Grandchild_DateOfBirth",
                                                                      11,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .Build();

            var mappingProfile = new EntityMappingProfile<Person>();
            mappingProfile.ForMember(e => e.Id, c => c.MapFromColumnName("Id", true))
                          .ForMember(e => e.Name, c => c.MapFromColumnName("Name"))
                          .ForMember(e => e.Gender, c => c.MapFromColumnName("Gender"))
                          .ForMember(e => e.DateOfBirth, c => c.MapFromColumnName("DateOfBirth"))
                          .ForGenericCollectionMember<List<Person>, Person>(e => e.Children,
                                                                            p => p.ForMember(c => c.Id,
                                                                                             cc => cc
                                                                                                 .MapFromColumnName("Child_Id",
                                                                                                                    true))
                                                                                  .ForMember(c => c.Name,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_Name"))
                                                                                  .ForMember(c => c.Gender,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_Gender"))
                                                                                  .ForMember(c => c
                                                                                                 .DateOfBirth,
                                                                                             cc =>
                                                                                                 cc
                                                                                                     .MapFromColumnName("Child_DateOfBirth"))
                                                                                  .ForGenericCollectionMember
                                                                                  <List<Person>, Person
                                                                                  >(c => c.Children,
                                                                                    cp =>
                                                                                        cp.ForMember(gc =>
                                                                                                         gc
                                                                                                             .Id,
                                                                                                     gcp =>
                                                                                                         gcp
                                                                                                             .MapFromColumnName("Grandchild_Id",
                                                                                                                                true))
                                                                                          .ForMember(gc => gc.Name, gcp => gcp.MapFromColumnName("Grandchild_Name"))
                                                                                          .ForMember(gc => gc.Gender, gcp => gcp.MapFromColumnName("Grandchild_Gender"))
                                                                                          .ForMember(gc => gc.DateOfBirth, gcp => gcp.MapFromColumnName("Grandchild_DateOfBirth"))));

            this.target.UseMappingProfile(mappingProfile);
            var actual = this.target.Map<Person>(dataReader)
                             .ToList();
            actual.Should()
                  .NotBeNullOrEmpty();
            actual.Count.Should()
                  .Be(2);

            var firstParent = actual.First();
            firstParent.Id.Should()
                       .Be(1);
            firstParent.Name.Should()
                       .Be("Male");
            firstParent.Gender.Should()
                       .Be(Gender.Male);
            firstParent.DateOfBirth.Should()
                       .Be(dateOfBirth);

            firstParent.Children.Should()
                       .NotBeNullOrEmpty();
            firstParent.Children.Count()
                       .Should()
                       .Be(2);

            var firstChild = firstParent.Children.First();
            firstChild.Id.Should()
                      .Be(3);
            firstChild.Name.Should()
                      .Be("Boy");
            firstChild.Gender.Should()
                      .Be(Gender.Male);
            firstChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            firstChild.Children.Should()
                      .NotBeNullOrEmpty();
            firstChild.Children.Count()
                      .Should()
                      .Be(1);

            var firstGrandchild = firstChild.Children.First();
            firstGrandchild.Id.Should()
                           .Be(7);
            firstGrandchild.Name.Should()
                           .Be("Boy GC");
            firstGrandchild.Gender.Should()
                           .Be(Gender.Male);
            firstGrandchild.DateOfBirth.Should()
                           .Be(dateOfBirth);

            var secondChild = firstParent.Children.Last();
            secondChild.Id.Should()
                       .Be(4);
            secondChild.Name.Should()
                       .Be("Girl");
            secondChild.Gender.Should()
                       .Be(Gender.Female);
            secondChild.DateOfBirth.Should()
                       .Be(dateOfBirth);

            var secondGrandchild = secondChild.Children.First();
            secondGrandchild.Id.Should()
                           .Be(8);
            secondGrandchild.Name.Should()
                           .Be("Girl GC");
            secondGrandchild.Gender.Should()
                           .Be(Gender.Female);
            secondGrandchild.DateOfBirth.Should()
                           .Be(dateOfBirth);

            var secondParent = actual.Last();
            secondParent.Id.Should()
                        .Be(2);
            secondParent.Name.Should()
                        .Be("Female");
            secondParent.Gender.Should()
                        .Be(Gender.Female);
            secondParent.DateOfBirth.Should()
                        .Be(dateOfBirth);

            secondParent.Children.Should()
                        .NotBeNullOrEmpty();
            secondParent.Children.Count()
                        .Should()
                        .Be(2);

            var thirdChild = secondParent.Children.First();
            thirdChild.Id.Should()
                      .Be(5);
            thirdChild.Name.Should()
                      .Be("Boy");
            thirdChild.Gender.Should()
                      .Be(Gender.Male);
            thirdChild.DateOfBirth.Should()
                      .Be(dateOfBirth);
            
            var thirdGrandchild = thirdChild.Children.First();
            thirdGrandchild.Id.Should()
                           .Be(9);
            thirdGrandchild.Name.Should()
                           .Be("Boy GC");
            thirdGrandchild.Gender.Should()
                           .Be(Gender.Male);
            thirdGrandchild.DateOfBirth.Should()
                           .Be(dateOfBirth);

            var fourthChild = secondParent.Children.Last();
            fourthChild.Id.Should()
                      .Be(6);
            fourthChild.Name.Should()
                      .Be("Girl");
            fourthChild.Gender.Should()
                      .Be(Gender.Female);
            fourthChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            var fourthGrandchild = fourthChild.Children.First();
            fourthGrandchild.Id.Should()
                           .Be(10);
            fourthGrandchild.Name.Should()
                           .Be("Girl GC");
            fourthGrandchild.Gender.Should()
                           .Be(Gender.Female);
            fourthGrandchild.DateOfBirth.Should()
                           .Be(dateOfBirth);
        }

        [Test]
        public void MapMultipleEntitiesWithMultipleLevelsOfChildrenCorrectlyUsingProfileAndIndexMapping()
        {
            var dateOfBirth = DateTime.Today;
            var dataReader = DataReaderMockBuilder.CreateNew(4)
                                                  .WithIntColumn("Id",
                                                                 0,
                                                                 1,
                                                                 1,
                                                                 2,
                                                                 2)
                                                  .WithStringColumn("Name",
                                                                    1,
                                                                    "Male",
                                                                    "Male",
                                                                    "Female",
                                                                    "Female")
                                                  .WithIntColumn("Gender",
                                                                 2,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("DateOfBirth",
                                                                      3,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .WithIntColumn("Child_Id",
                                                                 4,
                                                                 3,
                                                                 4,
                                                                 5,
                                                                 6)
                                                  .WithStringColumn("Child_Name",
                                                                    5,
                                                                    "Boy",
                                                                    "Girl",
                                                                    "Boy",
                                                                    "Girl")
                                                  .WithIntColumn("Child_Gender",
                                                                 6,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("Child_DateOfBirth",
                                                                      7,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .WithIntColumn("Grandchild_Id",
                                                                 8,
                                                                 7,
                                                                 8,
                                                                 9,
                                                                 10)
                                                  .WithStringColumn("Grandchild_Name",
                                                                    9,
                                                                    "Boy GC",
                                                                    "Girl GC",
                                                                    "Boy GC",
                                                                    "Girl GC")
                                                  .WithIntColumn("Grandchild_Gender",
                                                                 10,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female,
                                                                 (int)Gender.Male,
                                                                 (int)Gender.Female)
                                                  .WithDateTimeColumn("Grandchild_DateOfBirth",
                                                                      11,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth,
                                                                      dateOfBirth)
                                                  .Build();

            var grandchildProfile = new EntityMappingProfile<Person>();
             grandchildProfile.ForMember(gc => gc.Id, gcp => gcp.MapFromIndex(8, true))
                    .ForMember(gc => gc.Name, gcp => gcp.MapFromIndex(9))
                    .ForMember(gc => gc.Gender, gcp => gcp.MapFromIndex(10))
                    .ForMember(gc => gc.DateOfBirth, gcp => gcp.MapFromIndex(11));

             var childProfile = new EntityMappingProfile<Person>();
             childProfile.ForMember(c => c.Id, cc => cc.MapFromIndex(4, true))
                   .ForMember(c => c.Name, cc => cc.MapFromIndex(5))
                   .ForMember(c => c.Gender, cc => cc.MapFromIndex(6))
                   .ForMember(c => c.DateOfBirth, cc => cc.MapFromIndex(7))
                   .ForGenericCollectionMember<List<Person>, Person>(c => c.Children, grandchildProfile);

            var mappingProfile = new EntityMappingProfile<Person>();
            mappingProfile.ForMember(e => e.Id, c => c.MapFromIndex(0, true))
                          .ForMember(e => e.Name, c => c.MapFromIndex(1))
                          .ForMember(e => e.Gender, c => c.MapFromIndex(2))
                          .ForMember(e => e.DateOfBirth, c => c.MapFromIndex(3))
                          .ForGenericCollectionMember<List<Person>, Person>(e => e.Children, childProfile);

            this.target.UseMappingProfile(mappingProfile);
            var actual = this.target.Map<Person>(dataReader)
                             .ToList();
            actual.Should()
                  .NotBeNullOrEmpty();
            actual.Count.Should()
                  .Be(2);

            var firstParent = actual.First();
            firstParent.Id.Should()
                       .Be(1);
            firstParent.Name.Should()
                       .Be("Male");
            firstParent.Gender.Should()
                       .Be(Gender.Male);
            firstParent.DateOfBirth.Should()
                       .Be(dateOfBirth);

            firstParent.Children.Should()
                       .NotBeNullOrEmpty();
            firstParent.Children.Count()
                       .Should()
                       .Be(2);

            var firstChild = firstParent.Children.First();
            firstChild.Id.Should()
                      .Be(3);
            firstChild.Name.Should()
                      .Be("Boy");
            firstChild.Gender.Should()
                      .Be(Gender.Male);
            firstChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            firstChild.Children.Should()
                      .NotBeNullOrEmpty();
            firstChild.Children.Count()
                      .Should()
                      .Be(1);

            var firstGrandchild = firstChild.Children.First();
            firstGrandchild.Id.Should()
                           .Be(7);
            firstGrandchild.Name.Should()
                           .Be("Boy GC");
            firstGrandchild.Gender.Should()
                           .Be(Gender.Male);
            firstGrandchild.DateOfBirth.Should()
                           .Be(dateOfBirth);

            var secondChild = firstParent.Children.Last();
            secondChild.Id.Should()
                       .Be(4);
            secondChild.Name.Should()
                       .Be("Girl");
            secondChild.Gender.Should()
                       .Be(Gender.Female);
            secondChild.DateOfBirth.Should()
                       .Be(dateOfBirth);

            var secondGrandchild = secondChild.Children.First();
            secondGrandchild.Id.Should()
                           .Be(8);
            secondGrandchild.Name.Should()
                           .Be("Girl GC");
            secondGrandchild.Gender.Should()
                           .Be(Gender.Female);
            secondGrandchild.DateOfBirth.Should()
                           .Be(dateOfBirth);

            var secondParent = actual.Last();
            secondParent.Id.Should()
                        .Be(2);
            secondParent.Name.Should()
                        .Be("Female");
            secondParent.Gender.Should()
                        .Be(Gender.Female);
            secondParent.DateOfBirth.Should()
                        .Be(dateOfBirth);

            secondParent.Children.Should()
                        .NotBeNullOrEmpty();
            secondParent.Children.Count()
                        .Should()
                        .Be(2);

            var thirdChild = secondParent.Children.First();
            thirdChild.Id.Should()
                      .Be(5);
            thirdChild.Name.Should()
                      .Be("Boy");
            thirdChild.Gender.Should()
                      .Be(Gender.Male);
            thirdChild.DateOfBirth.Should()
                      .Be(dateOfBirth);
            
            var thirdGrandchild = thirdChild.Children.First();
            thirdGrandchild.Id.Should()
                           .Be(9);
            thirdGrandchild.Name.Should()
                           .Be("Boy GC");
            thirdGrandchild.Gender.Should()
                           .Be(Gender.Male);
            thirdGrandchild.DateOfBirth.Should()
                           .Be(dateOfBirth);

            var fourthChild = secondParent.Children.Last();
            fourthChild.Id.Should()
                      .Be(6);
            fourthChild.Name.Should()
                      .Be("Girl");
            fourthChild.Gender.Should()
                      .Be(Gender.Female);
            fourthChild.DateOfBirth.Should()
                      .Be(dateOfBirth);

            var fourthGrandchild = fourthChild.Children.First();
            fourthGrandchild.Id.Should()
                           .Be(10);
            fourthGrandchild.Name.Should()
                           .Be("Girl GC");
            fourthGrandchild.Gender.Should()
                           .Be(Gender.Female);
            fourthGrandchild.DateOfBirth.Should()
                           .Be(dateOfBirth);
        }

        private DataReaderEntityMapper target;
    }
}