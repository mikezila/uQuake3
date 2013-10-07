using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBSP
{
    class BezierPatch
    {
        public List<Vector3> vertex = new List<Vector3>();
        public List<int> indexes = new List<int>();
        List<int> trianglesPerRow = new List<int>();
        public List<int> rowIndexes = new List<int>();
        List<Vector3> controls = new List<Vector3>();

        public BezierPatch(List<Vector3> controls)
        {
            this.controls = controls;
            tesselate(10);

        }

        public string PrintInfo()
        {
            string blob = "\r\n=== Patch =====\r\nVerts: ";
            foreach (Vector3 vect in vertex)
            {
                blob += vect.ToString();
            }
            blob += "\r\nIndexes: ";
            foreach (int index in indexes)
            {
                blob += index + " ";
            }
            blob += "\r\nRowIndexes: ";
            foreach (int index in rowIndexes)
            {
                blob += index + " ";
            }
            return blob;
        }

        private void tesselate(int L)
        {
            // The number of vertices along a side is 1 + num edges
            int L1 = L + 1;

            // Compute the vertices
            for (int i = 0; i <= L; ++i)
            {
                double a = (double)i / L;
                double b = 1 - a;

                vertex.Add(
                    controls[0] * (float)(b * b) +
                    controls[3] * (float)(2 * b * a) +
                    controls[6] * (float)(a * a));
            }

            for (int i = 1; i <= L; ++i)
            {
                double a = (double)i / L;
                double b = 1.0 - a;

                List<Vector3> temp = new List<Vector3>();
                int j;
                for (j = 0; j < 3; ++j)
                {
                    int k = 3 * j;
                    temp.Add(
                        controls[k + 0] * (float)(b * b) +
                        controls[k + 1] * (float)(2 * b * a) +
                        controls[k + 2] * (float)(a * a));
                }

                for (j = 0; j <= L; ++j)
                {
                    double c = (double)j / L;
                    double d = 1.0 - c;
                    vertex.Add(
                        temp[0] * (float)(d * d) +
                        temp[1] * (float)(2 * b * c) +
                        temp[2] * (float)(c * a));
                }
            }


            // Compute the indices
            int row;

            for (row = 0; row < L; ++row)
            {
                for (int col = 0; col <= L; ++col)
                {
                    indexes.Add(row * L1 + col);
                    indexes.Add((row + 1) * L1 + col);
                }
            }

            for (row = 0; row < L; ++row)
            {
                trianglesPerRow.Add(2 * L1);
                rowIndexes.Add(indexes[row * 2 * L1]);
            }
        }
    }
}
