using System;
using UnityEngine;
using System.IO;

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
        public MeshvertLump meshvertLump;
        public LightmapLump lightmapLump;

        public BSPMap(string filename)
        {

            // Open the .bsp for reading
            BSP = new BinaryReader(File.Open(filename, FileMode.Open));
            
            // Read our header and lumps
            ReadHeader();
            ReadEntities();
            ReadTextures();
            ReadVertexes();
            ReadFaces();
            ReadMeshVerts();
            ReadLightmaps();
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
            BSP.BaseStream.Seek(header.directory[0].offset, SeekOrigin.Begin);
            entityLump = new EntityLump(new String(BSP.ReadChars(header.directory[0].length)));
        }

        private void ReadTextures()
        {
            // This calculates the number of textures in the lump, and creates a new texture
            // object inside of the texturelump's list for each of them.
            // Note that these aren't actually the texture graphics themselves, they're definitions
            // for getting the texture from an external source.
            textureLump = new TextureLump();
            BSP.BaseStream.Seek(header.directory[1].offset, SeekOrigin.Begin);
            // A texture is 72 bytes, so we use 72 to calculate the number of textures in the lump
            int textureCount = header.directory[1].length / 72;
            for (int i = 0; i < textureCount; i++)
            {
                textureLump.textures.Add(new Texture(new string(BSP.ReadChars(64)), BSP.ReadInt32(), BSP.ReadInt32()));
            }
        }



        private void ReadVertexes()
        {
            // Calc how many verts there are, them rip them into the vertexLump
            vertexLump = new VertexLump();
            BSP.BaseStream.Seek(header.directory[10].offset, SeekOrigin.Begin);
            // A vertex is 44 bytes, so use that to calc how many there are using the lump length from the header
            int vertCount = header.directory[10].length / 44;
            for (int i = 0; i < vertCount; i++)
            {
                vertexLump.verts.Add(new Vertex(new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle(), new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), BSP.ReadBytes(4)));
            }

        }

        private void ReadFaces()
        {
            faceLump = new FaceLump();
            BSP.BaseStream.Seek(header.directory[13].offset, SeekOrigin.Begin);
            // A face is 104 bytes of data, so the count is lenght of the lump / 104.
            int faceCount = header.directory[13].length / 104;
            for (int i = 0; i < faceCount; i++)
            {
                // This is pretty fucking intense.
                faceLump.faces.Add(new Face(BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32(), new int[] { BSP.ReadInt32(), BSP.ReadInt32() }, new int[] { BSP.ReadInt32(), BSP.ReadInt32() }, new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), new Vector3[] { new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()) }, new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), new int[] { BSP.ReadInt32(), BSP.ReadInt32() }));
            }
        }



        private void ReadMeshVerts()
        {
            meshvertLump = new MeshvertLump();
            BSP.BaseStream.Seek(header.directory[11].offset, SeekOrigin.Begin);
            // a meshvert is just a 4-byte int, so there are lumplength/4 meshverts
            int meshvertCount = header.directory[11].length / 4;
            for (int i = 0; i < meshvertCount; i++)
            {
                meshvertLump.meshVerts.Add(BSP.ReadInt32());
            }
        }

        // Lightmaps are broken right now, this code may be to blame, maybe not.
        private void ReadLightmaps()
        {
            lightmapLump = new LightmapLump();
            BSP.BaseStream.Seek(header.directory[14].offset, SeekOrigin.Begin);
            // a lightmap is 49152 bytes.  pretty big.  there are length/49152 lightmaps in the lump
            int lmapCount = header.directory[14].length / 49152;
            for (int i = 0; i < lmapCount; i++)
            {
                byte[] colors = BSP.ReadBytes(49152);
                lightmapLump.AddLight(colors);
            }
        }
    }
}
