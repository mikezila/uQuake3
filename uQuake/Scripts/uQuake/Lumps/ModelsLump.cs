using System.Collections.Generic;

namespace SharpBSP
{
    public class ModelsLump
    {
        public List<Model> models = new List<Model>();

        public ModelsLump()
        {
        }

        public string PrintInfo()
        {
            string blob = "\r\n=== Models =====\r\n";
            int count = 0;
            foreach (Model model in models)
            {
                blob += "Model " + count.ToString() + " Face: " + model.face.ToString() + " NumFaces: " + model.n_faces.ToString() + " Brush: " + model.brush.ToString() + " NumBrushes: " + model.n_brushes.ToString() + " Min: " + model.mins.ToString() + " Max: " + model.maxes.ToString() + "\r\n";
                count++;
            }
            return blob;
        }
    }
}
