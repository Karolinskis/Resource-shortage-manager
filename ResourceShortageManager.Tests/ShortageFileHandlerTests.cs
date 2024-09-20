using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using ResourceShortageManager.Models;
using ResourceShortageManager.Services;

namespace ResourceShortageManager.Tests
{
    public class ShortageFileHandlerTests
    {
        private readonly string _testFilePath = "test_shortages.json";

        private ShortageFileHandler CreateHandler()
        {
            return new ShortageFileHandler(_testFilePath);
        }

        [Fact]
        public void LoadShortages_FileDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
            var handler = CreateHandler();

            // Act
            var result = handler.LoadShortages();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void LoadShortages_FileExists_ReturnsShortages()
        {
            // Arrange
            var shortages = new List<Shortage>
            {
                new Shortage {
                    Title = "Test Shortage",
                    Name = "TestUser",
                    Room = Room.Kitchen,
                    Category = Category.Food,
                    Priority = 1,
                    CreatedOn = DateTime.Now
                }
            };
            File.WriteAllText(_testFilePath, JsonConvert.SerializeObject(shortages));
            var handler = CreateHandler();

            // Act
            var loadedShortages = handler.LoadShortages();

            // Assert
            Assert.NotNull(loadedShortages);
            Assert.Single(loadedShortages);
            Assert.Equal("Test Shortage", loadedShortages[0].Title);
        }

        [Fact]
        public void SaveShortages_WritesToFile()
        {
            // Arrange
            var shortages = new List<Shortage>
            {
                new Shortage {
                    Title = "Test Shortage",
                    Name = "TestUser",
                    Room = Room.Kitchen,
                    Category = Category.Food,
                    Priority = 1,
                    CreatedOn = DateTime.Now
                }
            };
            var handler = CreateHandler();

            // Act
            handler.SaveShortages(shortages);

            // Assert
            var jsonData = File.ReadAllText(_testFilePath);
            var loadedShortages = JsonConvert.DeserializeObject<List<Shortage>>(jsonData);
            Assert.NotNull(loadedShortages);
            Assert.Single(loadedShortages);
            Assert.Equal("Test Shortage", loadedShortages[0].Title);
        }

        [Fact]
        public void SaveShortages_EmptyList_CreatesEmptyFile()
        {
            // Arrange
            var shortages = new List<Shortage>();
            var handler = CreateHandler();

            // Act
            handler.SaveShortages(shortages);

            // Assert
            var jsonData = File.ReadAllText(_testFilePath);
            var loadedShortages = JsonConvert.DeserializeObject<List<Shortage>>(jsonData);
            Assert.NotNull(loadedShortages);
            Assert.Empty(loadedShortages);
        }
    }
}
