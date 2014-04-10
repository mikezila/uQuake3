using UnityEngine;
using System;
using SharpBSP;
using System.Collections;
using System.Collections.Generic;
using Ionic.Zip;
using System.IO;

public class GenerateMap : MonoBehaviour
{
    public Material replacementTexture;
    public bool useRippedTextures;
    public bool renderBezPatches;
    public string mapName;
    public bool mapIsInsidePK3;
    public bool applyLightmaps;
    public int tessellations = 5;
    private int faceCount = 0;
    private BSPMap map;

    void Awake()
    {        
        // Create a new BSPmap, which is an object that
        // represents the map and all its data as a whole
        if (mapIsInsidePK3)
            map = new BSPMap(mapName, true);
        else
            map = new BSPMap("Assets/baseq3/maps/" + mapName, false);

        // Each face is its own gameobject
        foreach (Face face in map.faceLump.faces)
        {
            if (face.type == 2)
            {
                if (renderBezPatches)
                {
                    GenerateBezObject(face);
                }
                faceCount++;
            } else if (face.type == 1)
            {
                GeneratePolygonObject(face);
                faceCount++;
            } else if (face.type == 3)
            {
                GeneratePolygonObject(face);
                faceCount++;
            } else
            {
                Debug.Log("Skipped Face " + faceCount.ToString() + " because it was not a polygon, mesh, or bez patch.");
                faceCount++;
            }
        }
        GC.Collect();
    }

    #region Object Generation
    // This makes gameobjects for every bez patch in a face
    // they are tessellated according to the "tessellations" field
    // in the editor
    void GenerateBezObject(Face face)
    {
        int numPatches = ((face.size [0] - 1) / 2) * ((face.size [1] - 1) / 2);

        for (int i = 0; i < numPatches; i++)
        {
            GameObject bezObject = new GameObject();
            bezObject.transform.parent = gameObject.transform;
            bezObject.name = "BSPface (bez) " + faceCount.ToString();
            //bezObject.AddComponent<MeshFilter>();
            bezObject.AddComponent<MeshFilter>().mesh = GenerateBezMesh(face, i);
            bezObject.AddComponent<MeshRenderer>();
            //bezObject.AddComponent<MeshCollider>();
            if (useRippedTextures)
                bezObject.renderer.material = FetchMaterial(face);
            else
                bezObject.renderer.material = replacementTexture;
        }
    }


    // This takes one face and generates a gameobject complete with
    // mesh, renderer, material with texture, and collider.
    void GeneratePolygonObject(Face face)
    {
        GameObject faceObject = new GameObject("BSPface " + faceCount.ToString());
        faceObject.transform.parent = gameObject.transform;
        // Our GeneratePolygonMesh will optimze and add the UVs for us
        faceObject.AddComponent<MeshFilter>().mesh = GeneratePolygonMesh(face);
        faceObject.AddComponent<MeshRenderer>();
        //faceObject.AddComponent<MeshCollider>();
        if (useRippedTextures)
        {
            faceObject.renderer.material = FetchMaterial(face);
        } else
        {
            faceObject.renderer.material = replacementTexture;
        }

    }
    #endregion

