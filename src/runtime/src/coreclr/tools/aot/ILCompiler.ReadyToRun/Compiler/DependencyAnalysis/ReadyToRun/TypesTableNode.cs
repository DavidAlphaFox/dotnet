// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using Internal;
using Internal.NativeFormat;
using Internal.Text;
using Internal.TypeSystem;
using Internal.TypeSystem.Ecma;

namespace ILCompiler.DependencyAnalysis.ReadyToRun
{
    public class TypesTableNode : ModuleSpecificHeaderTableNode
    {
        public TypesTableNode(EcmaModule module) : base(module)
        {
        }

        protected override string ModuleSpecificName => "__ReadyToRunAvailableTypesTable__";

        public override ObjectData GetData(NodeFactory factory, bool relocsOnly = false)
        {
            // This node does not trigger generation of other nodes.
            if (relocsOnly)
                return new ObjectData(Array.Empty<byte>(), Array.Empty<Relocation>(), 1, new ISymbolDefinitionNode[] { this });

            NativeWriter writer = new NativeWriter();
            Section section = writer.NewSection();

            VertexHashtable typesHashtable = new VertexHashtable();
            section.Place(typesHashtable);

            ReadyToRunTableManager r2rManager = (ReadyToRunTableManager)factory.MetadataManager;
            foreach (TypeInfo<TypeDefinitionHandle> defTypeInfo in r2rManager.GetDefinedTypes(_module))
            {
                TypeDefinitionHandle defTypeHandle = defTypeInfo.Handle;
                int hashCode = 0;
                for (; ; )
                {
                    TypeDefinition defType = defTypeInfo.MetadataReader.GetTypeDefinition(defTypeHandle);
                    string namespaceName = defTypeInfo.MetadataReader.GetString(defType.Namespace);
                    string typeName = defTypeInfo.MetadataReader.GetString(defType.Name);
                    hashCode ^= VersionResilientHashCode.NameHashCode(namespaceName, typeName);
                    if (!defType.Attributes.IsNested())
                    {
                        break;
                    }
                    defTypeHandle = defType.GetDeclaringType();
                }
                typesHashtable.Append(unchecked((uint)hashCode), section.Place(new UnsignedConstant(((uint)MetadataTokens.GetRowNumber(defTypeInfo.Handle) << 1) | 0)));
            }

            foreach (TypeInfo<ExportedTypeHandle> expTypeInfo in r2rManager.GetExportedTypes(_module))
            {
                ExportedTypeHandle expTypeHandle = expTypeInfo.Handle;
                int hashCode = 0;
                for (; ;)
                {
                    ExportedType expType = expTypeInfo.MetadataReader.GetExportedType(expTypeHandle);
                    string namespaceName = expTypeInfo.MetadataReader.GetString(expType.Namespace);
                    string typeName = expTypeInfo.MetadataReader.GetString(expType.Name);
                    hashCode ^= VersionResilientHashCode.NameHashCode(namespaceName, typeName);
                    if (expType.Implementation.Kind != HandleKind.ExportedType)
                    {
                        // Not a nested class
                        break;
                    }
                    expTypeHandle = (ExportedTypeHandle)expType.Implementation;
                }
                typesHashtable.Append(unchecked((uint)hashCode), section.Place(new UnsignedConstant(((uint)MetadataTokens.GetRowNumber(expTypeInfo.Handle) << 1) | 1)));
            }

            MemoryStream writerContent = new MemoryStream();
            writer.Save(writerContent);

            return new ObjectData(
                data: writerContent.ToArray(),
                relocs: null,
                alignment: 8,
                definedSymbols: new ISymbolDefinitionNode[] { this });
        }

        public override int ClassCode => -944318825;
    }
}
