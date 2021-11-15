using System;
using ExtractToWork.Core;
using Xunit;

namespace ExtractToWork.Test
{
    public class UtilTest
    {
        [Fact]
        public void RemoveExtensionReturnsProper()
        {
            Assert.Equal("filename", Utils.RemoveFileExtension("filename.txt"));
        }

        [Fact]
        public void RemoveExtensionWithNoExtension()
        {
            Assert.Equal("filename", Utils.RemoveFileExtension("filename"));
        }

        [Fact]
        public void RemoveExtensionExtensionWithNumber()
        {
            Assert.Equal("filename", Utils.RemoveFileExtension("filename.json5"));
        }

        [Fact]
        public void RemoveExtensionWithMultipleDots()
        {
            Assert.Equal("filename.asd.txt", Utils.RemoveFileExtension("filename.asd.txt.json"));
        }

        [Fact]
        public void RemoveExtensionThrowsWhenEmpty()
        {
            Assert.Equal("", Utils.RemoveFileExtension(""));
        }

        [Fact]
        public void RemoveExtensionLeavesDir()
        {
            Assert.Equal("net5.0\\filename", Utils.RemoveFileExtension("net5.0\\filename.txt"));
        }

        [Fact]
        public void RemoveExtensionMultipleDirsLeavesDir()
        {
            Assert.Equal("asd\\basdasdad\\net5.0\\filename", Utils.RemoveFileExtension("asd\\basdasdad\\net5.0\\filename.txt"));
        }

        [Fact]
        public void RemoveExtensionFullPath()
        {
            Assert.Equal("C:\\asd\\basdasdad\\net5.0\\filename", Utils.RemoveFileExtension("C:\\asd\\basdasdad\\net5.0\\filename.txt"));
        }
    }
}
