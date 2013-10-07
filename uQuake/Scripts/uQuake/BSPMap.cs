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
        public EntityLump entityLump;
        public TextureLump textureLump;
        public PlaneLump planeLump;
        public VertexLump vertexLump;
        public FaceLump faceLump;
        public NodeLump nodeLump;
        public MeshvertLump meshvertLump;
        public ModelsLump modelsLump;

        public BSPMap(string filename)
        {
            try
            {
                BSP = new BinaryReader(File.Open(filename, FileMode.Open));
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: File not found.");
                throw;
            }

            ReadHeader();
            if (ValidateBSP())
            {
                Console.Write("BSP seems valid...");
                Debug.Log("BSP seems valid...");
            }
            else
            {
                Console.Write("Warning: BSP seems invalid/corrupt...");
            }

            ReadEntities();
            ReadTextures();
            ReadPlanes();
            ReadVertexes();
            ReadFaces();
            ReadNodes();
            ReadMeshVerts();
            ReadModels();
        }

        public void Log(string filename)
        {
            //This opens up a text file and dumps info about all the lumps in it.
            //The resulting file is pretty big, and it takes a bit to dump all the info.
            //Be patient.  Not all types are implimented yet, hence not all types are logged.
            StreamWriter log = new StreamWriter(File.OpenWrite(filename));

            log.Write("SharpBSP .bsp Report\r\n");

            log.Write(header.PrintInfo());
            log.Write(entityLump.PrintInfo());
            log.Write(textureLump.PrintInfo());
            log.Write(planeLump.PrintInfo());
            log.Write(nodeLump.PrintInfo());
            //Leafs
            //Leaffaces
            log.Write(modelsLump.PrintInfo());
            //Brushes
            //Brushsides
            log.Write(vertexLump.PrintInfo());
            log.Write(meshvertLump.PrintInfo());
            //Effects
            log.Write(faceLump.PrintInfo());
            //Lightmaps
            //Lightvols
            //Visdata

            // Wrap up log writing before we exit
            log.Flush();
            log.Dispose();
        }

        private void ReadHeader()
        {
            header = new BSPHeader(BSP);
        }

        private void ReadEntities()
        {
            // Load Entity String
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

        private void ReadPlanes()
        {
            // Calculate how many planes there are to read, then do the damn thing
            // makes a plane object in the planelump for each one of them
            planeLump = new PlaneLump();
            BSP.BaseStream.Seek(header.directory[2].offset, SeekOrigin.Begin);
            // A plane is 16 bytes, so we use 16 to calculate the number of planes in the lump
            int planeCount = header.directory[2].length / 16;
            for (int i = 0; i < planeCount; i++)
            {
                planeLump.planes.Add(new Plane(new Vector3(BSP.ReadSingle(), BSP.ReadSingle(), BSP.ReadSingle()), BSP.ReadSingle()));
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

        private void ReadNodes()
        {
            nodeLump = new NodeLump();
            BSP.BaseStream.Seek(header.directory[3].offset, SeekOrigin.Begin);
            // a node is 36 bytes of data, so length of lump/36 = number of nodes
            int nodeCount = header.directory[3].length / 36;
            for (int i = 0; i < nodeCount; i++)
            {
                nodeLump.nodes.Add(new Node(BSP.ReadInt32(),new int[] {BSP.ReadInt32(), BSP.ReadInt32()},new int[] {BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32()},new int[] {BSP.ReadInt32(), BSP.ReadInt32(), BSP.ReadInt32()}));
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

        private void ReadModels()
        {
            modelsLump = new ModelsLump();
            BSP.BaseStream.Seek(header.directory[7].offset, SeekOrigin.Begin);
            // model is 40 bytes of data, so lumplength/40 = number of models in the lump
            int modelCount = header.directory[7].length / 40;
            for (int i = 0; i < modelCount; i++)
            {
                modelsLump.models.Add(new Model(new Vector3(BSP.ReadSingle(),BSP.ReadSingle(),BSP.ReadSingle()),new Vector3(BSP.ReadSingle(),BSP.ReadSingle(),BSP.ReadSingle()),BSP.ReadInt32(),BSP.ReadInt32(),BSP.ReadInt32(),BSP.ReadInt32()));
            }
        }

        public BSPHeader GetHeader()
        {
            return header;
        }

        public bool ValidateBSP()
        {
            if (header.magic == "IBSP" && header.version == 46)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
