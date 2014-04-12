using System.Collections.Generic;
using System.IO;

namespace SharpBSP
{
    public class BSPHeader
    {
		public BSPDirectoryEntry[] Directory {
			get;
			set;
		}

		public string Magic {
			get;
			private set;
		}

		public uint Version {
			get;
			private set;
		}

        private BinaryReader BSP;

		private const int LumpCount = 17;

        public BSPHeader(BinaryReader BSP)
        {
            this.BSP = BSP;

            ReadMagic();
            ReadVersion();
            ReadLumps();
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== BSP Header =====\r\n";
            blob += ("Magic Number: " + Magic + "\r\n");
            blob += ("BSP Version: " + Version + "\r\n");
            blob += ("Header Directory:\r\n");
            int count = 0;
            foreach (BSPDirectoryEntry entry in Directory)
            {
                blob += ("Lump " + count + ": " + entry.Name + " Offset: " + entry.Offset + " Length: " + entry.Length + "\r\n");
                count++;
            }
            return blob;
        }

        private void ReadLumps()
        {
            Directory = new BSPDirectoryEntry[LumpCount];
            for (int i = 0; i < 17; i++)
            {
                Directory[i] = new BSPDirectoryEntry(BSP.ReadInt32(), BSP.ReadInt32());
            }

            Directory[0].Name = "Entities";
			Directory[1].Name = "Textures";
			Directory[2].Name = "Planes";
			Directory[3].Name = "Nodes";
			Directory[4].Name = "Leafs";
			Directory[5].Name = "Leaf faces";
			Directory[6].Name = "Leaf brushes";
			Directory[7].Name = "Models";
			Directory[8].Name = "Brushes";
			Directory[9].Name = "Brush sides";
			Directory[10].Name = "Vertexes";
			Directory[11].Name = "Mesh vertexes";
			Directory[12].Name = "Effects";
			Directory[13].Name = "Faces";
			Directory[14].Name = "Lightmaps";
			Directory[15].Name = "Light volumes";
			Directory[16].Name = "Vis data";
        }

        private void ReadMagic()
        {
            BSP.BaseStream.Seek(0, SeekOrigin.Begin);
            Magic = new string(BSP.ReadChars(4));
        }



        private void ReadVersion()
        {
            BSP.BaseStream.Seek(4, SeekOrigin.Begin);
            Version = BSP.ReadUInt32();
        }
    }
}
