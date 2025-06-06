// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Text.Json;
using Microsoft.NET.Sdk.WebAssembly;

namespace Microsoft.NET.Sdk.BlazorWebAssembly.Tests
{
    public class WasmBuildLazyLoadTest : AspNetSdkTest
    {
        public WasmBuildLazyLoadTest(ITestOutputHelper log) : base(log) { }

        [RequiresMSBuildVersionFact("17.12", Reason = "Needs System.Text.Json 8.0.5")]
        public void Build_LazyLoadExplicitAssembly_Debug_Works()
        {
            // Arrange
            var testAppName = "BlazorWasmWithLibrary";
            var testInstance = CreateAspNetSdkTestAsset(testAppName);

            testInstance.WithProjectChanges((project) =>
            {
                var ns = project.Root.Name.Namespace;
                var itemGroup = new XElement(ns + "ItemGroup");
                itemGroup.Add(new XElement("BlazorWebAssemblyLazyLoad",
                    new XAttribute("Include", "RazorClassLibrary.wasm")));
                project.Root.Add(itemGroup);

                var propertyGroup = new XElement("PropertyGroup");
                propertyGroup.Add(new XElement("WasmFingerprintAssets", false));
                project.Root.Add(propertyGroup);
            });

            // Act
            var buildCommand = CreateBuildCommand(testInstance, "blazorwasm");
            ExecuteCommand(buildCommand)
                .Should().Pass();

            var outputDirectory = buildCommand.GetOutputDirectory(DefaultTfm);

            // Assert
            var expectedFiles = new[]
            {
                $"wwwroot/_framework/{WasmBootConfigFileName}",
                "wwwroot/_framework/RazorClassLibrary.wasm"
            };

            outputDirectory.Should().HaveFiles(expectedFiles);

            var bootJson = ReadBootJsonData(Path.Combine(outputDirectory.ToString(), "wwwroot", "_framework", WasmBootConfigFileName));

            // And that it has been labelled as a dynamic assembly in the boot.json
            var lazyAssemblies = bootJson.resources.lazyAssembly;
            var assemblies = bootJson.resources.assembly;

            lazyAssemblies.Should().NotBeNull();

            lazyAssemblies.Keys.Should().Contain("RazorClassLibrary.wasm");
            assemblies.Keys.Should().NotContain("RazorClassLibrary.wasm");

            // App assembly should not be lazy loaded
            lazyAssemblies.Keys.Should().NotContain("blazorwasm.wasm");
            assemblies.Keys.Should().Contain("blazorwasm.wasm");
        }

        [RequiresMSBuildVersionFact("17.12", Reason = "Needs System.Text.Json 8.0.5")]
        public void Build_LazyLoadExplicitAssembly_Release_Works()
        {
            // Arrange
            var testAppName = "BlazorWasmWithLibrary";
            var testInstance = CreateAspNetSdkTestAsset(testAppName);

            testInstance.WithProjectChanges((project) =>
            {
                var ns = project.Root.Name.Namespace;
                var itemGroup = new XElement(ns + "ItemGroup");
                itemGroup.Add(new XElement("BlazorWebAssemblyLazyLoad",
                    new XAttribute("Include", "RazorClassLibrary.wasm")));
                project.Root.Add(itemGroup);

                var propertyGroup = new XElement("PropertyGroup");
                propertyGroup.Add(new XElement("WasmFingerprintAssets", false));
                project.Root.Add(propertyGroup);
            });

            // Act
            var buildCommand = CreateBuildCommand(testInstance, "blazorwasm");
            buildCommand.Execute("/p:Configuration=Release")
                .Should().Pass();

            var outputDirectory = buildCommand.GetOutputDirectory(DefaultTfm, "Release");

            // Assert
            var expectedFiles = new[]
            {
                $"wwwroot/_framework/{WasmBootConfigFileName}",
                "wwwroot/_framework/RazorClassLibrary.wasm"
            };

            outputDirectory.Should().HaveFiles(expectedFiles);

            var bootJson = ReadBootJsonData(Path.Combine(outputDirectory.ToString(), "wwwroot", "_framework", WasmBootConfigFileName));

            // And that it has been labelled as a dynamic assembly in the boot.json
            var lazyAssemblies = bootJson.resources.lazyAssembly;
            var assemblies = bootJson.resources.assembly;

            lazyAssemblies.Should().NotBeNull();

            lazyAssemblies.Keys.Should().Contain("RazorClassLibrary.wasm");
            assemblies.Keys.Should().NotContain("RazorClassLibrary.wasm");

            // App assembly should not be lazy loaded
            lazyAssemblies.Keys.Should().NotContain("blazorwasm.wasm");
            assemblies.Keys.Should().Contain("blazorwasm.wasm");
        }