    #region Mesh Generation
    // This forms a mesh from a bez patch of your choice
    // from the face of your choice.
    // It's ready to render with tex coords and all.
    Mesh GenerateBezMesh(Face face, int patchNumber)
    {
        //Calculate how many patches there are using size[]
        //There are n_patchesX by n_patchesY patches in the grid, each of those
        //starts at a vert (i,j) in the overall grid
        //We don't actually need to know how many are on the Y length
        //but the forumla is here for historical/academic purposes
        int n_patchesX = ((face.size [0]) - 1) / 2;
        //int n_patchesY = ((face.size[1]) - 1) / 2;


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
        Vertex[,] vertGrid = new Vertex[face.size [0], face.size [1]];

        //Read the verts for this face into the grid, making sure
        //that the final shape of the grid matches the size[] of
        //the face.
        int gridXstep = 0;
        int gridYstep = 0;
        int vertStep = face.vertex;
        for (int i = 0; i < face.n_vertexes; i++)
        {
            vertGrid [gridXstep, gridYstep] = map.vertexLump.verts [vertStep];
            vertStep++;
            gridXstep++;
            if (gridXstep == face.size [0])
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

        //read texture/lightmap coords while we're at it
        //they will be tessellated as well.
        List<Vector2> uvs = new List<Vector2>();
        List<Vector2> uv2s = new List<Vector2>();

        //Top row
        bverts.Add(vertGrid [vi, vj].position);
        bverts.Add(vertGrid [vi + 1, vj].position);
        bverts.Add(vertGrid [vi + 2, vj].position);

        uvs.Add(vertGrid [vi, vj].texcoord);
        uvs.Add(vertGrid [vi + 1, vj].texcoord);
        uvs.Add(vertGrid [vi + 2, vj].texcoord);

        uv2s.Add(vertGrid [vi, vj].lmcoord);
        uv2s.Add(vertGrid [vi + 1, vj].lmcoord);
        uv2s.Add(vertGrid [vi + 2, vj].lmcoord);

        //Middle row
        bverts.Add(vertGrid [vi, vj + 1].position);
        bverts.Add(vertGrid [vi + 1, vj + 1].position);
        bverts.Add(vertGrid [vi + 2, vj + 1].position);

        uvs.Add(vertGrid [vi, vj + 1].texcoord);
        uvs.Add(vertGrid [vi + 1, vj + 1].texcoord);
        uvs.Add(vertGrid [vi + 2, vj + 1].texcoord);

        uv2s.Add(vertGrid [vi, vj + 1].lmcoord);
        uv2s.Add(vertGrid [vi + 1, vj + 1].lmcoord);
        uv2s.Add(vertGrid [vi + 2, vj + 1].lmcoord);

        //Bottom row
        bverts.Add(vertGrid [vi, vj + 2].position);
        bverts.Add(vertGrid [vi + 1, vj + 2].position);
        bverts.Add(vertGrid [vi + 2, vj + 2].position);

        uvs.Add(vertGrid [vi, vj + 2].texcoord);
        uvs.Add(vertGrid [vi + 1, vj + 2].texcoord);
        uvs.Add(vertGrid [vi + 2, vj + 2].texcoord);

        uv2s.Add(vertGrid [vi, vj + 2].lmcoord);
        uv2s.Add(vertGrid [vi + 1, vj + 2].lmcoord);
        uv2s.Add(vertGrid [vi + 2, vj + 2].lmcoord);

        //Now that we have our control grid, it's business as usual
        Mesh bezMesh = new Mesh();
        bezMesh.name = "BSPfacemesh (bez)";
        BezierMesh bezPatch = new BezierMesh(tessellations, bverts, uvs, uv2s);
        return bezPatch.mesh;
    }

    // Generate a mesh for a simple polygon/mesh face
    // It's ready to render with tex coords and all.
    Mesh GeneratePolygonMesh(Face face)
    {
        Mesh worldFace = new Mesh();
        worldFace.name = "BSPface (poly/mesh)";

        // Rip verts, uvs, and normals
        // I have ripping normals commented because it looks
        // like it's better to just let Unity recalculate them for us.
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector2> uv2s = new List<Vector2>();
        int vstep = face.vertex;
        for (int i = 0; i < face.n_vertexes; i++)
        {
            verts.Add(map.vertexLump.verts [vstep].position);
            uvs.Add(map.vertexLump.verts [vstep].texcoord);
            uv2s.Add(map.vertexLump.verts [vstep].lmcoord);
            vstep++;
        }

        // add the verts, uvs, and normals we ripped to the gameobjects mesh filter
        worldFace.vertices = verts.ToArray();

        // Add the texture co-ords (or UVs) to the face/mesh
        worldFace.uv = uvs.ToArray();
        worldFace.uv2 = uv2s.ToArray();

        // Rip meshverts / triangles
        List<int> mverts = new List<int>();
        int mstep = face.meshvert;
        for (int i = 0; i < face.n_meshverts; i++)
        {
            mverts.Add(map.meshvertLump.meshVerts [mstep]);
            mstep++;
        }

        // add the meshverts to the object being built
        worldFace.triangles = mverts.ToArray();

        // Let Unity do some heavy lifting for us
        worldFace.RecalculateBounds();
        worldFace.RecalculateNormals();
        worldFace.Optimize();

        return worldFace;
    }
    #endregion

    #region Material Generation
    // This returns a material with the correct texture for a given face
    Material FetchMaterial(Face face)
    {
        string texName = map.textureLump.textures [face.texture].name;

        // Load the primary texture for the face from the texture lump
        // The texture lump itself will have already looked over all
        // available .pk3 files and compiled a dictionary of textures for us.
        Texture2D tex;

        if (map.textureLump.ContainsTexture(texName))
        {
            tex = map.textureLump.GetTexture(texName);
        } else
        {
            return replacementTexture;
        }

        // Lightmapping is on, so calc the lightmaps
        if (face.lm_index >= 0 && applyLightmaps)
        {
            // Pick a shader that supports lightmaps
            Material bspMaterial = new Material(Shader.Find("Legacy Shaders/Lightmapped/Diffuse"));

            // LM experiment
            Texture2D lmap = map.lightmapLump.lightmaps [face.lm_index];
            lmap.Compress(true);
            lmap.Apply();

            // Put the textures in the shader.
            bspMaterial.mainTexture = tex;
            bspMaterial.SetTexture("_LightMap", lmap);

            return bspMaterial;
        } else
        { // Lightmapping is off, so don't.
            Material bspMaterial = new Material(Shader.Find("Diffuse"));
            bspMaterial.mainTexture = tex;
            return bspMaterial;
        }
        #endregion
    }

}


