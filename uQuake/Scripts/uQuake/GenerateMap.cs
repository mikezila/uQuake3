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
                if (face.n_vertexes > 9)
                {
                    continue;
                }
                GenerateBezModel(face);
                faceCount++;
                continue;
            }
            else if (face.type == 1 || face.type == 3)
            {
                GeneratePolygonModel(face);
                faceCount++;
                continue;
            }
            faceCount++;
        }
    }

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
        Debug.Log(bezPatch.PrintInfo());

        worldFace.vertices = bezPatch.vertex.ToArray();
        //worldFace.triangles = bezPatch.rowIndexes.ToArray();
        worldFace.SetTriangleStrip(bezPatch.indexes.ToArray(),0);

        faceObject.AddComponent<MeshFilter>();
        faceObject.GetComponent<MeshFilter>().mesh = worldFace;
        faceObject.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        //faceObject.GetComponent<MeshFilter>().mesh.Optimize();
        faceObject.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        faceObject.AddComponent<MeshRenderer>();
        //faceObject.AddComponent<MeshCollider>();
        faceObject.renderer.material.shader = Shader.Find("Diffuse");

        faceObject.transform.parent = gameObject.transform;
        return faceObject;
    }

    // This takes one face and returns a gameobject complete with
    // mesh, renderer, material with texture, and collider.
    GameObject GeneratePolygonModel(Face face)
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

        //Use a shader that has transparency for the alpha bits
        //0.15f is a good value to emulate what it looks like in quake3
        faceObject.renderer.material.shader = Shader.Find("Transparent/Cutout/Diffuse");
        faceObject.renderer.material.SetFloat("_Cutoff", 0.15f);

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


