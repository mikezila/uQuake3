using UnityEngine;
using System;
using SharpBSP;
using System.Collections;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour
{
    public Material replacementTexture;
    public bool replaceTextures = false;
    public string mapName;
    private int faceCount = 0;

    public BSPMap map;

    void Start()
    {
        // Create a new BSPmap, which is an object that
        // represents the map and all its data as a whole
        map = new BSPMap("Assets/Resources/Maps/" + mapName);

        // Each face is its own gameobject
        foreach (Face face in map.faceLump.faces)
        {
            if (face.type == 2)
            {
                GenerateBezObject(face);
                faceCount++;
            }
            else if (face.type == 1)
            {
                GeneratePolygonObject(face);
                faceCount++;
            }
            else if (face.type == 3)
            {
                GeneratePolygonObject(face);
                faceCount++;
            }
            else
            {
                Debug.Log("Skipped Face " + faceCount.ToString() + " because it was not a polygon, mesh, or bez patch");
                faceCount++;
            }
        }
    }

    Mesh GenerateBezMesh(Face face, int patchNumber)
    {
        //Calculate how many patches there are using size[]
        //There are n_patchesX by n_patchesY patches in the grid, each of those
        //starts at a vert (i,j) in the overall grid
        int n_patchesX = ((face.size[0]) - 1) / 2;
        int n_patchesY = ((face.size[1]) - 1) / 2;


        //Calculate what [n,m] patch we want by using an index
        //called patchNumber  Think of patchNumber as if you 
        //numbered the patches left to right, top to bottom on
        //the grid in a piece of paper.
        int pxStep = 0;
        int pyStep = 0;
        for (int i = 0; i < patchNumber; i++)
        {
            pxStep++;
            if (pxStep == n_patchesX)
            {
                pxStep = 0;
                pyStep++;
            }
        }

        //Create an array the size of the grid, which is given by
        //size[] on the face object.
        Vector3[,] vertGrid = new Vector3[face.size[0], face.size[1]];

        //Read the verts for this face into the grid, making sure
        //that the final shape of the grid matches the size[] of
        //the face.
        int gridXstep = 0;
        int gridYstep = 0;
        int vertStep = face.vertex;
        for (int i = 0; i < face.n_vertexes; i++)
        {
            vertGrid[gridXstep, gridYstep] = map.vertexLump.verts[vertStep].position;
            vertStep++;
            gridXstep++;
            if (gridXstep == face.size[0])
            {
                gridXstep = 0;
                gridYstep++;
            }
        }

        //We now need to pluck out exactly nine vertexes to pass to our
        //teselate function, so lets calculate the starting vertex of the
        //3x3 grid of nine vertexes that will make up our patch.
        //we already know how many patches are in the grid, which we have
        //as n and m.  There are n by m patches.  Since this method will
        //create one gameobject at a time, we only need to be able to grab
        //one.  The starting vertex will be called vi,vj think of vi,vj as x,y
        //coords into the grid.
        int vi = 2 * pxStep;
        int vj = 2 * pyStep;
        //Now that we have those, we need to get the vert at [vi,vj] and then
        //the two verts at [vi+1,vj] and [vi+2,vj], and then [vi,vj+1], etc.
        //the ending vert will at [vi+2,vj+2]

        List<Vector3> bverts = new List<Vector3>();

        //Top row
        bverts.Add(vertGrid[vi, vj]);
        bverts.Add(vertGrid[vi + 1, vj]);
        bverts.Add(vertGrid[vi + 2, vj]);

        //Middle row
        bverts.Add(vertGrid[vi, vj + 1]);
        bverts.Add(vertGrid[vi + 1, vj + 1]);
        bverts.Add(vertGrid[vi + 2, vj + 1]);

        //Bottom row
        bverts.Add(vertGrid[vi, vj + 2]);
        bverts.Add(vertGrid[vi + 1, vj + 2]);
        bverts.Add(vertGrid[vi + 2, vj + 2]);

        //Now that we have our control grid, it's business as usual
        Mesh bezMesh = new Mesh();
        bezMesh.name = "BSPfacemesh (bez)";
        BezierPatch bezPatch = new BezierPatch(bverts);
        bezMesh.vertices = bezPatch.vertex.ToArray();
        bezMesh.triangles = bezPatch.fromFanIndexes.ToArray();
        bezMesh.RecalculateNormals();
        bezMesh.RecalculateBounds();
        return bezMesh;
    }

    void GenerateBezObject(Face face)
    {
        int numPatches = ((face.size[0] - 1) / 2) * ((face.size[1] - 1) / 2);

        for (int i = 0; i < numPatches; i++)
        {
            GameObject bezObject = new GameObject();
            bezObject.name = "BSPface (bez) " + faceCount.ToString();
            bezObject.AddComponent<MeshFilter>();
            bezObject.GetComponent<MeshFilter>().mesh = GenerateBezMesh(face, i);
            bezObject.AddComponent<MeshRenderer>();
            bezObject.AddComponent<MeshCollider>();
            bezObject.renderer.material.shader = Shader.Find("Diffuse");
        }
    }

    // This is deprecated, as it only handles faces with exactly one bez patch.
    GameObject GenerateBezModel(Face face)
    {
        GameObject faceObject = new GameObject("BSPface (bez) " + faceCount.ToString());
        Mesh worldFace = new Mesh();
        worldFace.name = "BSPfacemesh (bez)";

        // Create a bezpatch using the control points from the face
        // that bezpatch object will then have the verts and indexes inside
        // it, so grab them.  Get the uvs from the face itself like normal.
        List<Vector3> bverts = new List<Vector3>();
        int bstep = face.vertex;
        for (int i = 0; i < face.n_vertexes; i++)
        {
            bverts.Add(map.vertexLump.verts[bstep].position);
            bstep++;
        }
        BezierPatch bezPatch = new BezierPatch(bverts);

        worldFace.vertices = bezPatch.vertex.ToArray();
        worldFace.triangles = bezPatch.fromFanIndexes.ToArray();
        //worldFace.SetTriangleStrip(bezPatch.indexes.ToArray(),0);

        faceObject.AddComponent<MeshFilter>();
        faceObject.GetComponent<MeshFilter>().mesh = worldFace;
        faceObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        //faceObject.GetComponent<MeshFilter>().mesh.Optimize();
        faceObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        faceObject.AddComponent<MeshRenderer>();
        faceObject.AddComponent<MeshCollider>();
        faceObject.renderer.material.shader = Shader.Find("Diffuse");

        faceObject.transform.parent = gameObject.transform;
        return faceObject;
    }

    // This takes one face and returns a gameobject complete with
    // mesh, renderer, material with texture, and collider.
    GameObject GeneratePolygonObject(Face face)
    {
        GameObject faceObject = new GameObject("BSPface " + faceCount.ToString());
        Mesh worldFace = new Mesh();
        worldFace.name = "BSPfacemesh";

        // Rip verts, uvs, and normals
        // I have ripping normals commented because it looks
        // like it's better to just let Unity recalculate them for us.
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        //List<Vector3> normals = new List<Vector3>();
        int vstep = face.vertex;
        for (int i = 0; i < face.n_vertexes; i++)
        {
            verts.Add(map.vertexLump.verts[vstep].position);
            uvs.Add(map.vertexLump.verts[vstep].texcoord);
            // normals.Add(map.vertexLump.verts[vstep].normal);
            vstep++;
        }

        // add the verts, uvs, and normals we ripped to the gameobjects mesh filter
        worldFace.vertices = verts.ToArray();
        //worldFace.normals = normals.ToArray();

        // Add the texture co-ords (or UVs) to the face/mesh
        worldFace.uv = uvs.ToArray();

        // Rip meshverts / triangles
        List<int> mverts = new List<int>();
        int mstep = face.meshvert;
        for (int i = 0; i < face.n_meshverts; i++)
        {
            mverts.Add(map.meshvertLump.meshVerts[mstep]);
            mstep++;
        }

        // add the meshverts to the object being built
        worldFace.triangles = mverts.ToArray();

        // bring it all together.
        // I'm not sure if it's needed to call recalculatebound and optimze
        // but I guess it can't hurt.  Recalculating normals gives better results
        // than using the ones from the map, but I could be ripping them/converting
        // them wrong.  Will investigate later maybe.  Working as-is for now.
        faceObject.AddComponent<MeshFilter>();
        faceObject.GetComponent<MeshFilter>().mesh = worldFace;
        faceObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        faceObject.GetComponent<MeshFilter>().mesh.Optimize();
        faceObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        faceObject.AddComponent<MeshRenderer>();
        faceObject.AddComponent<MeshCollider>();

        //Use a shader that has transparency and you'll get it,
        //though some textures that shouldn't be transparent end up
        //having transparent pieces, so I recommend using diffuse for
        //now an adding it with some smarts later as needed.
        //0.15f is a good value to emulate what it looks like in quake3
        //faceObject.renderer.material.shader = Shader.Find("Transparent/Cutout/Diffuse");
        //faceObject.renderer.material.SetFloat("_Cutoff", 0.15f);

        //Use a diffuse shader for now.  simple.
        faceObject.renderer.material.shader = Shader.Find("Diffuse");

        // Create a material and add it to the object you put this script on and it'll be used
        // on all of the faces.  This is a stand-in tex.
        if (replaceTextures)
        {
            faceObject.renderer.material = replacementTexture;
        }
        else
        {
            UnityEngine.Texture tex = (UnityEngine.Texture)Resources.Load(map.textureLump.textures[face.texture].name.ToString());
            faceObject.renderer.material.mainTexture = tex;
        }

        faceObject.transform.parent = gameObject.transform;
        return faceObject;
    }



}


