using System.Collections.Generic;
using System.IO;

namespace SharpBSP
{
    public class BSPHeader
    {
        public string magic;
        public uint version;
        public List<BSPDirectoryEntry> directory;
        private BinaryReader BSP;

        public BSPHeader(BinaryReader BSP)
        {
            this.BSP = BSP;

            this.magic = ReadMagic();
            this.version = ReadVersion();
            this.directory = ReadLumps();
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== BSP Header =====\r\n";
            blob += ("Magic Number: " + magic + "\r\n");
            blob += ("BSP Version: " + version + "\r\n");
            blob += ("Header Directory:\r\n");
            int count = 0;
            foreach (BSPDirectoryEntry entry in directory)
            {
                blob += ("Lump " + count + ": " + entry.name + " Offset: " + entry.offset + " Length: " + entry.length + "\r\n");
                count++;
            }
            return blob;
        }

        private List<BSPDirectoryEntry> ReadLumps()
        {
            List<BSPDirectoryEntry> lumps = new List<BSPDirectoryEntry>();
            for (int i = 0; i < 17; i++)
            {
                lumps.Add(new BSPDirectoryEntry(BSP.ReadInt32(), BSP.ReadInt32()));
            }

            lumps[0].name = "Entities";
            lumps[1].name = "Textures";
            lumps[2].name = "Planes";
            lumps[3].name = "Nodes";
            lumps[4].name = "Leafs";
            lumps[5].name = "Leaf faces";
            lumps[6].name = "Leaf brushes";
            lumps[7].name = "Models";
            lumps[8].name = "Brushes";
            lumps[9].name = "Brush sides";
            lumps[10].name = "Vertexes";
            lumps[11].name = "Mesh vertexes";
            lumps[12].name = "Effects";
            lumps[13].name = "Faces";
            lumps[14].name = "Lightmaps";
            lumps[15].name = "Light volumes";
            lumps[16].name = "Vis data";

            return lumps;
        }

        private string ReadMagic()
        {
            BSP.BaseStream.Seek(0, SeekOrigin.Begin);
            return new string(BSP.ReadChars(4));
        }



        private uint ReadVersion()
        {
            BSP.BaseStream.Seek(4, SeekOrigin.Begin);
            return BSP.ReadUInt32();
        }
    }
}
