
import sys
import math
import threading
from direct.showbase.ShowBase import ShowBase
from pandac.PandaModules import GeomVertexFormat, GeomVertexData, GeomVertexWriter, GeomTriangles, Geom, GeomNode, NodePath, GeomPoints

class CubeMaker:
    def __init__(self, x=1, y=1, z=1):
        # self.smooth = True/False
        # self.uv = True/False or Spherical/Box/...
        # self.color = Method1/Method2/...
        # self.subdivide = 0/1/2/...
        self.x =x
        self.y= y
        self.z= z



    def generate(self):
        format = GeomVertexFormat.getV3()
        data = GeomVertexData("Data", format, Geom.UHStatic)
        vertices = GeomVertexWriter(data, "vertex")

        size = self.x
        long = self.y
                        #  x      y      z
        vertices.addData3f(-size, -size, -size)
        vertices.addData3f(+size, -size, -size)
        vertices.addData3f(-size, +size *long, -size)
        vertices.addData3f(+size, +size *long, -size)
        vertices.addData3f(-size, -size, +size)
        vertices.addData3f(+size, -size, +size)
        vertices.addData3f(-size, +size *long, +size)
        vertices.addData3f(+size, +size *long, +size)

        triangles = GeomTriangles(Geom.UHStatic)

        def addQuad(v0, v1, v2, v3):
            triangles.addVertices(v0, v1, v2)
            triangles.addVertices(v0, v2, v3)
            triangles.closePrimitive()

        addQuad(4, 5, 7, 6) # Z+
        addQuad(0, 2, 3, 1) # Z-
        addQuad(3, 7, 5, 1) # X+
        addQuad(4, 6, 2, 0) # X-
        addQuad(2, 6, 7, 3) # Y+
        addQuad(0, 1, 5, 4) # Y+

        geom = Geom(data)
        geom.addPrimitive(triangles)

        node = GeomNode("CubeMaker")
        node.addGeom(geom)

        return NodePath(node)


    def ReturnNameOfPart(self):
        return self.PartName

class Cuber:
    def __init__(self, x,y,z):
        self.cube = CubeMaker(x,y,z).generate()
        self.PartName = "Undefined"

    def ReturnNameOfPart(self):
        return self.PartName
    def SetNameOfPart(self, name):
        self.PartName=  name

    def SetRender(self, render):
        self.cube.reparentTo(render)

    def GetPos(self):
        return self.cube.getPos()

    def SetPos(self,x,y,z):
        self.cube.setPos(x,y,z)
    def GetRot(self):
        return self.cube.getHpr()

    def SetRot(self, x,y,z):
        self.cube.setHpr(x,y,z)



