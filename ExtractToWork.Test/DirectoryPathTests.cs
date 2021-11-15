using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtractToWork.Core;
using Xunit;

namespace ExtractToWork.Test
{
    public class DirectoryPathTests
    {
        [Fact]
        public void ThrowsWhenBaseIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => { DirectoryPathGenerator generator = new DirectoryPathGenerator(null, "", " orig");});
        }

        [Fact]
        public void ThrowsWhenPathNotValid()
        {
            Assert.Throws<ArgumentException>(() => { DirectoryPathGenerator generator = new DirectoryPathGenerator("asd", "", " orig"); });
        }


        [Fact]
        public void ValidReturns()
        {
            string date = DateTimeOffset.Now.ToString("yyyy-MM-dd");
            var generator = new DirectoryPathGenerator("E:\\Work\\Current\\", date, " orig");

            Assert.Equal($"E:\\Work\\Current\\{date}\\update orig\\", generator.CreatePath("update.zip"));
        }

        [Fact]
        public void NoEndAppend()
        {
            string date = DateTimeOffset.Now.ToString("yyyy-MM-dd");
            var generator = new DirectoryPathGenerator("E:\\Work\\Current\\", date);

            Assert.Equal($"E:\\Work\\Current\\{date}\\update\\", generator.CreatePath("update.zip"));
        }

        [Fact]
        public void NoDirectoryAppend()
        {
            var generator = new DirectoryPathGenerator("E:\\Work\\Current\\", appendToEnd: " orig");

            Assert.Equal("E:\\Work\\Current\\update orig\\", generator.CreatePath("update.zip"));
        }

        [Fact]
        public void NoDirectoryAndEndAppend()
        {
            var generator = new DirectoryPathGenerator("E:\\Work\\Current\\");

            Assert.Equal("E:\\Work\\Current\\update\\", generator.CreatePath("update.zip"));
        }

        [Fact]
        public void DuplacateBackslashes()
        {
            var generator = new DirectoryPathGenerator("E:\\Work\\Current\\\\\\", "\\asd\\\\");

            Assert.Equal("E:\\Work\\Current\\asd\\update\\", generator.CreatePath("update.zip"));
        }
    }
}
