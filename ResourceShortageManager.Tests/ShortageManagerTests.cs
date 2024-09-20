using System;
using Moq;
using ResourceShortageManager.Services;
using Xunit;

using ResourceShortageManager.Managers;
using ResourceShortageManager.Models;
using ResousceShortageManager.Services;

namespace ResourceShortageManager.Tests
{
    public class ShortageManagerTests
    {
        private readonly Mock<IFileHandler> _mockShortageFileHandler;
        private readonly ShortageManager _shortageManager;

        public ShortageManagerTests()
        {
            _mockShortageFileHandler = new Mock<IFileHandler>();
            _shortageManager = new ShortageManager("TestUser", _mockShortageFileHandler.Object);
        }

        [Fact]
        public void RegisterShortage_ValidInput_AddsShortage()
        {
            // Arrange
            var shortages = new List<Shortage>();

            _mockShortageFileHandler.Setup(s => s.LoadShortages()).Returns(shortages);
            _mockShortageFileHandler.Setup(s => s.SaveShortages(It.IsAny<List<Shortage>>()));

            var stringReader = new StringReader("TestTitle\nMeeting room\nElectronics\n5\n");
            Console.SetIn(stringReader);

            // Act
            _shortageManager.RegisterShortage();

            // Assert
            Assert.Single(shortages);
            Assert.Equal("TestTitle", shortages[0].Title);
            Assert.Equal("TestUser", shortages[0].Name);
            Assert.Equal(Room.MeetingRoom, shortages[0].Room);
            Assert.Equal(Category.Electronics, shortages[0].Category);
            Assert.Equal(5, shortages[0].Priority);
        }

        [Fact]
        public void RegisterShortage_ExistingShortageWithHigherPriority_DoesNotUpdate()
        {
            // Arrange
            var shortages = new List<Shortage>
            {
                new Shortage
                {
                    Title = "TestTitle",
                    Name = "TestUser",
                    Room = Room.MeetingRoom,
                    Category = Category.Electronics,
                    Priority = 10,
                    CreatedOn = DateTime.Now
                }
            };
            _mockShortageFileHandler.Setup(s => s.LoadShortages()).Returns(shortages);
            _mockShortageFileHandler.Setup(s => s.SaveShortages(It.IsAny<List<Shortage>>()));

            var stringReader = new StringReader("TestTitle\nMeeting room\nElectronics\n5\n");
            Console.SetIn(stringReader);

            // Act
            _shortageManager.RegisterShortage();

            // Assert
            Assert.Single(shortages);
            Assert.Equal(10, shortages[0].Priority);
        }

        [Fact]
        public void RegisterShortage_ExistingShortageWithLowerPriority_UpdatesPriority()
        {
            // Arrange
            var shortages = new List<Shortage>
            {
                new Shortage
                {
                    Title = "TestTitle",
                    Name = "TestUser",
                    Room = Room.MeetingRoom,
                    Category = Category.Electronics,
                    Priority = 3,
                    CreatedOn = DateTime.Now
                }
            };
            _mockShortageFileHandler.Setup(s => s.LoadShortages()).Returns(shortages);
            _mockShortageFileHandler.Setup(s => s.SaveShortages(It.IsAny<List<Shortage>>()));

            var stringReader = new StringReader("TestTitle\nMeeting room\nElectronics\n5\nY\n");
            Console.SetIn(stringReader);

            // Act
            _shortageManager.RegisterShortage();

            // Assert
            Assert.Single(shortages);
            Assert.Equal(5, shortages[0].Priority);
        }

        [Fact]
        public void DeleteShortage_ValidInput_RemovesShortage()
        {
            // Arrange
            var shortages = new List<Shortage>
            {
                new Shortage
                {
                    Title = "TestTitle",
                    Name = "TestUser",
                    Room = Room.MeetingRoom,
                    Category = Category.Electronics,
                    Priority = 5,
                    CreatedOn = DateTime.Now
                }
            };
            _mockShortageFileHandler.Setup(s => s.LoadShortages()).Returns(shortages);
            _mockShortageFileHandler.Setup(s => s.SaveShortages(It.IsAny<List<Shortage>>()));

            var stringReader = new StringReader("TestTitle\nMeeting room\nElectronics\nY\n");
            Console.SetIn(stringReader);

            // Act
            _shortageManager.DeleteShortage();

            // Assert
            Assert.Empty(shortages);
        }

        [Fact]
        public void DeleteShortage_NonExistentShortage_DoesNothing()
        {
            // Arrange
            var shortages = new List<Shortage>();
            _mockShortageFileHandler.Setup(s => s.LoadShortages()).Returns(shortages);
            _mockShortageFileHandler.Setup(s => s.SaveShortages(It.IsAny<List<Shortage>>()));

            var stringReader = new StringReader("NonExistentTitle\nMeeting room\nElectronics\nY\n");
            Console.SetIn(stringReader);

            // Act
            _shortageManager.DeleteShortage();

            // Assert
            Assert.Empty(shortages);
        }
    }
}
