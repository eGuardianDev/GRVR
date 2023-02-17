from math import pi, sin, cos


from direct.showbase.ShowBase import ShowBase

from direct.task import Task

from Cube import Cuber
from array import *

import API as api

import threading

dataFromAPI = [[]]

class MyApp(ShowBase):
    global Data
    global Cubes
    def __init__(self):
      global Cubes
      ShowBase.__init__(self)
      # Load the environment model.
    #  self.scene = self.loader.loadModel("models/environment")
      # Reparent the model to render.
    # self.scene.reparentTo(self.render)
      # Apply scale and position transforms on the model.
    #  self.scene.setScale(0.25, 0.25, 0.25)
    #  self.scene.setPos(-8, 42, 0)
      
      Cubes = []
        
   #   cube = Cuber(1,4,1)
   #   cube.SetNameOfPart("Leg")
   #   cube.SetRender(self.render)
   #   cube.SetPos(2,1,1)
      
    #  cube.SetRot(45,0,90)
  
      self.taskMgr.add(self.spinCameraTask, "RandomTask")


    def spinCameraTask(self, task):
      global Cubes
      exist = False
      for y in (api.Data):
        for x in Cubes:
            if(y[0] == x.ReturnNameOfPart()):
                exist = True  
                x.SetPos(float(y[1]), float(y[2]),float(y[3]))
                x.SetRot(float(y[4]),float(y[5]),float(y[6]))
                print("Changin data")
        if(exist == False):
          cube = Cuber(1,1,2)
          cube.SetNameOfPart(y[0])
          cube.SetRender(self.render)
          Cubes.append(cube)
        
   # print(api.Data)
      return Task.cont
       #base.disableMouse()
        #self.camera.setPos(1, -20, 7)
       # self.camera.setHpr(0, -10,0)
       
  
def startAPI():
  api.Loop()




if __name__ == "__main__":

 #threading._start_new_thread( startAPI, () )
# threading._start_new_thread( startAPP, () ) # creating thread

 t2 = threading.Thread(target=startAPI)
 t2.start()# starting thread 2
# t2.start()    
 app = MyApp()
 app.run()
