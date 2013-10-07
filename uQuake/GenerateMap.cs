using UnityEngine;
using System;
using SharpBSP;
using System.Collections;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour
{
    public Material tempTex;

    public BSPMap map;

    void Start()
    {
        // Create a new BSPmap, which is an object that
        // represents the map and all its data as a whole
        map = new BSPMap("test.bsp");
        
        // For now each face is its own gameobject
        // for each face that isn't a bezier patch or
        // billboard, call the method that creates
        // an object.
        foreach (Face face in map.faceLump.faces)
        {
            if (face.type == 4 || face.type == 2)
            {
                continue;
            }
            GenerateModel(face);
        }
    }

    // This takes one face and returns a gameobject complete with
    // renderer, material, collider, and 
    GameObject GenerateModel(Face face)
    {
        GameObject faceObject = new GameObject("BSPface");
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
        // referse them before we add them, else our faces will point the
        // wrong way.
        mverts.Reverse();
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
        faceObject.renderer.material.shader = Shader.Find("Diffuse");

        // Create a material and add it to the object you put this script on and it'll be used
        // on all of the faces.  This is a stand-in tex.
        faceObject.renderer.material = tempTex;

        return faceObject;
    }

}
