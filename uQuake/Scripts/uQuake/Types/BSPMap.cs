using System;
using System.IO;
using Ionic.Zip;
using UnityEngine;

namespace SharpBSP
{
    public class BSPMap
    {
        // This is the reader that seeks around the map
        // and grabs the data.
        private BinaryReader BSP;

        // The header contains the directory of lumps
        public BSPHeader header;

        // These are the objects that hold data extraced from the lumps
        // each one has public fields that hold the data in them
        // Note that there are many lumps we don't need, so they
        // aren't processed.  If you want a tool to parse a .bsp
        // more throughly, check my github/google for "SharpBSP".
        public EntityLump entityLump;
        public TextureLump textureLump;
        public VertexLump vertexLump;
        public FaceLump faceLump;
        public LightmapLump lightmapLump;

        public BSPMap(string filename, bool loadFromPK3)
        {
            filename = "maps/" + filename;

            // Look through all available .pk3 files to find the map.
            // The first map will be used, which doesn't match Q3 behavior, as it would use the last found map, but eh.
            if (loadFromPK3)
            {
                foreach (var info in new DirectoryInfo("Assets/baseq3/").GetFiles())
                {
                    if (info.Name.EndsWith(".PK3") || info.Name.EndsWith(".pk3"))
                    {
                        using (ZipFile pk3 = ZipFile.Read("Assets/baseq3/" + info.Name))
                        {
                            if (pk3.ContainsEntry(filename))
                            {
                                var entry = pk3 [filename];
                                using (var mapstream = pk3 [filename].OpenReader())
                                {
                                    var ms = new MemoryStream();
                                    entry.Extract(ms);
                                    BSP = new BinaryReader(ms);
                                }
                            }
                        }
                    }
                }
            } else
            {
                BSP = new BinaryReader(File.Open(filename, FileMode.Open));
            }
            
            // Read our header and lumps
            ReadHeader();
            ReadEntities();
            ReadTextures();
            ReadVertexes();
            ReadFaces();
            ReadMeshVerts();
            ReadLightmaps();

            // Comb through all of the available .pk3 files and load in any textures needed by the current map.
            // Textures in higher numbered .pk3 files will be used over ones in lower ones.
            foreach (var info in new DirectoryInfo("Assets/baseq3/").GetFiles())
            {
                if (info.Name.EndsWith(".pk3") || info.Name.EndsWith(".PK3"))
                    textureLump.PullInTextures(info.Name);
            }

            BSP.Close();
        }

        private void ReadHeader()
        {
            header = new BSPHeader(BSP);
        }

        private void ReadEntities()
        {
            // Load Entity String
            // It's just one big mutha' string with a length defined in the header.
            // This is the only lump that may not end on an even four-byte block
            BSP.BaseStream.Seek(header.Directory [0].Offset, SeekOrigin.Begin);
            entityLump = new EntityLump(new String(BSP.ReadChars(header.Directory [0].Length)));
        }

        private void ReadTextures()
        {
            // This calculates the number of textures in the lump, and creates a new texture
            // object inside of the texturelump's list for each of them.
            // Note that these aren't actually the texture graphics themselves, they're definitions
            // for getting the texture from an external source.
            BSP.BaseStream.Seek(header.Directory [1].Offset, SeekOrigin.Begin);
            // A texture is 72 bytes, so we use 72 to calculate the number of textures in the lump
            int textureCount = header.Directory [1].Length / 72;
            textureLump = new TextureLump(textureCount);
            for (int i = 0; i < textureCount; i++)
            {
                textureLump.Textures [i] = new Texture(new string(BSP.ReadChars(64)), BSP.ReadInt32(), BSP.ReadInt32());
            }
        }

        private void ReadVertexes()
        {
            // Calc how many verts there are, them rip them into the vertexLump
            BSP.BaseStream.Seek(header.Directory [10].Offset, SeekOrigin.Begin);
            // A vertex is 44 bytes, so use that to calc how many there are using the lump length from the header
            int vertCount = header.Directory [10].Length / 44;
            vertexLump = new VertexLump(vertCount);
            for (int i = 0; i < vertCount; i++)
            {
                vertexLump.Verts [i] = new Vertex(new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle(), new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), BSP.ReadBytes(4));
            }

        }

        private void ReadFaces()
        {
            BSP.BaseStream.Seek(header.Directory [13].Offset, SeekOrigin.Begin);
            // A face is 104 bytes of data, so the count is lenght of the lump / 104.
            int faceCount = header.Directory [13].Length / 104;
            faceLump = new FaceLump(faceCount);
            for (int i = 0; i < faceCount; i++)
            {
                // This is pretty fucking intense.
                faceLump.Faces [i] = new Face(BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), new int[]
                {
                    BSP.ReadInt32(),
                    BSP.ReadInt32()
                }, new int[]
                {
                    BSP.ReadInt32(),
                    BSP.ReadInt32()
                }, new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), new Vector3[]
                {
                    new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()),
                    new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle())
                }, new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), new int[]
                {
                    BSP.ReadInt32(),
                    BSP.ReadInt32()
                });
            }
        }

        private void ReadMeshVerts()
        {
            BSP.BaseStream.Seek(header.Directory [11].Offset, SeekOrigin.Begin);
            // a meshvert is just a 4-byte int, so there are lumplength/4 meshverts
            int meshvertCount = header.Directory [11].Length / 4;
            vertexLump.MeshVerts = new int[meshvertCount];
            for (int i = 0; i<meshvertCount; i++)
                vertexLump.MeshVerts [i] = BSP.ReadInt32();
        }

        private void ReadLightmaps()
        {
            BSP.BaseStream.Seek(header.Directory [14].Offset, SeekOrigin.Begin);
            // a lightmap is 49152 bytes.  pretty big.  there are length/49152 lightmaps in the lump
            int lmapCount = header.Directory [14].Length / 49152;
            lightmapLump = new LightmapLump(lmapCount);
            for (int i = 0; i < lmapCount; i++)
            {
                lightmapLump.Lightmaps [i] = LightmapLump.CreateLightmap(BSP.ReadBytes(49152));
            }
        }
    }
}
