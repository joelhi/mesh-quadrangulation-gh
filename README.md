# Mesh Quadrangulation

### Overview

> **Warning**
> 
> This repo is still very much work in progress.

A grasshopper component to quadrangulate tri-meshes by merging faces using a graph-matching approach.

This stems from the need to get quad based meshes for FE-analysis. Many good tools exist to do smooth adaptable triangulations in gh but few
offer "sofistik" style FE-suitable meshes. This can be done through the process of *Triangulation -> Quadrangulation -> Smoothing*.

This is quite work in progress still, and may be extended to feature more graph based mesh processing algorithms in the future beyond what's needed for quadrangulation.

An example of the process is shown below.

![Example](img/quadrangulation2.gif)


### Contents

The repo has two projects. 

- **MeshGraphLib**
- **MeshQuadrangulationGH**

The first one features a graph data structure and the processing algorithms, along with some converion helpers to and from Rhino geometry.

The second is the gh-plugin, which for now only has one component: 'Quadrangulate Meshes'


### Todo

- [x] Base graph structure
- [x] Quadrangulation algorithm
- [ ] Handle loops (faces) in graph structure
- [ ] Catmull-Clark algorithm (with option to fix points) for smoothing
- [ ] Handle non-convex matching cases.
- [ ] Expose explicit steps in process as gh components