        [RequiresMSBuildVersionFact("17.12", Reason = "Needs System.Text.Json 8.0.5")]
        public void Publish_LazyLoadExplicitAssembly_Debug_Works()
        {
            // Arrange
            var testAppName = "BlazorWasmWithLibrary";
            var testInstance = CreateAspNetSdkTestAsset(testAppName);

            testInstance.WithProjectChanges((project) =>
            {
                var ns = project.Root.Name.Namespace;
                var itemGroup = new XElement(ns + "ItemGroup");
                itemGroup.Add(new XElement("BlazorWebAssemblyLazyLoad",
                    new XAttribute("Include", "RazorClassLibrary.wasm")));
                project.Root.Add(itemGroup);

                var propertyGroup = new XElement("PropertyGroup");
                propertyGroup.Add(new XElement("WasmFingerprintAssets", false));
                project.Root.Add(propertyGroup);
            });

            // Act
            var publishCommand = new PublishCommand(testInstance, "blazorwasm");
            publishCommand.Execute()
                .Should().Pass();

            var outputDirectory = publishCommand.GetOutputDirectory(DefaultTfm);

            // Assert
            var expectedFiles = new[]
            {
                $"wwwroot/_framework/{WasmBootConfigFileName}",
                "wwwroot/_framework/RazorClassLibrary.wasm"
            };

            outputDirectory.Should().HaveFiles(expectedFiles);

            var bootJson = ReadBootJsonData(Path.Combine(outputDirectory.ToString(), "wwwroot", "_framework", WasmBootConfigFileName));

            // And that it has been labelled as a dynamic assembly in the boot.json
            var lazyAssemblies = bootJson.resources.lazyAssembly;
            var assemblies = bootJson.resources.assembly;

            lazyAssemblies.Should().NotBeNull();

            lazyAssemblies.Keys.Should().Contain("RazorClassLibrary.wasm");
            assemblies.Keys.Should().NotContain("RazorClassLibrary.wasm");

            // App assembly should not be lazy loaded
            lazyAssemblies.Keys.Should().NotContain("blazorwasm.wasm");
            assemblies.Keys.Should().Contain("blazorwasm.wasm");
        }

        [RequiresMSBuildVersionFact("17.12", Reason = "Needs System.Text.Json 8.0.5")]
        public void Publish_LazyLoadExplicitAssembly_Release_Works()
        {
            // Arrange
            var testAppName = "BlazorWasmWithLibrary";
            var testInstance = CreateAspNetSdkTestAsset(testAppName);

            testInstance.WithProjectChanges((project) =>
            {
                var ns = project.Root.Name.Namespace;
                var itemGroup = new XElement(ns + "ItemGroup");
                itemGroup.Add(new XElement("BlazorWebAssemblyLazyLoad",
                    new XAttribute("Include", "RazorClassLibrary.wasm")));
                project.Root.Add(itemGroup);

                var propertyGroup = new XElement("PropertyGroup");
                propertyGroup.Add(new XElement("WasmFingerprintAssets", false));
                project.Root.Add(propertyGroup);
            });

            // Act
            var publishCommand = new PublishCommand(testInstance, "blazorwasm");
            publishCommand.Execute("/p:Configuration=Release")
                .Should().Pass();

            var outputDirectory = publishCommand.GetOutputDirectory(DefaultTfm, "Release");

            // Assert
            var expectedFiles = new[]
            {
                $"wwwroot/_framework/{WasmBootConfigFileName}",
                "wwwroot/_framework/RazorClassLibrary.wasm"
            };

            outputDirectory.Should().HaveFiles(expectedFiles);

            var bootJson = ReadBootJsonData(Path.Combine(outputDirectory.ToString(), "wwwroot", "_framework", WasmBootConfigFileName));

            // And that it has been labelled as a dynamic assembly in the boot.json
            var lazyAssemblies = bootJson.resources.lazyAssembly;
            var assemblies = bootJson.resources.assembly;

            lazyAssemblies.Should().NotBeNull();

            lazyAssemblies.Keys.Should().Contain("RazorClassLibrary.wasm");
            assemblies.Keys.Should().NotContain("RazorClassLibrary.wasm");

            // App assembly should not be lazy loaded
            lazyAssemblies.Keys.Should().NotContain("blazorwasm.wasm");
            assemblies.Keys.Should().Contain("blazorwasm.wasm");
        }

        [RequiresMSBuildVersionFact("17.12", Reason = "Needs System.Text.Json 8.0.5")]
        public void Build_LazyLoadExplicitAssembly_InvalidAssembly()
        {
            // Arrange
            var testAppName = "BlazorWasmWithLibrary";
            var testInstance = CreateAspNetSdkTestAsset(testAppName);

            testInstance.WithProjectChanges((project) =>
            {
                var ns = project.Root.Name.Namespace;
                var itemGroup = new XElement(ns + "ItemGroup");
                itemGroup.Add(new XElement("BlazorWebAssemblyLazyLoad",
                    new XAttribute("Include", "RazorClassLibraryInvalid.wasm")));
                project.Root.Add(itemGroup);
            });

            // Assert
            var buildCommand = CreateBuildCommand(testInstance, "blazorwasm");
            ExecuteCommand(buildCommand).Should().Fail().And.HaveStdOutContaining("BLAZORSDK1001");
        }

        [RequiresMSBuildVersionFact("17.12", Reason = "Needs System.Text.Json 8.0.5")]
        public void Publish_LazyLoadExplicitAssembly_InvalidAssembly()
        {
            // Arrange
            var testAppName = "BlazorWasmWithLibrary";
            var testInstance = CreateAspNetSdkTestAsset(testAppName);

            testInstance.WithProjectChanges((project) =>
            {
                var ns = project.Root.Name.Namespace;
                var itemGroup = new XElement(ns + "ItemGroup");
                itemGroup.Add(new XElement("BlazorWebAssemblyLazyLoad",
                    new XAttribute("Include", "RazorClassLibraryInvalid.wasm")));
                project.Root.Add(itemGroup);
            });

            // Assert
            var publishCommand = new PublishCommand(Log, Path.Combine(testInstance.TestRoot, "blazorwasm"));
            publishCommand.Execute("/p:Configuration=Release").Should().Fail().And.HaveStdOutContaining("BLAZORSDK1001");
        }

        private static BootJsonData ReadBootJsonData(string path)
        {
            return BootJsonDataLoader.ParseBootData(path);
        }
    }
}
